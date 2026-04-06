using Manmaru.Ability;
using Manmaru.Interaction;
using Manmaru.VFX;
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

        [Header("はきだし設定")]
        [SerializeField] private int _captureCountLimit = 5;
        [SerializeField] private StarBulletController _starBullet;
        [SerializeField] private Transform _spawnTrans;

        [Header("依存クラス設定")]
        [SerializeField] private PlayerStateManager _playerStateManager;
        [SerializeField] private VacuumAction _vacuumAction;
        [SerializeField] private PlayerVisualHandler _playerVisualController;

        // 内部変数：ほおばり・はきだし用
        private bool _needToRelease = false;
        private int _capturedCount = 0;

        // 内部変数：すいこみオブジェクトの管理者（イベント購読用）
        private CaptureTargetManager _captureTargetManager;

        void Start()
        {
            _captureTargetManager = CaptureTargetManager.Instance;

            // イベント購読設定
            _captureTargetManager.OnCaptureFinished += AddCapturedCount;
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

        private void CheckUnlockInput()
        {
            // ボタンリリースされたら入力ロックを解除
            if (_attackActionInput.action.WasReleasedThisFrame())
            {
                _needToRelease = false;
            }
        }

        /// <summary>
        /// 入力に応じてすいこみ処理を行うメソッド
        /// </summary>
        private void UpdateCaptureStatus()
        {
            // Attackボタンを押した瞬間に、すいこみ状態に遷移
            if (_attackActionInput.action.WasPressedThisFrame())
            {
                _vacuumAction.StartVacuuming();
            }

            // Attackボタンを押しているか、ひきよせ状態の間ずっと、すいこみ判定
            if (_attackActionInput.action.IsPressed()
                || _playerStateManager.CurrentState == PlayerStateManager.PlayerState.Capturing)
            {
                _vacuumAction.Vacuuming();
            }

            // Attackボタンを離した瞬間に、ひきよせ状態でなければ、通常状態に遷移
            if (_attackActionInput.action.WasReleasedThisFrame()
                && _playerStateManager.CurrentState != PlayerStateManager.PlayerState.Capturing)
            {
                _vacuumAction.FinishVacuuming();
            }
        }

        /// <summary>
        /// 入力に応じた、はきだし処理を行うメソッド
        /// </summary>
        private void UpdateShootStatus()
        {
            // Attackボタンを押した瞬間に、はきだし処理を開始
            if (_attackActionInput.action.WasPressedThisFrame())
            {
                Debug.Log($"はきだし！弾の強さ：Lv.{_capturedCount}");

                // グラフィック情報を更新
                _playerVisualController.ChangeToNormal();

                // 弾の生成と初期化
                StarBulletController bullet = Instantiate(_starBullet, _spawnTrans.position, Quaternion.LookRotation(transform.forward));
                bullet.Initialize(transform.forward, Mathf.Min(_capturedCount, _captureCountLimit));

                // ほおばり状態の初期化
                _capturedCount = 0;

                // 通常状態に遷移
                _playerStateManager.ChangeState(PlayerStateManager.PlayerState.Normal);

                // 入力ロック（ボタンリリースされるまですいこみ禁止）
                _needToRelease = true;
            }
        }

        /// <summary>
        /// すいこみ済みカウンターを増やすメソッド
        /// </summary>
        private void AddCapturedCount(ICapturable captureTarget)
        {
            _capturedCount += captureTarget.CaptureMass;
            Debug.Log($"すいこみ完了！現在のストック：{_capturedCount}");
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (_captureTargetManager != null)
            {
                _captureTargetManager.OnCaptureFinished -= AddCapturedCount;
            }
        }
    }
}