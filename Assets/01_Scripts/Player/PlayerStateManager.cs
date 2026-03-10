using System;
using UnityEngine;

namespace Manmaru.Player
{
    public class PlayerStateManager : MonoBehaviour
    {
        public enum PlayerState
        {
            Normal,
            Capturing,
            Mouthful,
            Damaged
        }

        [Header("状態ごとのパラメータデータ")]
        [SerializeField] private PlayerMoveParameters _normalParams;
        [SerializeField] private PlayerMoveParameters _capturingParams;
        [SerializeField] private PlayerMoveParameters _mouthfulParams;
        [SerializeField] private PlayerMoveParameters _damagedParams;

        // 内部変数：状態管理
        public PlayerState CurrentState { get; private set;}

        // 状態遷移イベント
        public Action<PlayerState, PlayerMoveParameters> OnStateChanged;

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
            CurrentState = nextState;

            switch (CurrentState)
            {
                case PlayerState.Normal:
                    OnStateChanged.Invoke(CurrentState, _normalParams);
                    Debug.Log($"PlayerState:{CurrentState} 通常モードへ");
                    break;
                case PlayerState.Capturing:
                    OnStateChanged.Invoke(CurrentState, _capturingParams);
                    Debug.Log($"PlayerState:{CurrentState} すいこみ状態スタート！");
                    break;
                case PlayerState.Mouthful:
                    OnStateChanged?.Invoke(CurrentState, _mouthfulParams);
                    Debug.Log($"PlayerState:{CurrentState} すいこみ完全完了！ほおばりモードへ");
                    break;
                case PlayerState.Damaged:
                    OnStateChanged?.Invoke(CurrentState, _damagedParams);
                    Debug.Log($"PlayerState:{CurrentState} いてっ！被ダメージモードへ");
                    break;
            }
        }
    }
}