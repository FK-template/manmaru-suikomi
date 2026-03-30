using UnityEngine;

namespace Manmaru.Enemy.States
{
    /// <summary>
    /// 敵の行動状態クラスのインターフェース
    /// </summary>
    public interface IEnemyStateLogic
    {
        /// <summary>
        /// 本状態に遷移したときに呼ばれるメソッド
        /// </summary>
        void Enter();

        /// <summary>
        /// 毎フレーム呼ばれ、理想の移動速度を返すメソッド
        /// </summary>
        Vector3 UpdateState();

        /// <summary>
        /// 他の状態に遷移するときに呼ばれるメソッド
        /// </summary>
        void Exit();
    }
}