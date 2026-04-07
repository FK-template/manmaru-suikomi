using Manmaru.Ability;
using Manmaru.Interaction;
using Manmaru.VFX;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーの固有アクション全般を制御するクラス
    /// </summary>
    public class PlayerAbilityController : MonoBehaviour
    {
        [Header("入力設定")]
        [SerializeField] private InputActionReference _attackActionInput;

        [Header("ほおばり上限（実行中の変更は無効）")]
        [SerializeField] private int _captureCountLimit = 5;

        [Header("依存クラス設定")]
        [SerializeField] private PlayerStateManager _playerStateManager;
        [SerializeField] private VacuumAction _vacuumAction;
        [SerializeField] private ShootAction _shootAction;
        [SerializeField] private MouthfulStock _mouthfulStock;
        [SerializeField] private PlayerVisualHandler _playerVisualController;
        [SerializeField] private VacuumEffectHandler _vacuumEffectController;
        private CaptureTargetManager _captureTargetManager;

        // 公開変数：サウンド用イベント
        public Action OnVacuumStarted;
        public Action OnVacuumFinished;
        public Action OnCaptureFinished;

        // 内部変数
        private bool _needToRelease = false;

        void Start()
        {
            _mouthfulStock.SetCountLimit(_captureCountLimit);
            _captureTargetManager = CaptureTargetManager.Instance;

            // イベント購読設定
            _captureTargetManager.OnCaptureFinished += _mouthfulStock.AddCapturedCount;
            _captureTargetManager.OnAllCapturesFinished += ReadyToShoot;
        }

        void Update()
        {
            // ゲームオーバー状態 or ノックバック状態なら、入力を受け付けない
            if (_playerStateManager.CurrentState == PlayerStateManager.PlayerState.Damaged ||
                _playerStateManager.CurrentState == PlayerStateManager.PlayerState.Dead) return;

            // すいこみ・はきだし（すいこみ開始条件：ほおばり状態でない かつ 入力ロック状態でないとき）
            if (_playerStateManager.CurrentState == PlayerStateManager.PlayerState.Mouthful)
            {
                UpdateShootStatus();
            }
            else
            {
                // 入力ロック判定
                if (_needToRelease)
                {
                    CheckUnlockInput();
                }
                else
                {
                    UpdateCaptureStatus();
                }
            }
        }

        /// <summary>
        /// 入力に応じてすいこみ処理を行うメソッド
        /// </summary>
        private void UpdateCaptureStatus()
        {
            // Attackボタンを押した瞬間に、ひきよせ状態でなければ、すいこみ状態に遷移
            if (_attackActionInput.action.WasPressedThisFrame()
                && _playerStateManager.CurrentState != PlayerStateManager.PlayerState.Capturing)
            {
                StartVacuuming();
            }

            // Attackボタンを押しているか、ひきよせ状態の間ずっと、すいこみ判定
            else if (_attackActionInput.action.IsPressed()
                || _playerStateManager.CurrentState == PlayerStateManager.PlayerState.Capturing)
            {
                ProcessVacuum();
            }

            // Attackボタンを離した瞬間に、ひきよせ状態でなければ、通常状態に遷移
            else if (_attackActionInput.action.WasReleasedThisFrame()
                && _playerStateManager.CurrentState != PlayerStateManager.PlayerState.Capturing)
            {
                FinishVacuuming();
            }
        }

        /// <summary>
        /// すいこみ開始処理を行うメソッド
        /// </summary>
        private void StartVacuuming()
        {
            OnVacuumStarted?.Invoke();
            _playerVisualController.ChangeToCapturing();
            _vacuumEffectController.PlayWind();
            _playerStateManager.ChangeState(PlayerStateManager.PlayerState.Vacuuming);
        }

        /// <summary>
        /// すいこみ中の処理を行うメソッド
        /// </summary>
        private void ProcessVacuum()
        {
            bool isCaught = _vacuumAction.IsCaptureTargetDetected();

            // すいこみが成功 かつ まだひきよせ状態でないなら、状態遷移
            if (isCaught && _playerStateManager.CurrentState != PlayerStateManager.PlayerState.Capturing)
            {
                _playerStateManager.ChangeState(PlayerStateManager.PlayerState.Capturing);
            }
        }

        /// <summary>
        /// すいこみ終了処理を行うメソッド
        /// </summary>
        private void FinishVacuuming()
        {
            OnVacuumFinished?.Invoke();
            _playerVisualController.ChangeToNormal();
            _vacuumEffectController.StopWind();
            _playerStateManager.ChangeState(PlayerStateManager.PlayerState.Normal);
        }

        /// <summary>
        /// はきだし準備完了メソッド
        /// </summary>
        private void ReadyToShoot()
        {
            OnVacuumFinished?.Invoke();
            OnCaptureFinished?.Invoke();
            _playerVisualController.ChangeToMouthful();
            _vacuumEffectController.StopWind();
            _playerStateManager.ChangeState(PlayerStateManager.PlayerState.Mouthful);
        }

        /// <summary>
        /// 入力に応じた、はきだし処理を行うメソッド
        /// </summary>
        private void UpdateShootStatus()
        {
            // Attackボタンを押した瞬間に、はきだし処理を開始
            if (_attackActionInput.action.WasPressedThisFrame())
            {
                _shootAction.Shoot(_mouthfulStock.CapturedCount);
                FinishMouthful();
                LockInput();
            }
        }

        /// <summary>
        /// ほおばり終了処理を行うメソッド
        /// </summary>
        private void FinishMouthful()
        {
            _mouthfulStock.ResetCapturedCount();
            _playerVisualController.ChangeToNormal();
            _playerStateManager.ChangeState(PlayerStateManager.PlayerState.Normal);
        }

        /// <summary>
        /// 入力ロックを有効にするメソッド
        /// </summary>
        /// <remarks>用途例：はきだし処理とすいこみ開始が同時に起きないようにする</remarks>
        private void LockInput()
        {
            _needToRelease = true;
        }

        /// <summary>
        /// ボタンが離されたら入力ロックを解除するメソッド
        /// </summary>
        private void CheckUnlockInput()
        {
            if (_attackActionInput.action.WasReleasedThisFrame())
            {
                _needToRelease = false;
            }
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (_captureTargetManager != null)
            {
                _captureTargetManager.OnCaptureFinished -= _mouthfulStock.AddCapturedCount;
                _captureTargetManager.OnAllCapturesFinished -= ReadyToShoot;
            }
        }
    }
}