using UnityEngine;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーの状態別移動パラメータをまとめたデータアセット
    /// </summary>
    [CreateAssetMenu(fileName = "NewPlayerMoveParams", menuName = "Manmaru/PlayerMoveParameters")]
    public class PlayerMoveParametersSO : ScriptableObject
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
        public float Gravity = 70.0f;
        public float MaxFallSpeed = -20.0f;
        [Space(5)]
        public float BrakeThreshold = 5.0f;
        public float BrakeGravityMultiplier = 0.5f;
    }
}