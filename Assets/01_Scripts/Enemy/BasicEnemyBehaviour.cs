using Manmaru.Enemy.States;
using UnityEngine;

namespace Manmaru.Enemy
{
    /// <summary>
    /// 標準的な近接型の敵の状態遷移と振る舞いを管理する子クラス
    /// </summary>
    public class BasicEnemyBehaviour : EnemyBehaviourController
    {
        // 内部変数：状態クラス
        private PatrolStateLogic _patrol;

        protected override void Start()
        {
            base.Start();

            // 状態クラス生成
            _patrol = new PatrolStateLogic(this);

            // 状態遷移設定
            _patrol.OnPlayerFound += () => Debug.Log("プレイヤー発見！とっしん状態に移行予定");

            ChangeState(_patrol);
        }
    }
}