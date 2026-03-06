using Manmaru.Player;
using UnityEngine;

namespace Manmaru.Movement
{
    /// <summary>
    /// プレイヤーの重力処理を制御するクラス
    /// </summary>
    public class GravityCalculator : MonoBehaviour
    {
        [Header("デバッグ用 - 機能オンオフ")]
        [SerializeField] private bool _canFunwari = true;
        [SerializeField] private bool _canMaxFallSpeed = true;

        /// <summary>
        /// 重力を計算して速度を返すメソッド
        /// </summary>
        public float CalculateGravity(float curVelY, bool isGrounded, PlayerMoveParameters parameters)
        {
            // 通常減速（落下）処理
            if (!isGrounded)
            {
                float curGravity = parameters.Gravity;

                // ふんわり滞空のために重力減衰
                if (Mathf.Abs(curVelY) < parameters.BrakeThreshold && _canFunwari)
                {
                    curGravity *= parameters.BrakeGravityMultiplier;
                }

                // 実際の落下処理
                float nextVelY = curVelY - curGravity * Time.deltaTime;

                // 落下が速くなり過ぎないように補正
                if (nextVelY < parameters.MaxFallSpeed && _canMaxFallSpeed)
                {
                    return parameters.MaxFallSpeed;
                }

                return nextVelY;
            }
            else
            {
                return 0f;
            }
        }
    }
}