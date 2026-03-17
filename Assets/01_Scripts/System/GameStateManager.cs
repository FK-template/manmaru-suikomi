using System;
using Manmaru.Player;
using UnityEngine;

namespace Manmaru.System
{
    public class GameStateManager : MonoBehaviour
    {
        public enum GameState
        {
            Playing,
            GameOver,
            GameClear
        }

        [Header("依存クラス設定")]
        [SerializeField] private PlayerStateManager _playerStateManager;

        // 現在のゲーム状態
        public GameState CurrentState { get; private set; }

        // 状態遷移イベント
        public Action OnGameOverState;

        // インスタンス設定
        public static GameStateManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        void Start()
        {
            _playerStateManager.OnPlayerDead += OnPlayerDeadHandler;
        }

        private void OnPlayerDeadHandler()
        {
            ChangeGameState(GameState.GameOver);
        }

        /// <summary>
        /// ゲームの状態を遷移させ、Actionを発火するメソッド
        /// </summary>
        private void ChangeGameState(GameState nextState)
        {
            CurrentState = nextState;
            OnGameOverState?.Invoke();

            Debug.Log($"GameState:{CurrentState} ゲーム状態変更！");
        }
    }
}