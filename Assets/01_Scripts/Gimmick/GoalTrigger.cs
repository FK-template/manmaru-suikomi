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

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
                GameStateManager.Instance.ChangeToGameClearState();
        }
    }
}