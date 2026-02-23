using UnityEngine;

namespace Manmaru.Movement
{
    public class PlayerRotation : MonoBehaviour
    {
        [Header("回転設定")]
        [SerializeField] private float _rotationSpeed = 720.0f;

        /// <summary>
        /// 入力に基づいて次のフレームの向きを計算し、Quaternionで返すメソッド
        /// </summary>
        public Quaternion CalculateRotation(Vector3 inputDirection, Quaternion currentRotation)
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