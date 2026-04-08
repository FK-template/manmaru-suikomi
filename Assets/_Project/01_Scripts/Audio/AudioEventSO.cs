using UnityEngine;

namespace Manmaru.Audio
{
    /// <summary>
    /// ランダムな音源とピッチで音を再生する、汎用イベントのデータアセット
    /// </summary>
    [CreateAssetMenu(fileName = "NewAudioEvent", menuName = "Manmaru/AudioEvent")]

    public class AudioEventSO : ScriptableObject
    {
        [Header("鳴らしたい音源（※複数個あるとランダム再生）")]
        [SerializeField] private AudioClip[] _clips;

        [Header("音調整パラメータ設定")]
        [SerializeField] private float _maxPitch = 1.1f;
        [SerializeField] private float _minPitch = 0.9f;
        [SerializeField][Range(0f, 1f)] private float _volume = 1.0f;

        [Header("鳴らし方")]
        [SerializeField] private bool _isOneShot = true;

        // 外部に再生処理を依頼する場合用のプロパティ
        public float MaxPitch => _maxPitch;
        public float MinPitch => _minPitch;

        /// <summary>
        /// 任意のAudioSourceで、設定された音源とピッチからランダムに再生するメソッド
        /// </summary>
        public void PlayRandomPitch(AudioSource source)
        {
            if (_clips == null || _clips.Length == 0) return;

            // ランダムに音源を選択
            int randomIndex = Random.Range(0, _clips.Length);
            AudioClip clip = _clips[randomIndex];
            source.clip = clip;

            // ランダムにピッチずらし（音を生き生きとさせるため）
            source.pitch = Random.Range(_minPitch, _maxPitch);
            source.volume = _volume;

            // 再生処理
            if (_isOneShot) source.PlayOneShot(clip);
            else source.Play();
        }

        /// <summary>
        /// 任意のAudioSourceとピッチで、設定された音源からランダムに再生するメソッド
        /// </summary>
        public void PlaySetPitch(AudioSource source, float pitch)
        {
            if (_clips == null || _clips.Length == 0) return;

            // ランダムに音源を選択
            int randomIndex = Random.Range(0, _clips.Length);
            AudioClip clip = _clips[randomIndex];
            source.clip = clip;

            // ピッチを固定
            source.pitch = pitch;
            source.volume = _volume;

            // 再生処理
            if (_isOneShot) source.PlayOneShot(clip);
            else source.Play();
        }

        /// <summary>
        /// 任意のAudioSourceとピッチで、設定された音源を全て再生するメソッド
        /// </summary>
        /// <remarks>（※PlayOneShotのみ対応。Playは処理を受け付けない）</remarks>
        public void PlayAllWithSetPitch(AudioSource source, float pitch)
        {
            if (_clips == null || _clips.Length == 0 || !_isOneShot) return;

            // ピッチを固定
            source.pitch = pitch;
            source.volume = _volume;

            // 再生処理
            foreach (var c in _clips)
            {
                source.PlayOneShot(c);
            }
        }
    }
}