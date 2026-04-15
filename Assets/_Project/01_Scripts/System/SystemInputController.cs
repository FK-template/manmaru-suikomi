using UnityEngine;
using UnityEngine.InputSystem;

namespace Manmaru.System
{
    /// <summary>
    /// ゲーム全体のシステム操作入力に応じた処理を呼び出すクラス
    /// </summary>
    public class SystemInputController : MonoBehaviour
    {
        [Header("入力設定")]
        [SerializeField] private InputActionReference _pauseActionInput;

        void Update()
        {
            if (_pauseActionInput.action.WasPressedThisFrame())
            {
                TogglePause();
            }
        }

        /// <summary>
        /// ポーズ状態の切り替えを行うメソッド
        /// </summary>
        private void TogglePause()
        {
            var stateManager = GameStateManager.Instance;

            if(stateManager.CurrentState == GameStateManager.GameState.Playing)
            {
                stateManager.ChangeToPauseState();
            }
            else if (stateManager.CurrentState == GameStateManager.GameState.Pause)
            {
                stateManager.ChangeToPlayingState();
            }
        }
    }
}