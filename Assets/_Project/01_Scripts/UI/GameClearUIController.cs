using Manmaru.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// ゲームクリアに関するUI処理を制御するクラス
    /// </summary>
    public class GameClearUIController : MonoBehaviour
    {
        [Header("ゲームクリアUI")]
        [SerializeField] private TextMeshProUGUI _gameClearText;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _titleButton;

        [Header("依存クラス設定")]
        [SerializeField] private SceneFlowController _sceneFlowController;

        void Start()
        {
            // イベント購読設定
            GameStateManager.Instance.OnGameClearState += ShowGameClearUI;

            _retryButton.onClick.AddListener(_sceneFlowController.ReloadCurrentScene);
            _nextButton.onClick.AddListener(_sceneFlowController.MoveToTitleScene);
            _titleButton.onClick.AddListener(_sceneFlowController.MoveToTitleScene);
        }

        /// <summary>
        /// ゲームクリア時に必要なUIを表示するメソッド
        /// </summary>
        private void ShowGameClearUI()
        {
            // UI表示
            _gameClearText.gameObject.SetActive(true);
            _retryButton.gameObject.SetActive(true);
            _nextButton.gameObject.SetActive(true);
            _titleButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnGameClearState -= ShowGameClearUI;
            }
        }
    }
}