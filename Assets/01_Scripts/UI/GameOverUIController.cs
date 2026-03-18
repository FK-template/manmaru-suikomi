using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.System
{
    public class GameOverUIController : MonoBehaviour
    {
        [Header("ゲームオーバーUI")]
        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _titleButton;

        [Header("依存クラス設定")]
        [SerializeField] private GameStateManager _gameStateManager;
        [SerializeField] private SceneFlowController _sceneFlowController;

        void Start()
        {
            _gameStateManager.OnGameOverState += ShowGameOverUI;

            _retryButton.onClick.AddListener(_sceneFlowController.ReloadCurrentScene);
            _titleButton.onClick.AddListener(_sceneFlowController.MoveToTitleScene);
        }

        private void ShowGameOverUI()
        {
            // UI表示
            _gameOverText.gameObject.SetActive(true);
            _retryButton.gameObject.SetActive(true);
            _titleButton.gameObject.SetActive(true);
        }
    }
}