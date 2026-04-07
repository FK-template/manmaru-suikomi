using Manmaru.Interaction;
using UnityEngine;

namespace Manmaru.Ability
{
    /// <summary>
    /// はきだし処理を行うクラス
    /// </summary>
    public class ShootAction : MonoBehaviour
    {
        [Header("はきだしパラメータ設定")]
        [SerializeField] private StarBulletController _starBullet;
        [SerializeField] private Transform _spawnTrans;

        /// <summary>
        /// はきだし弾を生成し、弾の初期設定を行うメソッド
        /// </summary>
        public void Shoot(int capturedCount)
        {
            // 弾の生成と初期化
            StarBulletController bullet = Instantiate(_starBullet, _spawnTrans.position, Quaternion.LookRotation(_spawnTrans.forward));
            bullet.Initialize(_spawnTrans.forward, capturedCount);

            Debug.Log($"はきだし！弾の強さ：Lv.{capturedCount}");
        }
    }
}