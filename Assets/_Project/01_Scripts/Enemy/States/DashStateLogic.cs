using System;
using UnityEngine;

namespace Manmaru.Enemy.States
{
    /// <summary>
    /// とっしん状態の挙動処理クラス
    /// </summary>
    public class DashStateLogic : IEnemyStateLogic
    {
        // 内部変数：状態管理クラス
        private EnemyBehaviourController _brain;

        // 内部変数：状態遷移タイマー
        private float _dashTime;

        // 公開変数：状態変更用イベント
        public Action OnDashFinished;

        // コンストラクタ
        public DashStateLogic(EnemyBehaviourController behaviourController)
        {
            _brain = behaviourController;
        }

        public void Enter()
        {
            _dashTime = _brain.Data.DashDurationSecond;
        }

        public Vector3 UpdateState() 
        {
            UpdateDashTimer();
            return _brain.transform.forward * _brain.Data.DashSpeed;
        }

        // ----- 以下、Update処理の分割メソッド群 -----

        /// <summary>
        /// 状態遷移タイマーを更新し、一定時間が経過したら状態変更用イベントを発火するメソッド
        /// </summary>
        private void UpdateDashTimer()
        {
            _dashTime -= Time.deltaTime;
            if (_dashTime <= 0)
            {
                OnDashFinished?.Invoke();
            }
        }

        // -----

        public void Exit() { }
    }
}