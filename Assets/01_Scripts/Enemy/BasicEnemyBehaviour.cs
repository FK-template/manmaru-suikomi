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
        private NoticeStateLogic _notice;
        private DashStateLogic _dash;
        private CooldownStateLogic _cooldown;

        protected override void Start()
        {
            base.Start();

            // 状態クラス生成
            _patrol = new PatrolStateLogic(this);
            _notice = new NoticeStateLogic(this);
            _dash = new DashStateLogic(this);
            _cooldown = new CooldownStateLogic(this);

            // 状態遷移設定
            _patrol.OnPlayerFound += () => ChangeState(_notice);
            _notice.OnChargeReady += () => ChangeState(_dash);
            _dash.OnDashFinished += () => ChangeState(_cooldown);
            _cooldown.OnCooldownFinished += () => ChangeState(_patrol);

            ChangeState(_patrol);
        }
    }
}