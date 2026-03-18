using Manmaru.Player;
using UnityEngine;

namespace Manmaru.Movement
{
    /// <summary>
    /// ジャンプ処理全体を制御するクラス
    /// </summary>
    public class JumpAction : MonoBehaviour
    {
        [Header("デバッグ用 - 機能オンオフ")]
        [SerializeField] private bool _canSmallJump = true;

        // ジャンプフラグ
        public bool IsJumping { get; private set; }

        // 内部変数：パラメータ
        private PlayerMoveParameters _currentParams;

        /// <summary>
        /// 新しくパラメータを設定するメソッド
        /// </summary>
        public void SetParams(PlayerMoveParameters newParams)
        {
            _currentParams = newParams;
        }

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
                return _currentParams.JumpForce;
            }
            else if (jumpReleased && curVelY > 0f && _canSmallJump)
            {
                // 上昇中に離したらキュッと減速（小ジャンプ）
                return ApplyJumpCutoff(curVelY, _currentParams.JumpCutoffMultiplier);
            }

            // 入力がなければそのまま
            return curVelY;
        }

        /// <summary>
        /// ジャンプ中に入力を止めたときの減速率をかけて返すメソッド
        /// </summary>
        private float ApplyJumpCutoff(float curVelY, float jumpCutoffMultiplier)
        {
            Debug.Log("ジャンプ中断（小ジャンプ）");
            return curVelY * jumpCutoffMultiplier;
        }
    }
}