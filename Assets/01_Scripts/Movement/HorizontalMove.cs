using UnityEngine;

namespace Manmaru.Movement
{
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
        public Vector3 CalculateVelocity(Vector3 inputDirection, Vector3 currentVelocity, bool isGrounded)
        {
            // 滑らか加速の目標水平速度
            Vector3 targetVelocity = inputDirection * _maxSpeed;
            // 現在の水平速度
            Vector3 currentHorizontalVel = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

            // 入力の有無に応じて、加速度を決定
            float accelerationRate;
            if (inputDirection.sqrMagnitude > 0f)
            {
                accelerationRate = isGrounded ? _groundAcceleration : _airAcceleration;
            }
            else
            {
                accelerationRate = isGrounded ? _groundDeceleration : _airDeceleration;
            }

            // 滑らかに加速/減速した速度を返す
            return Vector3.MoveTowards(currentHorizontalVel, targetVelocity, accelerationRate * Time.deltaTime);
        }
    }
}