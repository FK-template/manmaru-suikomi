using Manmaru.Collision;
using Manmaru.Movement;
using UnityEngine;

namespace Manmaru.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("デバッグ用 - 機能オンオフ")]
        [SerializeField] private bool _canSmallJump = true;
        [SerializeField] private bool _canFunwari = true;
        [SerializeField] private bool _canMaxFallSpeed = true;

        [Header("依存クラス設定")]
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private JumpAction _jumpAction;
        [SerializeField] private GravityController _gravityController;

        // 内部変数
        private Vector3 _currentVelocity;
        private bool _isGrounded;

        private void Update()
        {
            // 着地判定
            _isGrounded = _groundChecker.CheckGrounded();

            // ジャンプ入力判定
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                // 押したらグンと加速
                _currentVelocity.y = _jumpAction.GetJumpVelocity();
            }
            else if (Input.GetButtonUp("Jump") && _currentVelocity.y > 0f && _canSmallJump)
            {
                // 上昇中に離したらギュッと減速（小ジャンプ）
                _currentVelocity.y = _jumpAction.ApplyJumpCutoff(_currentVelocity.y);
            }

            // 落下処理
            else
                _currentVelocity.y = _gravityController.CalculateGravity(_currentVelocity.y, _isGrounded, _canFunwari, _canMaxFallSpeed);

            // 座標移動
            MoveToFinalPos();
        }

        /// <summary>
        /// 実際の座標移動を行うメソッド
        /// </summary>
        private void MoveToFinalPos()
        {
            transform.position += _currentVelocity * Time.deltaTime;
        }
    }
}