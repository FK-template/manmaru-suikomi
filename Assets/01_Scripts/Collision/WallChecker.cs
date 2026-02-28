using UnityEngine;
using System.Collections.Generic;

namespace Manmaru.Collision
{
    public class WallChecker : MonoBehaviour
    {
        [Header("Raycast着地判定の設定")]
        [Tooltip("Rayの始点リスト")]
        [SerializeField] private List<Transform> _frontPosList = new List<Transform>();
        [Tooltip("基本のRayの長さ")]
        [SerializeField] private float _rayLength = 0.2f;

        /// <summary>
        /// 壁判定を取り、壁滑りを考慮した速度を返すメソッド
        /// </summary>
        public Vector3 CalculateWallSliding(Vector3 curVel, LayerMask wallLayer)
        {
            // 水平方向の速度ベクトルと移動方向を抽出
            Vector3 horVel = new Vector3(curVel.x, 0f, curVel.z);

            // 水平移動していないのなら終了
            if (horVel.sqrMagnitude < 0.00001f) return curVel;

            // 移動速度に応じたRayの動的長さ調節
            float moveDist = horVel.magnitude * Time.deltaTime;
            float finalRayLength = _rayLength + moveDist;

            // --- 以下、壁滑りを考慮した最終速度を計算---

            Vector3 finalVel = curVel;
            Vector3 moveDir = horVel.normalized;

            foreach (Transform t in _frontPosList)
            {
                // 移動方向にRayを発射
                Ray ray = new Ray(t.position, moveDir);

                if (Physics.Raycast(ray, out RaycastHit hit, finalRayLength, wallLayer))
                {
                    // 「最終速度」と「壁の法線」の内積がマイナスなら、壁に向かう速度を除去
                    float hitNormalVel = Vector3.Dot(finalVel, hit.normal);
                    if (hitNormalVel < 0f) finalVel -= hitNormalVel * hit.normal;

                    Debug.DrawRay(t.position, moveDir * finalRayLength, Color.green);
                }
                else
                {
                    Debug.DrawRay(t.position, moveDir * finalRayLength, Color.red);
                }
            }
            return finalVel;
        }
    }
}