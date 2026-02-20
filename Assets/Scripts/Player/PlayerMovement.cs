using Manmaru.Collision;
using Manmaru.Movement;
using UnityEngine;

namespace Manmaru.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("デバッグ用 - 機能オンオフ")]
        [SerializeField] private bool _canSmallJump = true;

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
            _isGrounded = _groundChecker.CheckGrounded(_currentVelocity.y, out float groundY);

            // 落下処理
            _currentVelocity.y = _gravityController.CalculateGravity(_currentVelocity.y, _isGrounded);

            // ジャンプ入力判定
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                // 押したらグンと加速
                _currentVelocity.y = _jumpAction.GetJumpVelocity();
            }
            else if (Input.GetButtonUp("Jump") && _currentVelocity.y > 0f && _canSmallJump)
            {
                // 上昇中に離したらキュッと減速（小ジャンプ）
                _currentVelocity.y = _jumpAction.ApplyJumpCutoff(_currentVelocity.y);
            }

            // 座標移動
            MoveToFinalPos();

            // 着地めり込み補正
            if (_isGrounded && _currentVelocity.y <= 0f)
            {
                CorrectGroundPos(groundY);
            }
        }

        /// <summary>
        /// 実際の座標移動を行うメソッド
        /// </summary>
        private void MoveToFinalPos()
        {
            transform.position += _currentVelocity * Time.deltaTime;
        }

        /// <summary>
        /// 着地位置を補正してめり込みを防ぐメソッド
        /// </summary>
        private void CorrectGroundPos(float groundY)
        {
            // 地面の表面 + Rayの長さ + (プレイヤーの原点 - プレイヤーの足元)
            float heightOffset = transform.position.y - _groundChecker.FeetPosY;
            float correctY = groundY + heightOffset;

            // 正しい位置でなければ、補正
            if (Mathf.Abs(transform.position.y - correctY) > 0.001f)
            {
                Debug.Log($"めり込み補正！y={correctY}");
                transform.position = new Vector3(transform.position.x, correctY, transform.position.z);
            }
        }
    }
}