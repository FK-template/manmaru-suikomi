namespace Manmaru.Movement
{
    /// <summary>
    /// 重力計算に必要なパラメータを提供するためのインターフェース
    /// </summary>
    public interface IGravityParameters
    {
        public float Gravity { get; }
        public float MaxFallSpeed { get; }
        public float JumpTopThreshold { get; }
        public float JumpTopGravityScale { get; }
    }
}