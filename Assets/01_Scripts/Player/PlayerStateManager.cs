using System;
using UnityEngine;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーの状態を管理するクラス
    /// </summary>
    public class PlayerStateManager : MonoBehaviour
    {
        public enum PlayerState
        {
            Normal,
            Vacuuming,  // すいこみ状態
            Capturing,  // ひきよせ状態（すいこみ成功）
            Mouthful,
            Damaged,
            Dead
        }

        [Header("状態ごとのパラメータデータ")]
        [SerializeField] private PlayerMoveParametersSO _normalParams;
        [SerializeField] private PlayerMoveParametersSO _vacuumingParams;
        [SerializeField] private PlayerMoveParametersSO _capturingParams;
        [SerializeField] private PlayerMoveParametersSO _mouthfulParams;
        [SerializeField] private PlayerMoveParametersSO _damagedParams;

        // 現在のプレイヤーの状態
        public PlayerState CurrentState { get; private set;}

        // 状態遷移イベント（汎用とやられ用）
        public Action<PlayerState, PlayerMoveParametersSO> OnStateChanged;
        public Action OnPlayerDead;

        // 内部変数：強制状態遷移フラグ（主に状態の初期設定に使う）
        private bool _force = true;

        void Start()
        {
            // パラメータを初期化し、強制フラグを無効化
            ChangeState(PlayerState.Normal);
            _force = false;
        }

        /// <summary>
        /// プレイヤーの状態を遷移させ、それに応じてパラメータを受け渡すActionを発火するメソッド
        /// </summary>
        public void ChangeState(PlayerState nextState)
        {
            // 現在の状態に重複遷移しようとしてたら、早期リターン
            if (!_force && CurrentState == nextState) return;

            CurrentState = nextState;

            switch (CurrentState)
            {
                case PlayerState.Normal:
                    OnStateChanged?.Invoke(CurrentState, _normalParams);
                    Debug.Log($"PlayerState:{CurrentState} 通常モードへ");
                    break;
                case PlayerState.Vacuuming:
                    OnStateChanged?.Invoke(CurrentState, _vacuumingParams);
                    Debug.Log($"PlayerState:{CurrentState} すいこみ状態スタート！");
                    break;
                case PlayerState.Capturing:
                    OnStateChanged?.Invoke(CurrentState, _capturingParams);
                    Debug.Log($"PlayerState:{CurrentState} すいこみ成功！すいこみを自動継続します");
                    break;
                case PlayerState.Mouthful:
                    OnStateChanged?.Invoke(CurrentState, _mouthfulParams);
                    Debug.Log($"PlayerState:{CurrentState} すいこみ完全完了！ほおばりモードへ");
                    break;
                case PlayerState.Damaged:
                    OnStateChanged?.Invoke(CurrentState, _damagedParams);
                    Debug.Log($"PlayerState:{CurrentState} いてっ！被ダメージモードへ");
                    break;
                case PlayerState.Dead:
                    OnPlayerDead?.Invoke();
                    Debug.Log($"PlayerState:{CurrentState} やられた！ゲームオーバーモードへ");
                    break;
            }
        }
    }
}