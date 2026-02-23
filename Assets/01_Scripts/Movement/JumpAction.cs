using UnityEngine;

namespace Manmaru.Movement
{
    public class JumpAction : MonoBehaviour
    {
        [Header("ジャンプ設定")]
        [Tooltip("ジャンプの初速度")]
        [SerializeField] private float _jumpForce = 2.5f;
        [Tooltip("上昇中に離したときの減速率（小ジャンプ処理用）")]
        [SerializeField] private float _jumpCutoffMultiplier = 0.4f;

        /// <summary>
        /// ジャンプ初速度を返すメソッド
        /// </summary>
        public float GetJumpVelocity()
        {
            Debug.Log("ジャンプ開始！");
            return _jumpForce;
        }

        /// <summary>
        /// ジャンプ中に入力を止めたときの減速率をかけて返すメソッド
        /// </summary>
        /// <param name="curVelocityY">減速する対象速度</param>
        public float ApplyJumpCutoff(float curVelocityY)
        {
            Debug.Log("ジャンプ中断（小ジャンプ）");
            return curVelocityY * _jumpCutoffMultiplier;
        }
    }
}