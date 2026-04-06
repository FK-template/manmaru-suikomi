using System;
using UnityEngine;

namespace Manmaru.Enemy.States
{
    /// <summary>
    /// はっけん状態の挙動処理クラス
    /// </summary>
    public class NoticeStateLogic : IEnemyStateLogic
    {
        // 内部変数：状態管理クラス
        private EnemyBehaviourController _brain;

        // 内部変数：状態遷移タイマー
        private float _readyTime;

        // 公開変数：状態変更用イベント
        public Action OnChargeReady;

        // コンストラクタ
        public NoticeStateLogic(EnemyBehaviourController behaviourController)
        {
            _brain = behaviourController;
        }

        public void Enter() 
        {
            _readyTime = _brain.Data.NoticeWaitSecond;
        }

        public Vector3 UpdateState() 
        {
            UpdateWaitTimer();
            _brain.transform.rotation = CalculateRotToPlayer();
            return Vector3.zero; 
        }

        // ----- 以下、Update処理の分割メソッド群 -----

        /// <summary>
        /// 状態遷移タイマーを更新し、一定時間が経過したら状態変更用イベントを発火するメソッド
        /// </summary>
        private void UpdateWaitTimer()
        {
            _readyTime -= Time.deltaTime;
            if (_readyTime <= 0)
            {
                OnChargeReady?.Invoke();
            }
        }

        /// <summary>
        /// プレイヤーの方へ向くためのQuaternionを計算して返すメソッド
        /// </summary>
        public Quaternion CalculateRotToPlayer()
        {
            // y方向の角度は使わず、地面への投影はEnemyMoveControllerに任せる
            Vector3 dirToPlayer = _brain.PlayerTransform.position - _brain.transform.position;
            dirToPlayer.y = 0f;

            // 目標の向きに、高速で向く
            Quaternion targetRot = Quaternion.LookRotation(dirToPlayer);
            return Quaternion.RotateTowards(_brain.transform.rotation, targetRot, _brain.Data.HighRotationSpeed);
        }

        // -----

        public void Exit() { }
    }
}