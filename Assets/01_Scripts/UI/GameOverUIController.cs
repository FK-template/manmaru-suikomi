using Manmaru.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// ゲームオーバーに関するUI処理を制御するクラス
    /// </summary>
    public class GameOverUIController : MonoBehaviour
    {
        [Header("ゲームオーバーUI")]
        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _titleButton;

        [Header("依存クラス設定")]
        [SerializeField] private SceneFlowController _sceneFlowController;

        void Start()
        {
            // イベント購読設定
            GameStateManager.Instance.OnGameOverState += ShowGameOverUI;

            _retryButton.onClick.AddListener(_sceneFlowController.ReloadCurrentScene);
            _titleButton.onClick.AddListener(_sceneFlowController.MoveToTitleScene);
        }

        /// <summary>
        /// ゲームオーバー時に必要なUIを表示するメソッド
        /// </summary>
        private void ShowGameOverUI()
        {
            // UI表示
            _gameOverText.gameObject.SetActive(true);
            _retryButton.gameObject.SetActive(true);
            _titleButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            // イベント購読解除
            GameStateManager.Instance.OnGameOverState -= ShowGameOverUI;
        }
    }
}