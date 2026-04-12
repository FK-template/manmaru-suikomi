using Manmaru.Movement;
using UnityEngine;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーの状態別移動パラメータをまとめたデータアセット
    /// </summary>
    [CreateAssetMenu(fileName = "NewPlayerMoveParams", menuName = "Manmaru/PlayerMoveParameters")]
    public class PlayerMoveParametersSO : ScriptableObject, IGravityParameters
    {
        [Header("移動許可フラグ")]
        public bool CanJump = true;

        [Header("水平移動用パラメータ")]
        public float MaxSpeed = 8.0f;
        [Space(5)]
        public float GroundAcceleration = 50.0f;
        public float GroundDeceleration = 60.0f;
        [Space(5)]
        public float AirAcceleration = 20.0f;
        public float AirDeceleration = 5.0f;

        [Header("回転用パラメータ")]
        public float RotationSpeed = 720.0f;

        [Header("ジャンプ用パラメータ")]
        public float JumpForce = 23.5f;
        public float JumpCutoffMultiplier = 0.4f;

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