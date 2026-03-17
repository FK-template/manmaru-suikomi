using Manmaru.Collision;
using Manmaru.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーの移動処理全般を制御するクラス
    /// </summary>
    public class PlayerMoveController : MonoBehaviour
    {
        [Header("地形判定を取るレイヤー")]
        [SerializeField] private LayerMask _groundLayer;

        [Header("パラメータ設定")]
        [SerializeField] private float _bodyRadius = 0.5f;
        [SerializeField] private float _kbBackForce = 1.0f;
        [SerializeField] private float _kbUpForce = 3.0f;

        [Header("入力設定")]
        [SerializeField] private InputActionReference _moveActionInput;
        [SerializeField] private InputActionReference _jumpActionInput;

        [Header("依存クラス設定")]
        [SerializeField] private PlayerStateManager _playerStateManager;
        [SerializeField] private MultiRayGroundChecker _groundChecker;
        [SerializeField] private GroundFitter _groundFitter;
        [SerializeField] private GravityCalculator _gravityCalculator;
        [SerializeField] private JumpAction _jumpAction;
        [SerializeField] private HorizontalMovement _horizontalMovement;
        [SerializeField] private PlayerRotation _playerRotation;
        [SerializeField] private WallChecker _wallChecker;
        [SerializeField] private WallFitter _wallFitter;

        // 内部変数
        public Vector3 _currentVelocity;

        void Awake()
        {
            // イベント設定（引数渡し付き）
            _playerStateManager.OnStateChanged += ChangeParams;
        }

        private void Update()
        {
            // ゲームオーバー状態なら、物理挙動を停止して入力も受け付けない
            if (_playerStateManager.CurrentState == PlayerStateManager.PlayerState.Dead) return;

            // 着地判定の保存
            bool isGrounded = _groundChecker.MultiRayCheckGrounded(_currentVelocity.y, out float groundY, out Vector3 groundNormal, _bodyRadius, _groundLayer);

            // ノックバック状態かどうかの保存
            bool isDamaged = _playerStateManager.CurrentState == PlayerStateManager.PlayerState.Damaged;

            // ノックバック判定
            if (isDamaged)
            {
                UpdateKnockbackState();
            }
            else
            {
                UpdateNormalMoveState(isGrounded, groundNormal);
            }

            // 移動・補正処理
            ApplyWallSliding();
            MoveToFinalPos();

            // 移動後の地面情報を再取得して補正
            isGrounded = _groundChecker.MultiRayCheckGrounded(_currentVelocity.y, out groundY, out groundNormal, _bodyRadius, _groundLayer);
            ApplyGroundFitting(groundY, isGrounded);

            // ノックバック中に落下して着地したら、ノックバック状態解除
            if (isDamaged && _currentVelocity.y <= 0f && isGrounded)
                _playerStateManager.ChangeState(PlayerStateManager.PlayerState.Normal);
        }

        /// <summary>
        /// 移動パラメータを変更するメソッド
        /// </summary>
        private void ChangeParams(PlayerStateManager.PlayerState newState, PlayerMoveParameters newParams)
        {
            _gravityCalculator.SetParams(newParams);
            _jumpAction.SetParams(newParams);
            _horizontalMovement.SetParams(newParams);

            if (newState == PlayerStateManager.PlayerState.Damaged)
            {
                ApplyKnockback();
            }
        }

        /// <summary>
        /// 被ダメージ時のノックバック用ベクトルを、現在の速度に適用するメソッド
        /// </summary>
        private void ApplyKnockback()
        {
            _currentVelocity = (-transform.forward * _kbBackForce) + (Vector3.up * _kbUpForce);
        }

        // ---------------------------------------------------------------------------
        // 以下、Update処理の分割メソッド群 
        // ---------------------------------------------------------------------------

        /// <summary>
        /// 入力に応じて移動用の方向ベクトルをVector3で返すメソッド
        /// </summary>
        private Vector3 GetInputDirection(InputActionReference argInput)
        {
            Vector2 inputVec = argInput.action.ReadValue<Vector2>();
            return new Vector3(inputVec.x, 0f, inputVec.y).normalized;
        }

        /// <summary>
        /// ノックバック状態専用の移動・ジャンプ処理を行うメソッド
        /// </summary>
        private void UpdateKnockbackState()
        {
            // 重力処理（落下速度がゼロにならないよう、空中として処理）
            _currentVelocity.y = _gravityCalculator.CalculateGravity(_currentVelocity.y, false);

            // 水平移動処理（無入力）
            UpdateHorizontalVelocity(Vector3.zero, Vector3.up, false);
        }

        /// <summary>
        /// 通常時の移動・ジャンプ処理を行うメソッド
        /// </summary>
        private void UpdateNormalMoveState(bool isGrounded, Vector3 groundNormal)
        {
            // 入力状況の保存
            Vector3 inputDirection = GetInputDirection(_moveActionInput);

            // 速度・回転計算
            UpdateHorizontalVelocity(inputDirection, groundNormal, isGrounded);
            UpdateVerticalVelocity(isGrounded);
            UpdateRotation(inputDirection, groundNormal);
        }

        /// <summary>
        /// 垂直方向の速度に関する処理をまとめたメソッド
        /// </summary>
        private void UpdateVerticalVelocity(bool isGrounded)
        {
            // 重力処理
            _currentVelocity.y = _gravityCalculator.CalculateGravity(_currentVelocity.y, isGrounded);

            // ジャンプ入力情報
            bool isJumpPressed = _jumpActionInput.action.WasPressedThisFrame();
            bool isJumpReleased = _jumpActionInput.action.WasReleasedThisFrame();

            // ジャンプ状態の更新
            _currentVelocity.y = _jumpAction.UpdateJumpState(_currentVelocity.y, isGrounded, isJumpPressed, isJumpReleased);
        }

        /// <summary>
        /// 水平方向の速度に関する処理をまとめたメソッド
        /// </summary>
        private void UpdateHorizontalVelocity(Vector3 inputDir, Vector3 groundNormal, bool isGrounded)
        {
            Vector3 nextHorVel = _horizontalMovement.CalculateHorVelocity(inputDir, groundNormal, _currentVelocity, isGrounded);
            _currentVelocity.x = nextHorVel.x;
            _currentVelocity.z = nextHorVel.z;
        }

        /// <summary>
        /// 移動に伴う回転処理をまとめたメソッド
        /// </summary>
        private void UpdateRotation(Vector3 inputDir, Vector3 groundNormal)
        {
            transform.rotation = _playerRotation.CalculateRotation(inputDir, transform.rotation, groundNormal);
        }

        /// <summary>
        /// 現在の速度に壁滑りを適用するメソッド
        /// </summary>
        private void ApplyWallSliding()
        {
            _currentVelocity = _wallChecker.CalculateWallSliding(_currentVelocity, _groundLayer);
            transform.position = _wallFitter.FixWallPenetration(transform.position, _bodyRadius, _groundLayer);
        }

        /// <summary>
        /// 実際の座標移動を行うメソッド
        /// </summary>
        private void MoveToFinalPos()
        {
            transform.position += _currentVelocity * Time.deltaTime;
        }

        /// <summary>
        /// めり込み補正を適用するメソッド
        /// </summary>
        private void ApplyGroundFitting(float groundY, bool isGrounded)
        {
            // ジャンプ中 or 空中 or 上昇中 なら終了
            if (_jumpAction.IsJumping || !isGrounded || _currentVelocity.y > 0f) return;

            // 着地時のめり込み補正
            transform.position = _groundFitter.FitToGround(transform.position, groundY, _groundChecker.FeetPosY);
        }
    }
}