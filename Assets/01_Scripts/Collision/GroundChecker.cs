using UnityEngine;

namespace Manmaru.Collision
{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Raycast着地判定の設定")]
        [Tooltip("Rayの始点")]
        [SerializeField] private Transform _feetPos;
        [Tooltip("始点を上げるためのオフセット値")]
        [SerializeField] private float _offsetY = 0.1f;
        [Tooltip("Rayの長さ")]
        [SerializeField] private float _rayLength = 2.0f;
        [Tooltip("着地判定を取るレイヤー")]
        [SerializeField] private LayerMask _groundLayer;

        // PlayerMovementから参照するためのプロパティ
        public float FeetPosY => _feetPos.position.y;

        /// <summary>
        /// 着地判定の結果をboolで返し、接地している場合は地面のy座標を返すメソッド
        /// </summary>
        public bool CheckGrounded(float currentVelocityY, out float groundPosY)
        {
            // 始点を足元より少し上に（めり込み補正後、地面の内部からRayを発射しないように）
            Vector3 rayStartPos = _feetPos.position + Vector3.up * _offsetY;
            Ray ray = new Ray(rayStartPos, Vector3.down);

            // 落下速度に応じたRayの動的長さ調節
            float finalRayLength = _rayLength;
            if (currentVelocityY < 0f)
            {
                float fallDist = Mathf.Abs(currentVelocityY * Time.deltaTime);
                finalRayLength += fallDist;
            }

            // 衝突判定と地面のy座標取得
            if (Physics.Raycast(ray, out RaycastHit hit, finalRayLength, _groundLayer))
            {
                groundPosY = hit.point.y;
                Debug.DrawRay(rayStartPos, ray.direction * finalRayLength, Color.green);
                return true;
            }

            // 接地していなければ、仮のy座標を返す＆Ray可視化
            groundPosY = 0f;
            Debug.DrawRay(rayStartPos, ray.direction * finalRayLength, Color.red);
            return false;
        }
    }
}