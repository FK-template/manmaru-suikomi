using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// ダメージを与える相手の干渉判定を行うクラス
    /// </summary>
    public class DamageAreaDetector : MonoBehaviour
    {
        /// <summary>
        /// 任意のレイヤー上で球状の干渉判定を取り、当たった相手を返すメソッド
        /// </summary>
        /// <remarks>（※レイヤーを指定しないと、自分自身を判定して消えてしまう）</remarks>
        public Collider[] GetHittingColliders(Vector3 centerPos, float sphereRad, LayerMask targetLayer)
        {
            Collider[] hitCols = Physics.OverlapSphere(centerPos, sphereRad, targetLayer);
            return hitCols;
        }
    }
}