using UnityEngine;

namespace Manmaru.Enemy.States
{
    /// <summary>
    /// 敵の行動状態クラスのインターフェース
    /// </summary>
    public interface IEnemyStateLogic
    {
        void Enter();
        Vector3 UpdateState();
        void Exit();
    }
}