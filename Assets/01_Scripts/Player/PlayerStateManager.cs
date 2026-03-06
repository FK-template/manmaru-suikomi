using System;
using UnityEngine;

namespace Manmaru.Player
{
    public class PlayerStateManager : MonoBehaviour
    {
        public enum PlayerState
        {
            Normal,
            Capturing,
            Mouthful
        }

        [Header("状態ごとのパラメータデータ")]
        [SerializeField] private PlayerMoveParameters _normalParams;
        [SerializeField] private PlayerMoveParameters _capturingParams;
        [SerializeField] private PlayerMoveParameters _mouthfulParams;

        // 内部変数：状態管理
        private PlayerState _currentState;

        // 状態遷移イベント
        public Action<PlayerMoveParameters> OnStateChanged;

        void Start()
        {
            // パラメータ初期化
            ChangeState(PlayerState.Normal);
        }

        /// <summary>
        /// プレイヤーの状態を遷移させ、それに応じてパラメータを受け渡すActionを発火するメソッド
        /// </summary>
        public void ChangeState(PlayerState nextState)
        {
            _currentState = nextState;

            switch (_currentState)
            {
                case PlayerState.Normal:
                    OnStateChanged.Invoke(_normalParams);
                    Debug.Log($"PlayerState:{_currentState} 通常モードへ");
                    break;
                case PlayerState.Capturing:
                    OnStateChanged.Invoke(_capturingParams);
                    Debug.Log($"PlayerState:{_currentState} すいこみ状態スタート！");
                    break;
                case PlayerState.Mouthful:
                    OnStateChanged?.Invoke(_mouthfulParams);
                    Debug.Log($"PlayerState:{_currentState} すいこみ完全完了！ほおばりモードへ");
                    break;
            }
        }
    }
}