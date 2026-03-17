using UnityEngine;

namespace Manmaru.System
{
    public class GameOverUIController : MonoBehaviour
    {
        [Header("ゲームオーバーUI")]
        [SerializeField] private GameObject _gameOverText;
        [SerializeField] private GameObject _nextButton;
        [SerializeField] private GameObject _retryButton;

        [Header("依存クラス設定")]
        [SerializeField] private GameStateManager _gameStateManager;

        void Start()
        {
            _gameStateManager.OnGameOverState += ShowGameOverUI;
        }

        private void ShowGameOverUI()
        {
            // UI表示
            _gameOverText.SetActive(true);
            _nextButton.SetActive(true);
            _retryButton.SetActive(true);
        }
    }
}