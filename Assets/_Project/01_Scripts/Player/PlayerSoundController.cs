using Manmaru.Audio;
using Manmaru.Collision;
using Manmaru.Interaction;
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

        [Header("着地音設定")]
        [SerializeField] private PlayerMoveController _moveController;
        [SerializeField] private AudioEventSO _landAudio;

        [Header("すいこみ・はきだし音設定")]
        [SerializeField] private float _vacuumPitchDuration = 0.5f;
        [SerializeField] private PlayerAbilityController _abilityController;
        [SerializeField] private AudioEventSO _vacuumAudio;
        [SerializeField] private AudioEventSO _captureAudio;
        [SerializeField] private AudioEventSO _shootAudio;
        private CaptureTargetManager _captureTargetManager;

        // 内部変数
        private bool _isVacuuming = false;
        private float _vacuumTargetPitch;
        private float _vacuumPitchTimer;

        void Start()
        {
            _captureTargetManager = CaptureTargetManager.Instance;

            // イベント購読設定
            _jumpAction.OnJumped += PlayJumpSound;
            _moveController.OnLanded += PlayLandSound;
            _abilityController.OnVacuumStarted += PlayVacuumSound;
            _abilityController.OnVacuumFinished += StopVacuumSound;
            _captureTargetManager.OnCaptureFinished += PlayCaptureSound;
            _abilityController.OnShooted += PlayShootSound;
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
            _jumpAudio.PlayRandomPitch(_oneShotSource);
        }

        /// <summary>
        /// 着地時のサウンドを再生するメソッド
        /// </summary>
        private void PlayLandSound()
        {
            _landAudio.PlayRandomPitch(_oneShotSource);
        }

        /// <summary>
        /// すいこみ中のサウンドを再生するメソッド
        /// </summary>
        private void PlayVacuumSound()
        {
            _isVacuuming = true;
            _vacuumTargetPitch = _vacuumAudio.MaxPitch;
            _vacuumPitchTimer = 0;

            _vacuumAudio.PlaySetPitch(_vacuumSource, _vacuumAudio.MinPitch);
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
        /// すいこみ終わった瞬間、ほおばりカウントに応じてピッチを調整したサウンドを再生するメソッド
        /// </summary>
        private void PlayCaptureSound(ICapturable _)
        {
            float t = (float)_abilityController.CapturedCount / (float)_abilityController.CaptureCountLimit;
            float pitch = Mathf.Lerp(_captureAudio.MinPitch, _captureAudio.MaxPitch, t);

            _captureAudio.PlaySetPitch(_oneShotSource, pitch);
        }

        /// <summary>
        /// はきだし時、ほおばりカウントに応じてピッチを調整したサウンドを再生するメソッド
        /// </summary>
        private void PlayShootSound()
        {
            float t = (float)_abilityController.CapturedCount / (float)_abilityController.CaptureCountLimit;
            float pitch = Mathf.Lerp(_captureAudio.MinPitch, _captureAudio.MaxPitch, t);

            _shootAudio.PlayAllWithSetPitch(_oneShotSource, pitch);
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (_jumpAction != null) _jumpAction.OnJumped -= PlayJumpSound;
            if (_moveController != null) _moveController.OnLanded -= PlayLandSound;
            if (_abilityController != null)
            {
                _abilityController.OnVacuumStarted -= PlayVacuumSound;
                _abilityController.OnVacuumFinished -= StopVacuumSound;
                _abilityController.OnShooted -= PlayShootSound;
            }
            if (_captureTargetManager != null) _captureTargetManager.OnCaptureFinished -= PlayCaptureSound;
        }

    }
}