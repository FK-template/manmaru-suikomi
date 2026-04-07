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
        [Tooltip("単発音専用スピーカー")]
        [SerializeField] private AudioSource _oneShotSource;
        [Tooltip("すいこみ音専用スピーカー")]
        [SerializeField] private AudioSource _vacuumSource;

        [Header("ジャンプ音設定")]
        [SerializeField] private JumpAction _jumpAction;
        [SerializeField] private AudioEventSO _jumpAudio;

        [Header("すいこみ・はきだし音設定")]
        [SerializeField] private float _vacuumPitchDuration = 0.5f;
        [SerializeField] private PlayerAbilityController _abilityController;
        [SerializeField] private AudioEventSO _vacuumAudio;
        //[SerializeField] private AudioEventSO _capturedAudio;
        //[SerializeField] private AudioEventSO _shootAudio;

        [Header("被ダメージ音設定")]
        [SerializeField] private PlayerHealthController _playerHealthController;
        [SerializeField] private AudioEventSO _damagedAudio;

        // 内部変数
        private bool _isVacuuming = false;
        private float _vacuumTargetPitch;
        private float _vacuumPitchTimer;

        void Start()
        {
            // イベント購読設定
            _jumpAction.OnJumped += PlayJumpSound;
            _abilityController.OnVacuumStarted += PlayVacuumSound;
            _abilityController.OnVacuumFinished += StopVacuumSound;
            _playerHealthController.OnTookDamage += PlayDamagedSound;
        }

        void Update()
        {
            if (_isVacuuming && _vacuumPitchTimer < _vacuumPitchDuration)
            {
                UpVacuumPitch();
            }
        }

        /// <summary>
        /// ジャンプ時のサウンドを再生するメソッド
        /// </summary>
        private void PlayJumpSound()
        {
            _jumpAudio.Play(_oneShotSource);
        }

        /// <summary>
        /// すいこみ中のサウンドを再生するメソッド
        /// </summary>
        private void PlayVacuumSound()
        {
            _isVacuuming = true;
            _vacuumTargetPitch = _vacuumAudio.MaxPitch;
            _vacuumSource.pitch = _vacuumAudio.MinPitch;
            _vacuumPitchTimer = 0;

            _vacuumAudio.Play(_vacuumSource);
        }

        /// <summary>
        /// すいこみ中のサウンドのピッチを徐々に上げていくメソッド
        /// </summary>
        private void UpVacuumPitch()
        {
            _vacuumPitchTimer += Time.deltaTime;

            float t = _vacuumPitchTimer / _vacuumPitchDuration;
            _vacuumSource.pitch = Mathf.Lerp(_vacuumAudio.MinPitch, _vacuumTargetPitch, t);
        }

        /// <summary>
        /// すいこみ中のサウンドを停止し、ピッチを初期化するメソッド
        /// </summary>
        private void StopVacuumSound()
        {
            _vacuumSource.Stop();
            _vacuumSource.pitch = 1.0f;
            _isVacuuming = false;
        }

        /// <summary>
        /// 被ダメージ時のサウンドを再生するメソッド
        /// </summary>
        private void PlayDamagedSound()
        {
            _damagedAudio.Play(_oneShotSource);
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (_jumpAction != null) _jumpAction.OnJumped -= PlayJumpSound;
            if (_abilityController != null)
            {
                _abilityController.OnVacuumStarted -= PlayVacuumSound;
                _abilityController.OnVacuumFinished -= StopVacuumSound;
            }
            if (_playerHealthController != null) _playerHealthController.OnTookDamage -= PlayDamagedSound;
        }

    }
}