using Manmaru.Audio;
using UnityEngine;

namespace Manmaru.System
{
    /// <summary>
    /// ゲーム全体の状態遷移に応じた演出処理を制御するクラス
    /// </summary>
    public class GameStatePresenter : MonoBehaviour
    {
        [Header("ゲームオーバーパラメータ設定")]
        [SerializeField] private BGMDataSO _gameOverBGMData;

        [Header("ゲームクリアパラメータ設定")]
        [SerializeField] private BGMDataSO _gameClearBGMData;

        // 内部変数
        private GameStateManager _gameStateManager;
        private BGMPlayer _bgmPlayer;

        void Start()
        {
            _gameStateManager = GameStateManager.Instance;
            _bgmPlayer = BGMPlayer.Instance;

            // イベント購読設定
            _gameStateManager.OnGameOverState += PresentGameOver;
            _gameStateManager.OnGameClearState += PresentGameClear;
        }

        /// <summary>
        /// ゲームオーバー時の演出を再生するメソッド
        /// </summary>
        private void PresentGameOver()
        {
            _bgmPlayer.PlayJingleAndBGM(_gameOverBGMData);
        }

        /// <summary>
        /// ゲームクリア時の演出を再生するメソッド
        /// </summary>
        private void PresentGameClear()
        {
            _bgmPlayer.PlayJingleAndBGM(_gameClearBGMData);
        }

        private void OnDestroy()
        {
            if (_gameStateManager != null)
            {
                // イベント購読解除
                _gameStateManager.OnGameOverState -= PresentGameOver;
                _gameStateManager.OnGameClearState -= PresentGameClear;
            }
        }
    }
}