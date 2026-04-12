using Manmaru.Movement;
using UnityEngine;

namespace Manmaru.Enemy
{
    /// <summary>
    /// 敵の種別ごとの基本パラメータをまとめたデータアセット
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Manmaru/EnemyData")]
    public class EnemyDataSO : ScriptableObject, IGravityParameters
    {
        [Header("基本パラメータ")]
        public float MaxHP = 1.0f;

        [Header("当たり判定パラメータ")]
        public float BodyRadius = 0.5f;

        [Header("基本移動パラメータ")]
        public float RotationSpeed = 360.0f;
        public float HighRotationSpeed = 720.0f;

        public float ActionWaitSecond = 1.0f;
        public float NoticeWaitSecond = 0.5f;
        public float CooldownWaitSecond = 3.0f;

        [Header("うろうろ移動パラメータ")]
        public float PatrolSpeed = 1.0f;
        public float PatrolRadius = 5.0f;
        public float ReachThreshold = 1.0f;

        [Header("とっしん移動パラメータ")]
        public float DashSpeed = 3.0f;
        public float DashDurationSecond = 5.0f;

        [Header("視界センサーパラメータ")]
        public bool CanDetectPlayer = true;
        public float SightRange = 3.0f;
        public float SightAngle = 30.0f;

        [Header("重力用パラメータ")]
        [SerializeField] private float _gravity = 70.0f;
        [SerializeField] private float _maxFallSpeed = -20.0f;
        [Space(5)]
        [SerializeField] private float _jumpTopThreshold = 5.0f;
        [SerializeField] private float _jumpTopGravityScale = 0.5f;

        // プロパティ：重力用パラメータ
        public float Gravity => _gravity;
        public float MaxFallSpeed => _maxFallSpeed;
        public float JumpTopThreshold => _jumpTopThreshold;
        public float JumpTopGravityScale => _jumpTopGravityScale;
    }
}