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

        [Header("音調整パラメータ")]
        [SerializeField] private float _maxPitch = 1.1f;
        [SerializeField] private float _minPitch = 0.9f;
        [SerializeField][Range(0f, 1f)] private float _volume = 1.0f;

        [Header("鳴らし方")]
        [SerializeField] private bool _isOneShot = true;

        /// <summary>
        /// 任意のAudioSourceで、設定された音源からランダムに再生するメソッド
        /// </summary>
        public void Play(AudioSource source)
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
    }
}