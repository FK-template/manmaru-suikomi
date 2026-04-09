using System.Collections;
using UnityEngine;

namespace Manmaru.Audio
{
    /// <summary>
    /// BGMとジングルの再生処理を行うクラス
    /// </summary>
    public class BGMPlayer : MonoBehaviour
    {
        [Header("スピーカー設定")]
        [SerializeField] private AudioSource _source;

        // 内部変数：動作中コルーチン
        private Coroutine _currentRoutine;

        // インスタンス設定
        public static BGMPlayer Instance { get; private set; }

        void Awake()
        {
            // インスタンス かつ 不死身
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// ジングルとBGMを順番に再生するメソッド
        /// </summary>
        public void PlayJingleAndBGM(BGMDataSO data)
        {
            if (_source == null || data == null) return;

            // 動作中の再生処理を停止
            if (_currentRoutine != null) StopCoroutine(_currentRoutine);
            if (_source.isPlaying) _source.Stop();

            _currentRoutine = StartCoroutine(PlayJingleAndBGMRoutine(data));
        }

        /// <summary>
        /// ジングル再生が終わった後、BGMの再生を開始するメソッド
        /// </summary>
        public IEnumerator PlayJingleAndBGMRoutine(BGMDataSO data)
        {
            // ジングルを再生し、ジングルの長さだけ待機
            if (data.JingleClip != null)
            {
                _source.volume = 1.0f;
                _source.PlayOneShot(data.JingleClip, data.JingleVolume);
                yield return new WaitForSeconds(data.JingleClip.length);
            }

            // BGMも、セットアップして再生
            if (data.BGMClip != null)
            {
                _source.clip = data.BGMClip;
                _source.volume = data.BGMVolume;
                _source.loop = data.IsBGMLoop;
                _source.Play();
            }
        }

        private void OnDestroy()
        {
            // インスタンス削除
            if (Instance == this) Instance = null;
        }
    }
}