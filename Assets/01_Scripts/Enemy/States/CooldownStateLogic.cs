using System;
using UnityEngine;

namespace Manmaru.Enemy.States
{
    /// <summary>
    /// いきぎれ状態の挙動処理クラス
    /// </summary>
    public class CooldownStateLogic : IEnemyStateLogic
    {
        // 内部変数：状態管理クラス
        private EnemyBehaviourController _brain;

        // 内部変数：状態遷移タイマー
        private float _readyTime;

        // 公開変数：状態変更用イベント
        public Action OnCooldownFinished;

        // コンストラクタ
        public CooldownStateLogic(EnemyBehaviourController behaviourController)
        {
            _brain = behaviourController;
        }

        public void Enter() 
        {
            _readyTime = _brain.Data.KyoroKyoroWaitSecond;
        }

        public Vector3 UpdateState() 
        {
            UpdateCooldownTimer();
            return Vector3.zero;
        }

        // ----- 以下、Update処理の分割メソッド群 -----

        /// <summary>
        /// 状態遷移タイマーを更新し、一定時間が経過したら状態変更用イベントを発火するメソッド
        /// </summary>
        private void UpdateCooldownTimer()
        {
            _readyTime -= Time.deltaTime;
            if (_readyTime <= 0)
            {
                OnCooldownFinished?.Invoke();
            }
        }

        // -----

        public void Exit() { }
    }
}