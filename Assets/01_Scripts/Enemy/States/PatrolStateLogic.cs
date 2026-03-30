using System;
using UnityEngine;

namespace Manmaru.Enemy.States
{
    public class PatrolStateLogic : IEnemyStateLogic
    {
        // 内部変数：状態管理クラス
        private EnemyBehaviourController _brain;

        // 内部変数：うろうろ移動処理用
        private Vector3 _destinationPos;
        private float _waitTime;
        private bool _isWaiting;

        // 公開変数：状態変更用イベント
        public Action OnPlayerFound;

        // コンストラクタ
        public PatrolStateLogic(EnemyBehaviourController behaviourController)
        {
            _brain = behaviourController;
        }

        public void Enter()
        {
            SetRandomDestination();
        }

        public Vector3 UpdateState()
        {
            // 索敵処理
            if (_brain.VisionSensor.IsTargetInSight)
            {
                OnPlayerFound?.Invoke();
                return Vector3.zero;
            }

            // 待機処理
            if (_isWaiting)
            {
                return UpdateWaiting();
            }

            // 移動中処理
            return UpdateMovement();
        }

        // ----- 以下、Update処理の分割メソッド群 -----

        /// <summary>
        /// 待機タイマーを更新し、一定時間が経過したら目的地を再設定するメソッド
        /// </summary>
        private Vector3 UpdateWaiting()
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime <= 0)
            {
                _isWaiting = false;
                SetRandomDestination();
            }
            return Vector3.zero;
        }

        /// <summary>
        /// データSOで指定されている範囲内で目的地をランダムに再設定するメソッド
        /// </summary>
        private void SetRandomDestination()
        {
            Vector2 randPosInCircle = UnityEngine.Random.insideUnitCircle;
            Vector3 offsetInCircle = new Vector3(randPosInCircle.x, 0, randPosInCircle.y);
            Vector3 offset = offsetInCircle * (_brain.Data.PatrolRadius - _brain.Data.ReachThreshold);
            _destinationPos = _brain.SpawnPosition + offset;
        }

        /// <summary>
        /// 目的地の到達判定を取り、到達していなければ理想の移動速度を返すメソッド
        /// </summary>
        private Vector3 UpdateMovement()
        {
            // 自分と目的地の平面距離を計算
            Vector3 dirToDestination = _destinationPos - _brain.transform.position;
            dirToDestination.y = 0;

            // 距離が閾値以下なら、待機を開始してゼロリターン
            float distSqr = dirToDestination.sqrMagnitude;
            float thresholdSqr = _brain.Data.ReachThreshold * _brain.Data.ReachThreshold;
            if (distSqr <= thresholdSqr)
            {
                _isWaiting = true;
                _waitTime = _brain.Data.ActionWaitSecond;
                return Vector3.zero;
            }

            // 期待される速度を返す
            return CalculateVelocityTowards(dirToDestination);
        }

        /// <summary>
        /// 任意の方向に移動速度をかけ合わせたベクトルを返すメソッド
        /// </summary>
        private Vector3 CalculateVelocityTowards(Vector3 dir)
        {
            return dir.normalized * _brain.Data.PatrolSpeed;
        }

        // -----

        public void Exit() { }
    }
}