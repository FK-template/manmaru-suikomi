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

        [Header("プレイヤーの状態（デバッグ用に表示）")]
        public PlayerState CurrentState;
        public PlayerState PreviousState;

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

            // 前の状態として今の状態を格納（被ダメ状態とやられ状態はスルー）
            if (nextState != PlayerState.Damaged && nextState != PlayerState.Dead) PreviousState = nextState;

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

        /// <summary>
        /// イベント発火や前状態の更新を行わず、現在状態の更新のみを行うメソッド
        /// </summary>
        public void OnlyChangeState(PlayerState nextState)
        {
            CurrentState = nextState;
        }

        /// <summary>
        /// 1つ前の状態に復帰するメソッド
        /// </summary>
        public void ResumePreviousState()
        {
            ChangeState(PreviousState);
        }
    }
}