using Manmaru.Collision;
using Manmaru.Movement;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manmaru.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("デバッグ用 - 機能オンオフ")]
        [SerializeField] private bool _canSmallJump = true;

        [Header("地形判定を取るレイヤー")]
        [SerializeField] private LayerMask _groundLayer;

        [Header("パラメータ設定")]
        [SerializeField] private float _bodyRadius = 0.5f;

        [Header("入力設定")]
        [SerializeField] private InputActionReference _moveAction;

        [Header("依存クラス設定")]
        [SerializeField] private MultiRayGroundChecker _groundChecker;
        [SerializeField] private GroundFitter _groundFitter;
        [SerializeField] private GravityController _gravityController;
        [SerializeField] private JumpAction _jumpAction;
        [SerializeField] private HorizontalMovement _horizontalMovement;
        [SerializeField] private PlayerRotation _playerRotation;
        [SerializeField] private WallChecker _wallChecker;
        [SerializeField] private WallFitter _wallFitter;

        // 内部変数
        private Vector3 _currentVelocity;

        private void Update()
        {
            // 着地判定の保存
            bool isGrounded = _groundChecker.MultiRayCheckGrounded(_currentVelocity.y, out float groundY, out Vector3 groundNormal, _bodyRadius, _groundLayer);

            // 入力状況の保存
            Vector3 inputDirection = GetInputDirection(_moveAction);

            // 速度・回転計算
            UpdateHorizontalVelocity(inputDirection, groundNormal, isGrounded);
            UpdateVerticalVelocity(isGrounded);
            UpdateRotation(inputDirection, groundNormal);

            // 移動・補正処理
            ApplyWallSliding();
            MoveToFinalPos();
            ApplyGroundFitting(groundY, isGrounded);
        }

        // ---------------------------------------------------------------------------
        // 以下、Update処理の分割メソッド群 
        // ---------------------------------------------------------------------------

        /// <summary>
        /// 入力に応じて方向ベクトルをVector3で返すメソッド
        /// </summary>
        private Vector3 GetInputDirection(InputActionReference argInput)
        {
            Vector2 inputVec = argInput.action.ReadValue<Vector2>();
            return new Vector3(inputVec.x, 0f, inputVec.y).normalized;
        }

        /// <summary>
        /// 垂直方向の速度に関する処理をまとめたメソッド
        /// </summary>
        private void UpdateVerticalVelocity(bool isGrounded)
        {
            // 重力処理
            _currentVelocity.y = _gravityController.CalculateGravity(_currentVelocity.y, isGrounded);

            // ジャンプ入力判定
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                // 押したらグンと加速
                _currentVelocity.y = _jumpAction.GetJumpVelocity();
            }
            else if (Input.GetButtonUp("Jump") && _currentVelocity.y > 0f && _canSmallJump)
            {
                // 上昇中に離したらキュッと減速（小ジャンプ）
                _currentVelocity.y = _jumpAction.ApplyJumpCutoff(_currentVelocity.y);
            }
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
            // 着地時のめり込み補正
            if (isGrounded && _currentVelocity.y <= 0f)
            {
                transform.position = _groundFitter.FitToGround(transform.position, groundY, _groundChecker.FeetPosY);
            }
        }
    }
}