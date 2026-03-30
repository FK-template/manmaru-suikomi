using UnityEngine;

namespace Manmaru.Enemy
{
    /// <summary>
    /// ターゲットが視界の中にいるかを計算で判定する純粋なC#クラス
    /// </summary>
    public class EnemyVisionSensor
    {
        // 内部変数：干渉判定用
        private Transform _myTransform;
        private Transform _targetTransform;
        private EnemyDataSO _data;

        // コンストラクタ
        public EnemyVisionSensor(Transform myTrans, Transform targetTrans, EnemyDataSO data)
        {
            _myTransform = myTrans;
            _targetTransform = targetTrans;
            _data = data;
        }

        /// <summary>
        /// 視界の中にターゲットがいたらTrue、いなかったらFalseを返す
        /// </summary>
        public bool IsTargetInSight
        {
            get
            {
                // 判定用の距離2乗
                float sightDistSqr = _data.SightRange * _data.SightRange;

                // 自分との距離を判定
                Vector3 dirToTarget = _targetTransform.position - _myTransform.position;
                float distSqr = dirToTarget.sqrMagnitude;
                if (distSqr > sightDistSqr) return false;

                // 自分との角度（内積）を判定
                float dot = Vector3.Dot(_myTransform.forward, dirToTarget.normalized);
                float dotThreshold = Mathf.Cos(_data.SightAngle * Mathf.Deg2Rad);
                if (dot < dotThreshold) return false;

                return true;
            }
        }
    }
}