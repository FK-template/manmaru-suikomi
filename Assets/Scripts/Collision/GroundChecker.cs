using UnityEngine;

namespace Manmaru.Collision
{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Raycast着地判定の設定")]
        [Tooltip("Rayの始点")]
        [SerializeField] private Transform _feetPos;
        [Tooltip("Rayの長さ")]
        [SerializeField] private float _rayLength = 2.0f;
        [Tooltip("着地判定を取るレイヤー")]
        [SerializeField] private LayerMask _groundLayer;

        /// <summary>
        /// Raycastを用いた着地判定の結果を、boolで返すメソッド
        /// </summary>
        public bool CheckGrounded()
        {
            // 判定
            Ray ray = new Ray(_feetPos.position, Vector3.down);
            bool isGrounded = Physics.Raycast(ray, _rayLength, _groundLayer);

            // デバッグ用に可視化
            Debug.DrawRay(_feetPos.position, ray.direction * _rayLength, isGrounded ? Color.green : Color.red);

            return isGrounded;
        }
    }
}