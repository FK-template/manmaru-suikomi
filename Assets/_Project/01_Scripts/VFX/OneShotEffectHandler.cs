using Manmaru.Audio;
using System.Collections;
using UnityEngine;

namespace Manmaru.Effect
{
    /// <summary>
    /// 単発再生専門のエフェクト制御クラス
    /// </summary>
    public class OneShotEffectHandler : MonoBehaviour
    {
        [Header("ビジュアルエフェクト設定")]
        [SerializeField] private ParticleSystem[] _particles;

        [Header("サウンドエフェクト設定")]
        [SerializeField] private AudioEventSO[] _sounds;
        [SerializeField] private AudioSource _source;

        [Header("状況確認用パラメータ設定")]
        [SerializeField] private float _checkIntervalSecond = 0.1f;

        void Start()
        {
            PlayAllEffects();
            StartCoroutine(CheckAndDestroyEffectsRoutine());
        }

        /// <summary>
        /// 設定されたビジュアルエフェクトとサウンドエフェクトを全て単発再生するメソッド
        /// </summary>
        private void PlayAllEffects()
        {
            PlayAllVisualEffects();
            PlayAllSoundEffects();
        }

        /// <summary>
        /// 設定されたビジュアルエフェクトを全て単発再生するメソッド
        /// </summary>
        private void PlayAllVisualEffects()
        {
            if (_particles == null || _particles.Length == 0) return;
            foreach (var p in _particles)
            {
                if (p == null || p.isPlaying) continue;
                p.Play();
            }
        }

        /// <summary>
        /// 設定されたサウンドエフェクトを全て単発再生するメソッド
        /// </summary>
        private void PlayAllSoundEffects()
        {
            if (_sounds == null || _sounds.Length == 0) return;
            foreach (var s in _sounds)
            {
                if (s == null) continue;
                s.PlayRandomPitch(_source);
            }
        }

        /// <summary>
        /// 全てのエフェクトの終了を監視し、完了後に自身を破棄するコルーチン
        /// </summary>
        /// <remarks>（パーティクルは粒が全て消えたら完了にしたいので、isPlayingではなくIsAlive(true)で状況チェック）</remarks>
        private IEnumerator CheckAndDestroyEffectsRoutine()
        {
            while (true)
            {
                bool isWorking = false;

                // サウンド再生状況チェック
                if (_source != null && _source.isPlaying)
                {
                    isWorking = true;
                }

                // パーティクル再生状況チェック
                foreach (var p in _particles)
                {
                    if (p != null && p.IsAlive(true))
                    {
                        isWorking = true;
                        break;
                    }
                }

                // 全ての再生が終わっていたら、ループ脱出
                if (!isWorking)
                {
                    break;
                }

                // ちょっと待ってからまたチェックする（負荷対策）
                yield return new WaitForSeconds(_checkIntervalSecond);
            }

            // 全て再生が終わったら、自身を破棄
            Destroy(gameObject);
        }
    }
}