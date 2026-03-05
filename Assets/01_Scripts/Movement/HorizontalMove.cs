using UnityEngine;

namespace Manmaru.Movement
{
    /// <summary>
    /// プレイヤーの水平移動を制御するクラス
    /// </summary>
    public class HorizontalMovement : MonoBehaviour
    {
        [Header("移動速度")]
        [SerializeField] private float _maxSpeed = 8.0f;

        [Header("地上での挙動")]
        [SerializeField] private float _groundAcceleration = 50.0f;
        [SerializeField] private float _groundDeceleration = 60.0f;

        [Header("空中での挙動")]
        [SerializeField] private float _airAcceleration = 20.0f;
        [SerializeField] private float _airDeceleration = 5.0f;

        /// <summary>
        /// 入力に基づいて次のフレームの水平速度を計算し、Vector3で返すメソッド
        /// </summary>
        public Vector3 CalculateHorVelocity(Vector3 inputDir, Vector3 groundNormal, Vector3 curVel, bool isGrounded)
        {
            if (isGrounded)
                return CalculateHorVelOnGround(inputDir, groundNormal, curVel);
            else 
                return CalculateHorVelInTheSky(inputDir,groundNormal, curVel);
        }

        /// <summary>
        /// 地面の傾きを考慮した水平速度を計算し、Vector3で返すメソッド
        /// </summary>
        private Vector3 CalculateHorVelOnGround(Vector3 inputDir, Vector3 groundNormal, Vector3 curVel)
        {
            // 入力ベクトルを地面に投影し、地面の傾きを反映
            float normalVelToPlane = Vector3.Dot(inputDir, groundNormal);
            if (normalVelToPlane < 0f) inputDir -= normalVelToPlane * groundNormal;

            // 滑らか加速の目標水平速度
            Vector3 targetVel = inputDir * _maxSpeed;

            // 入力の有無に応じて、加速度を決定
            bool isInput = inputDir.sqrMagnitude > 0f;
            float accelRate = isInput ? _groundAcceleration : _groundDeceleration;

            // 滑らかに加速/減速した速度を返す
            return Vector3.MoveTowards(curVel, targetVel, accelRate * Time.deltaTime);
        }

        /// <summary>
        /// y成分を切り捨てた水平速度を計算し、Vector3で返すメソッド
        /// </summary>
        private Vector3 CalculateHorVelInTheSky(Vector3 inputDir, Vector3 groundNormal, Vector3 curVel)
        {
            // 滑らか加速の目標水平速度
            Vector3 targetVel = inputDir * _maxSpeed;

            // 現在の水平速度（y成分を切り捨て）
            Vector3 curHorVel = new Vector3(curVel.x, 0f, curVel.z);

            // 入力の有無に応じて、加速度を決定
            bool isInput = inputDir.sqrMagnitude > 0f;
            float accelRate = isInput ? _airAcceleration : _airDeceleration;

            // 滑らかに加速/減速した速度を返す
            return Vector3.MoveTowards(curHorVel, targetVel, accelRate * Time.deltaTime);
        }
    }
}