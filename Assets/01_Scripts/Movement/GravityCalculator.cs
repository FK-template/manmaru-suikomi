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

        [Header("落下設定")]
        [Tooltip("重力")]
        [SerializeField] private float _gravity = 1.0f;
        [Tooltip("最大落下速度")]
        [SerializeField] private float _maxFallSpeed = -2.5f;

        [Header("ふわっと滞空の設定")]
        [Tooltip("ふわっと滞空を始める/終わるときの、現在速度の絶対値")]
        [SerializeField] private float _brakeThreshold = 1.0f;
        [Tooltip("ふわっと滞空時の重力減衰率")]
        [SerializeField] private float _brakeGravityMultiplier = 0.5f;

        /// <summary>
        /// 重力を計算して速度を返すメソッド
        /// </summary>
        public float CalculateGravity(float curVelY, bool isGrounded)
        {
            // 通常減速（落下）処理
            if (!isGrounded)
            {
                float curGravity = _gravity;

                // ふんわり滞空のために重力減衰
                if (Mathf.Abs(curVelY) < _brakeThreshold && _canFunwari)
                {
                    curGravity *= _brakeGravityMultiplier;
                }

                // 実際の落下処理
                float nextVelY = curVelY - curGravity * Time.deltaTime;

                // 落下が速くなり過ぎないように補正
                if (nextVelY < _maxFallSpeed && _canMaxFallSpeed)
                {
                    return _maxFallSpeed;
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