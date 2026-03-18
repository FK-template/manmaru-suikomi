using UnityEngine;

namespace Manmaru.Collision
{
    /// <summary>
    /// めり込み対策として、複数Rayによる位置補正を行うクラス
    /// </summary>
    public class WallFitter : MonoBehaviour
    {
        /// <summary>
        /// 水平8方向に補正をかけた位置を返し、めり込みを防ぐメソッド
        /// </summary>
        public Vector3 FixWallPenetration(Vector3 playerPos, float bodyRad, LayerMask wallLayer)
        {
            Vector3 finalPos = playerPos;

            // キャラの中心から水平8方向にRayを発射
            Vector3[] directions =
            {
                Vector3.forward, Vector3.back, Vector3.left, Vector3.right,
                (Vector3.forward + Vector3.right).normalized,
                (Vector3.forward + Vector3.left).normalized,
                (Vector3.back + Vector3.right).normalized,
                (Vector3.back + Vector3.left).normalized
            };

            foreach (Vector3 dir in directions)
            {
                Ray ray = new Ray(playerPos, dir);

                // Rayの長さは「体の半径」
                if (Physics.Raycast(ray, out RaycastHit hit, bodyRad, wallLayer))
                {
                    // めり込み距離ぶんだけ、壁と逆方向に移動
                    float pushDist = bodyRad - hit.distance;
                    finalPos += hit.normal * pushDist;
                }

                Debug.DrawRay(playerPos, dir * bodyRad, Color.yellow);
            }

            return finalPos;
        }
    }
}