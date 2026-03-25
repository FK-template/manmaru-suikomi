using Manmaru.System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーの操作入力の受付可否を制御するクラス
    /// </summary>
    public class PlayerInputLocker : MonoBehaviour
    {
        [Header("入力設定(ここからPlayerグループを取得)")]
        [SerializeField] private InputActionReference _anyPlayerAction;

        void Start()
        {
            // イベント購読設定
            GameStateManager.Instance.OnGameClearState += LockPlayerInput;
            GameStateManager.Instance.OnGameOverState += LockPlayerInput;
        }

        /// <summary>
        /// プレイヤーの操作入力を受付禁止にするメソッド
        /// </summary>
        private void LockPlayerInput()
        {
            _anyPlayerAction.action.actionMap.Disable();
        }

        /// <summary>
        /// プレイヤーの操作入力を受付開始するメソッド
        /// </summary>
        private void UnLockPlayerInput()
        {
            _anyPlayerAction.action.actionMap.Enable();
        }

        private void OnDisable()
        {
            UnLockPlayerInput();
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnGameClearState -= LockPlayerInput;
                GameStateManager.Instance.OnGameOverState -= LockPlayerInput;
            }
        }
    }
}