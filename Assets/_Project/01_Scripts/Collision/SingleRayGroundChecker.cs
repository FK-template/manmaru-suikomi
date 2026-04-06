using UnityEngine;

namespace Manmaru.Collision
{
    /// <summary>
    /// 単一Rayによる着地判定を行うクラス
    /// </summary>
    public class SingleRayGroundChecker : MonoBehaviour
    {
        [Header("Raycast着地判定の設定")]
        [Tooltip("Rayの始点")]
        [SerializeField] private Transform _feetPos;
        [Tooltip("始点を上げるためのオフセット値")]
        [SerializeField] private float _offsetY = 0.1f;
        [Tooltip("基本のRayの長さ")]
        [SerializeField] private float _rayLength = 2.0f;

        // PlayerMovementから参照するためのプロパティ
        public float FeetPosY => _feetPos.position.y;

        /// <summary>
        /// 着地判定の結果をboolで返し、接地している場合は地面のy座標と法線ベクトルを返すメソッド
        /// </summary>
        public bool CheckGrounded(float curVelY, out float groundPosY, out Vector3 groundNormal, LayerMask groundLayer)
        {
            // 始点を足元より少し上に（めり込み補正後、地面の内部からRayを発射しないように）
            Vector3 rayStartPos = _feetPos.position + Vector3.up * _offsetY;
            Ray ray = new Ray(rayStartPos, Vector3.down);

            // 落下速度に応じたRayの動的長さ調節
            float finalRayLength = _rayLength;
            if (curVelY < 0f)
            {
                float fallDist = Mathf.Abs(curVelY * Time.deltaTime);
                finalRayLength += fallDist;
            }

            // 衝突判定と地面のy座標＆法線取得
            if (Physics.Raycast(ray, out RaycastHit hit, finalRayLength, groundLayer))
            {
                groundPosY = hit.point.y;
                groundNormal = hit.normal;
                Debug.DrawRay(rayStartPos, ray.direction * finalRayLength, Color.green);
                return true;
            }

            // 接地していなければ、仮のy座標＆法線を返す
            groundPosY = 0f;
            groundNormal = Vector3.up;
            Debug.DrawRay(rayStartPos, ray.direction * finalRayLength, Color.red);
            return false;
        }
    }
}