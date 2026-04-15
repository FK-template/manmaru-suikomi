using UnityEngine;

namespace Manmaru.System
{
    /// <summary>
    /// ゲームの時間の流れを制御するクラス
    /// </summary>
    public class GameTimeController : MonoBehaviour
    {
        void Start()
        {
            if (GameStateManager.Instance.CurrentState == GameStateManager.GameState.Playing)
            {
                StartTime();
            }

            // イベント購読設定
            GameStateManager.Instance.OnPauseState += StopTime;
            GameStateManager.Instance.OnResumed += StartTime;
        }

        /// <summary>
        /// タイムスケールを標準値にするメソッド
        /// </summary>
        private void StartTime()
        {
            Time.timeScale = 1f;
        }

        /// <summary>
        /// タイムスケールをゼロにするメソッド
        /// </summary>
        private void StopTime()
        {
            Time.timeScale = 0f;
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnPauseState -= StopTime;
                GameStateManager.Instance.OnResumed -= StartTime;
            }
        }
    }
}