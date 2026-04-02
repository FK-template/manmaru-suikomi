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
        [SerializeField] private PlayerMoveParameters _normalParams;
        [SerializeField] private PlayerMoveParameters _capturingParams;
        [SerializeField] private PlayerMoveParameters _mouthfulParams;
        [SerializeField] private PlayerMoveParameters _damagedParams;

        // 現在のプレイヤーの状態
        public PlayerState CurrentState { get; private set;}

        // 状態遷移イベント（汎用とやられ用）
        public Action<PlayerState, PlayerMoveParameters> OnStateChanged;
        public Action OnPlayerDead;

        void Start()
        {
            // パラメータ初期化
            ChangeState(PlayerState.Normal);
        }

        /// <summary>
        /// プレイヤーの状態を遷移させ、それに応じてパラメータを受け渡すActionを発火するメソッド
        /// </summary>
        public void ChangeState(PlayerState nextState)
        {
            // 現在の状態に重複遷移しようとしてたら、早期リターン
            if (CurrentState == nextState) return;

            CurrentState = nextState;

            switch (CurrentState)
            {
                case PlayerState.Normal:
                    OnStateChanged?.Invoke(CurrentState, _normalParams);
                    Debug.Log($"PlayerState:{CurrentState} 通常モードへ");
                    break;
                case PlayerState.Vacuuming:
                    OnStateChanged?.Invoke(CurrentState, _capturingParams);
                    Debug.Log($"PlayerState:{CurrentState} すいこみ状態スタート！");
                    break;
                case PlayerState.Capturing:
                    // すいこみ成功時のイベント発火はホリュウ
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