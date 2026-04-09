using UnityEngine;

namespace Manmaru.Audio
{
    /// <summary>
    /// BGMとジングルの再生に必要なパラメータをまとめたデータアセット
    /// </summary>
    [CreateAssetMenu(fileName = "NewBGMData", menuName = "Manmaru/BGMData")]
    public class BGMDataSO : ScriptableObject
    {
        [Header("ジングル音源パラメータ設定")]
        [SerializeField] private AudioClip _jingleClip;
        [SerializeField][Range(0f, 1f)] private float _jingleVolume = 1.0f;

        [Header("BGM音源パラメータ設定")]
        [SerializeField] private AudioClip _bgmClip;
        [SerializeField][Range(0f, 1f)] private float _bgmVolume = 1.0f;
        [SerializeField] private bool _isBGMLoop = true;

        // プロパティ
        public AudioClip JingleClip => _jingleClip;
        public float JingleVolume => _jingleVolume;

        public AudioClip BGMClip => _bgmClip;
        public float BGMVolume => _bgmVolume;
        public bool IsBGMLoop => _isBGMLoop;
    }
}