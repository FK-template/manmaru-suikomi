using Unity.VisualScripting;
using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// はきだし弾の衝突判定を行うクラス
    /// </summary>
    public class StarBulletCollision : MonoBehaviour
    {
        /// <summary>
        /// 進行方向に球状のRayを飛ばして予め衝突判定を取り、結果をboolで返すメソッド
        /// </summary>
        /// <param name="moveDir">移動方向</param>
        /// <param name="moveDist">移動距離</param>
        /// <param name="moveSpeed">移動速度/f</param>
        /// <param name="sphereRad">球状Rayの半径</param>
        /// <param name="targetLayer">判定を取るレイヤー</param>
        /// <param name="hit">衝突相手の情報</param>
        public bool CheckHitsBySphereRay(Vector3 moveDir, float moveDist, float moveSpeed, float sphereRad, LayerMask targetLayer, out RaycastHit hit)
        {
            // 球状Rayで、次フレームまでの移動の軌跡を衝突判定
            // - 引数：（発射位置, 球の半径, 発射方向, 衝突相手, 発射距離, 対象レイヤー）
            bool isHit = Physics.SphereCast(
                transform.position,
                sphereRad,
                moveDir,
                out hit,
                moveDist,
                targetLayer
            );

            return isHit;
        }

        /// <summary>
        /// 進行方向に球状のRayを飛ばして予め衝突判定を取り、結果をboolで返すメソッド
        /// </summary>
        /// <param name="moveDir">移動方向</param>
        /// <param name="moveDist">移動距離</param>
        /// <param name="moveSpeed">移動速度/f</param>
        /// <param name="sphereRad">球状Rayの半径</param>
        /// <param name="targetLayer">判定を取るレイヤー</param>
        /// <param name="hits">全衝突相手の情報</param>
        public bool CheckHitsBySphereRay(Vector3 moveDir, float moveDist, float moveSpeed, float sphereRad, LayerMask targetLayer, out RaycastHit[] hits)
        {
            // 球状Rayで、次フレームまでの移動の軌跡を衝突判定
            // - 引数：（発射位置, 球の半径, 発射方向, 発射距離, 対象レイヤー）
            hits = Physics.SphereCastAll(
                transform.position,
                sphereRad,
                moveDir,
                moveDist,
                targetLayer
            );

            return hits.Length > 0;
        }
    }
}