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
        /// 重力を計算して速度を返すメソッド
        /// </summary>
        public float CalculateGravity(float curVelY, bool isGrounded)
        {
            // 通常減速（落下）処理
            if (!isGrounded)
            {
                float curGravity = _currentParams.Gravity;

                // ふんわり滞空のために重力減衰
                if (Mathf.Abs(curVelY) < _currentParams.BrakeThreshold && _canFunwari)
                {
                    curGravity *= _currentParams.BrakeGravityMultiplier;
                }

                // 実際の落下処理
                float nextVelY = curVelY - curGravity * Time.deltaTime;

                // 落下が速くなり過ぎないように補正
                if (nextVelY < _currentParams.MaxFallSpeed && _canMaxFallSpeed)
                {
                    return _currentParams.MaxFallSpeed;
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