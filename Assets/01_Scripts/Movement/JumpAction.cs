using UnityEngine;

namespace Manmaru.Movement
{
    public class JumpAction : MonoBehaviour
    {
        [Header("デバッグ用 - 機能オンオフ")]
        [SerializeField] private bool _canSmallJump = true;

        [Header("ジャンプ設定")]
        [Tooltip("ジャンプの初速度")]
        [SerializeField] private float _jumpForce = 2.5f;
        [Tooltip("上昇中に離したときの減速率（小ジャンプ処理用）")]
        [SerializeField] private float _jumpCutoffMultiplier = 0.4f;

        // ジャンプフラグ
        public bool IsJumping { get; private set; }

        /// <summary>
        /// ジャンプに関する状態を更新して、y速度を返すメソッド
        /// </summary>
        public float UpdateJumpState(float curVelY, bool isGrounded, bool jumpPressed, bool jumpReleased)
        {
            // 落下し始めたら、ジャンプフラグオフ
            if (IsJumping && curVelY <= 0f)
            {
                IsJumping = false;
            }

            // ジャンプ入力に応じて、y速度を計算
            if (jumpPressed && isGrounded)
            {
                // 押したらグンと加速
                IsJumping = true;
                return _jumpForce;
            }
            else if (jumpReleased && curVelY > 0f && _canSmallJump)
            {
                // 上昇中に離したらキュッと減速（小ジャンプ）
                return ApplyJumpCutoff(curVelY);
            }

            // 入力がなければそのまま
            return curVelY;
        }

        /// <summary>
        /// ジャンプ中に入力を止めたときの減速率をかけて返すメソッド
        /// </summary>
        /// <param name="curVelY">減速する対象速度</param>
        public float ApplyJumpCutoff(float curVelY)
        {
            Debug.Log("ジャンプ中断（小ジャンプ）");
            return curVelY * _jumpCutoffMultiplier;
        }
    }
}