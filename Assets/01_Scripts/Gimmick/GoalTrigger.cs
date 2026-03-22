using Manmaru.System;
using UnityEngine;

namespace Manmaru.Gimmick
{
    /// <summary>
    /// ステージのゴール地点での干渉判定を行うクラス
    /// </summary>
    public class GoalTrigger : MonoBehaviour
    {
        [Header("ゴール干渉判定を取るレイヤー")]
        [SerializeField] private LayerMask _playerLayer;

        // 内部変数：ゴール干渉フラグ
        private bool _isGoalReached;

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
            {
                // 多重クリア判定防止
                if (_isGoalReached) return;

                GameStateManager.Instance.ChangeToGameClearState();
                _isGoalReached = true;
            }
        }
    }
}