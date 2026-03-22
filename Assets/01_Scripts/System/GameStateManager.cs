using System;
using Manmaru.Player;
using UnityEngine;

namespace Manmaru.System
{
    /// <summary>
    /// ゲーム全体の状態を管理するクラス
    /// </summary>
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
        public Action OnGameClearState;

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

        /// <summary>
        /// ゲームをクリア状態に遷移させるメソッド
        /// </summary>
        public void ChangeToGameClearState()
        {
            ChangeGameState(GameState.GameClear);
            OnGameClearState?.Invoke();
        }

        /// <summary>
        /// プレイヤーやられ時のイベントハンドラメソッド
        /// </summary>
        private void OnPlayerDeadHandler()
        {
            ChangeGameState(GameState.GameOver);
            OnGameOverState?.Invoke();
        }

        /// <summary>
        /// ゲームの状態を遷移させるメソッド
        /// </summary>
        private void ChangeGameState(GameState nextState)
        {
            // すでにゲームが終了していたら中止
            if (CurrentState == GameState.GameClear || CurrentState == GameState.GameOver) return;

            CurrentState = nextState;
            Debug.Log($"GameState:{CurrentState} ゲーム状態変更！");
        }
    }
}