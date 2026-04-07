using Manmaru.Player;
using System;
using UnityEngine;

namespace Manmaru.Movement
{
    /// <summary>
    /// ジャンプ処理全体を制御するクラス
    /// </summary>
    public class JumpAction : MonoBehaviour
    {
        [Header("ジャンプパラメータ設定")]
        [SerializeField] private float _jumpForceThreshold = 0.01f;

        [Header("デバッグ用 - 機能オンオフ")]
        [SerializeField] private bool _canSmallJump = true;

        // ジャンプフラグ
        public bool IsJumping { get; private set; }

        // 公開変数：サウンド用イベント
        public Action OnJumped;

        // 内部変数：パラメータ
        private PlayerMoveParametersSO _currentParams;

        /// <summary>
        /// 新しくパラメータを設定するメソッド
        /// </summary>
        public void SetParams(PlayerMoveParametersSO newParams)
        {
            _currentParams = newParams;
        }

        /// <summary>
        /// ジャンプに関する状態を更新して、y速度を返すメソッド
        /// </summary>
        public float UpdateJumpState(float curVelY, bool isGrounded, bool jumpPressed, bool jumpReleased)
        {
            float jumpForce = _currentParams.JumpForce;

            // ジャンプ力パラメータが極端に低い時は、処理をスキップ
            if (jumpForce < _jumpForceThreshold) return curVelY;

            // 落下し始めたら、ジャンプフラグオフ
            if (IsJumping && curVelY <= 0f)
            {
                IsJumping = false;
            }

            // ジャンプ入力に応じて、y速度を計算
            if (jumpPressed && isGrounded)
            {
                // サウンド用イベントを発火
                OnJumped?.Invoke();

                // 押したらグンと加速
                IsJumping = true;
                return jumpForce;
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
            return curVelY * jumpCutoffMultiplier;
        }
    }
}