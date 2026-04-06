using Manmaru.Audio;
using Manmaru.Collision;
using Manmaru.Movement;
using UnityEngine;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーのサウンド処理全般を制御するクラス
    /// </summary>
    public class PlayerSoundController : MonoBehaviour
    {
        [Header("スピーカー設定")]
        [SerializeField] private AudioSource _source;

        [Header("依存クラス設定（ジャンプ）")]
        [SerializeField] private JumpAction _jumpAction;
        [SerializeField] private AudioEventSO _jumpAudio;

        void Start()
        {
            // イベント購読設定
            _jumpAction.OnJumped += PlayJumpSound;
        }

        /// <summary>
        /// ジャンプ時のサウンドを再生するメソッド
        /// </summary>
        private void PlayJumpSound()
        {
            _jumpAudio.Play(_source);
        }
    }
}