using UnityEngine;

namespace Manmaru.Collision
{
    public class MultiRayGroundChecker : MonoBehaviour
    {
        [Header("Raycast着地判定の設定")]
        [Tooltip("Rayの始点")]
        [SerializeField] private Transform _feetPos;
        [Tooltip("始点を上げるためのオフセット値")]
        [SerializeField] private float _offsetY = 0.1f;
        [Tooltip("基本のRayの長さ")]
        [SerializeField] private float _rayLength = 0.2f;

        // PlayerMovementから参照するためのプロパティ
        public float FeetPosY => _feetPos.position.y;

        /// <summary>
        /// 着地判定の結果をboolで返し、最も高い地面のy座標と法線を返すメソッド
        /// </summary>
        public bool MultiRayCheckGrounded(float currentVelocityY, out float groundPosY, out Vector3 groundNormal, float radius, LayerMask groundLayer)
        {
            groundPosY = float.MinValue;
            groundNormal = Vector3.up;

            // 落下速度に応じたRayの動的長さ調節
            float finalRayLength = _rayLength;
            if (currentVelocityY < 0f)
            {
                float fallDist = Mathf.Abs(currentVelocityY * Time.deltaTime);
                finalRayLength += fallDist;
            }

            // 始点を足元より少し上に（めり込み補正後、地面の内部からRayを発射しないように）
            Vector3 centerStartPos = _feetPos.position + Vector3.up * _offsetY;

            // 円周上の8点＋中心の合計9点のRay発射地点
            Vector3[] offsets =
            {
                Vector3.zero,
                Vector3.forward * radius, Vector3.back * radius,
                Vector3.left * radius, Vector3.right * radius,
                (Vector3.forward + Vector3.left).normalized * radius,
                (Vector3.forward + Vector3.right).normalized * radius,
                (Vector3.back + Vector3.left).normalized * radius,
                (Vector3.back + Vector3.right).normalized * radius
            };

            // --- 以下、最も高い地面のy座標と法線を計算---

            bool isGrounded = false;

            // 地面との衝突判定
            foreach (Vector3 offset in offsets)
            {
                // 地面方向にRayを発射
                Ray ray = new Ray(centerStartPos + offset, Vector3.down);

                if (Physics.Raycast(ray, out RaycastHit hit, finalRayLength, groundLayer))
                {
                    // 地面なのか壁（勾配が急）なのか判定
                    float angle = Vector3.Angle(Vector3.up, hit.normal);
                    if (angle > 45f) continue;

                    isGrounded = true;

                    // 最も高い地点なら、その地点と法線を更新
                    if (hit.point.y > groundPosY)
                    {
                        groundPosY = hit.point.y;
                        groundNormal = hit.normal;
                    }

                    Debug.DrawRay(ray.origin, ray.direction * finalRayLength, Color.green);
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction * finalRayLength, Color.red);
                }
            }

            if (isGrounded) return true;
            else return false;
        }
    }
}