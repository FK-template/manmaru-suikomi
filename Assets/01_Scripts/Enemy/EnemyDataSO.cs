using UnityEngine;

namespace Manmaru.Enemy
{
    /// <summary>
    /// 敵の種別ごとの基本パラメータをまとめたデータアセット
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Manmaru/EnemyData")]
    public class EnemyDataSO : ScriptableObject
    {
        [Header("基本ステータス")]
        public float MaxHP = 1.0f;

        [Header("うろうろ移動ステータス")]
        public float PatrolSpeed = 1.0f;
        public float PatrolRadius = 5.0f;
        public float ReachThreshold = 1.0f;
        
        public float DashSpeed = 3.0f;
        public float ActionWaitSecond = 1.0f;
        public float RotationSpeed = 360.0f;

        [Header("視界センサー（干渉判定）系")]
        public bool CanDetectPlayer = true;
        public float SightRange = 3.0f;
        public float SightAngle = 30.0f;
    }
}