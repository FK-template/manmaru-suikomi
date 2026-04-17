using Manmaru.System;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// ゲームクリア画面の表示制御とボタン入力を管理するクラス
    /// </summary>
    public class GameClearUIController : BaseUIController
    {
        [Header("ボタン設定")]
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _titleButton;

        protected override void RegisterEvents()
        {
            // イベント購読設定
            GameStateManager.Instance.OnGameClearState += ShowUI;

            // ボタンの役割設定
            _retryButton.onClick.AddListener(_sceneFlowController.ReloadCurrentScene);
            _nextButton.onClick.AddListener(_sceneFlowController.MoveToNextScene);
            _titleButton.onClick.AddListener(_sceneFlowController.MoveToTitleScene);
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnGameClearState -= ShowUI;
            }
        }
    }
}