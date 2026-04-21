using Manmaru.System;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// ゲームオーバー画面の表示制御とボタン入力を管理するクラス
    /// </summary>
    public class GameOverScreen : BaseScreen
    {
        [Header("ボタン設定")]
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _titleButton;

        [Header("シーン管理クラス")]
        [SerializeField] private SceneFlowController _sceneFlowController;

        protected override void RegisterEvents()
        {
            // イベント購読設定
            GameStateManager.Instance.OnGameOverState += ShowUI;

            // ボタンの役割設定
            _retryButton.onClick.AddListener(_sceneFlowController.ReloadCurrentScene);
            _titleButton.onClick.AddListener(() => _sceneFlowController.LoadSceneByName(_sceneFlowController.TitleSceneName));
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnGameOverState -= ShowUI;
            }
        }
    }
}