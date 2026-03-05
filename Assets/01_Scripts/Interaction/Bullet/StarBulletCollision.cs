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
        /// <param name="moveSpeed">移動速度/f</param>
        /// <param name="sphereRad">球状Rayの半径</param>
        /// <param name="targetLayer">判定を取るレイヤー</param>
        public bool CheckHitBySphereRay(Vector3 moveDir, float moveDist, float moveSpeed, float sphereRad, LayerMask targetLayer, out RaycastHit hitInfo)
        {
            // 球状Rayで、次フレームまでの移動の軌跡を衝突判定
            // - 引数：（発射位置, 球の半径, 発射方向, 衝突相手, 発射距離, 対象レイヤー）
            bool isHit = Physics.SphereCast(
                transform.position,
                sphereRad,
                moveDir,
                out hitInfo,
                moveDist,
                targetLayer
            );

            return isHit;
        }
    }
}