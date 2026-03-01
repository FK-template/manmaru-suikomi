using UnityEngine;

namespace Manmaru.Movement
{
    public class PlayerRotation : MonoBehaviour
    {
        [Header("回転設定")]
        [SerializeField] private float _rotationSpeed = 720.0f;

        /// <summary>
        /// 入力と地面の傾きに基づいて次のフレームの向きを計算し、Quaternionで返すメソッド
        /// </summary>
        public Quaternion CalculateRotation(Vector3 inputDirection, Quaternion currentRotation, Vector3 groundNormal)
        {
            // 入力が無いなら、現在の「前」の向きを維持
            Vector3 forwardDir = inputDirection.sqrMagnitude > 0.01f ? inputDirection : currentRotation * Vector3.forward;

            // 前向きベクトルを地面に投影し、地面の傾きを反映
            forwardDir = Vector3.ProjectOnPlane(forwardDir, groundNormal).normalized;

            // 目標の向き＝「前方:投影したベクトル」＋「上方:地面の法線」
            Quaternion targetRotation = Quaternion.LookRotation(forwardDir, groundNormal);

            // 滑らかに回転したあとの角度を返す
            return Quaternion.RotateTowards(currentRotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 入力に基づいて次のフレームの向きを計算し、Quaternionで返すメソッド
        /// </summary>
        public Quaternion CalculateForwardRotation(Vector3 inputDirection, Quaternion currentRotation)
        {
            // 入力が無いなら終了
            if (inputDirection.sqrMagnitude < 0.01f) return currentRotation;

            // 目標の向き＝入力の向き
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);

            // 滑らかに回転したあとの角度を返す
            return Quaternion.RotateTowards(currentRotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}