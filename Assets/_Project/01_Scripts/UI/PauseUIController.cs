using Manmaru.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// ポーズ画面に関するUI処理を制御するクラス
    /// </summary>
    public class PauseUIController : MonoBehaviour
    {
        [Header("ポーズUI")]
        [SerializeField] private TextMeshProUGUI _pauseText;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _titleButton;

        [Header("依存クラス設定")]
        [SerializeField] private SceneFlowController _sceneFlowController;

        void Start()
        {
            // イベント購読設定
            GameStateManager.Instance.OnPauseState += ShowPauseUI;
            GameStateManager.Instance.OnResumed += HidePauseUI;

            _closeButton.onClick.AddListener(GameStateManager.Instance.ChangeToPlayingState);
            _retryButton.onClick.AddListener(_sceneFlowController.ReloadCurrentScene);
            _titleButton.onClick.AddListener(_sceneFlowController.MoveToTitleScene);
        }

        /// <summary>
        /// ポーズ画面に必要なUIを表示するメソッド
        /// </summary>
        private void ShowPauseUI()
        {
            // UI表示
            _pauseText.gameObject.SetActive(true);
            _closeButton.gameObject.SetActive(true);
            _retryButton.gameObject.SetActive(true);
            _titleButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// ポーズ画面に必要なUIを非表示にするメソッド
        /// </summary>
        private void HidePauseUI()
        {
            // UI表示
            _pauseText.gameObject.SetActive(false);
            _closeButton.gameObject.SetActive(false);
            _retryButton.gameObject.SetActive(false);
            _titleButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnPauseState -= ShowPauseUI;
                GameStateManager.Instance.OnResumed -= HidePauseUI;
            }
        }
    }
}