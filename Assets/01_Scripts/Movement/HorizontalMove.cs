using Manmaru.Player;
using UnityEngine;

namespace Manmaru.Movement
{
    /// <summary>
    /// プレイヤーの水平移動を制御するクラス
    /// </summary>
    public class HorizontalMovement : MonoBehaviour
    {
        /// <summary>
        /// 入力に基づいて次のフレームの水平速度を計算し、Vector3で返すメソッド
        /// </summary>
        public Vector3 CalculateHorVelocity(Vector3 inputDir, Vector3 groundNormal, Vector3 curVel, bool isGrounded, PlayerMoveParameters parameters)
        {
            if (isGrounded)
                return CalculateHorVelOnGround(inputDir, groundNormal, curVel, parameters);
            else 
                return CalculateHorVelInTheSky(inputDir,groundNormal, curVel, parameters);
        }

        /// <summary>
        /// 地面の傾きを考慮した水平速度を計算し、Vector3で返すメソッド
        /// </summary>
        private Vector3 CalculateHorVelOnGround(Vector3 inputDir, Vector3 groundNormal, Vector3 curVel, PlayerMoveParameters parameters)
        {
            // 入力ベクトルを地面に投影し、地面の傾きを反映
            float normalVelToPlane = Vector3.Dot(inputDir, groundNormal);
            if (normalVelToPlane < 0f) inputDir -= normalVelToPlane * groundNormal;

            // 滑らか加速の目標水平速度
            Vector3 targetVel = inputDir * parameters.MaxSpeed;

            // 入力の有無に応じて、加速度を決定
            bool isInput = inputDir.sqrMagnitude > 0f;
            float accelRate = isInput ? parameters.GroundAcceleration : parameters.GroundDeceleration;

            // 滑らかに加速/減速した速度を返す
            return Vector3.MoveTowards(curVel, targetVel, accelRate * Time.deltaTime);
        }

        /// <summary>
        /// y成分を切り捨てた水平速度を計算し、Vector3で返すメソッド
        /// </summary>
        private Vector3 CalculateHorVelInTheSky(Vector3 inputDir, Vector3 groundNormal, Vector3 curVel, PlayerMoveParameters parameters)
        {
            // 滑らか加速の目標水平速度
            Vector3 targetVel = inputDir * parameters.MaxSpeed;

            // 現在の水平速度（y成分を切り捨て）
            Vector3 curHorVel = new Vector3(curVel.x, 0f, curVel.z);

            // 入力の有無に応じて、加速度を決定
            bool isInput = inputDir.sqrMagnitude > 0f;
            float accelRate = isInput ? parameters.AirAcceleration : parameters.AirDeceleration;

            // 滑らかに加速/減速した速度を返す
            return Vector3.MoveTowards(curHorVel, targetVel, accelRate * Time.deltaTime);
        }
    }
}