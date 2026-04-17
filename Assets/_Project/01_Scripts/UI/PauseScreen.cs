using Manmaru.System;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// ポーズ画面の表示制御とボタン入力を管理するクラス
    /// </summary>
    public class PauseScreen : BaseScreen
    {
        [Header("ボタン設定")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _titleButton;

        protected override void RegisterEvents()
        {
            // イベント購読設定
            GameStateManager.Instance.OnPauseState += ShowUI;
            GameStateManager.Instance.OnResumed += HideUI;

            // ボタンの役割設定
            _closeButton.onClick.AddListener(GameStateManager.Instance.ChangeToPlayingState);
            _retryButton.onClick.AddListener(_sceneFlowController.ReloadCurrentScene);
            _titleButton.onClick.AddListener(() => _sceneFlowController.LoadSceneByName(_sceneFlowController.TitleSceneName));
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnPauseState -= ShowUI;
                GameStateManager.Instance.OnResumed -= HideUI;
            }
        }
    }
}