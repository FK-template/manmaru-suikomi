using Manmaru.Effect;
using UnityEngine;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーのエフェクト生成を統括するクラス
    /// </summary>
    public class PlayerEffectSpawner : MonoBehaviour
    {
        [Header("エフェクト設定")]
        [SerializeField] private OneShotEffectHandler _damagedEffect;

        [Header("依存クラス設定")]
        [SerializeField] private PlayerHealthController _healthController;

        void Start()
        {
            // イベント購読設定
            _healthController.OnTookDamage += SpawnDamagedEffect;
        }

        /// <summary>
        /// 被ダメージ時のエフェクトを生成するメソッド
        /// </summary>
        private void SpawnDamagedEffect()
        {
            Instantiate(_damagedEffect, transform.position, Quaternion.identity);
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (_healthController != null)
            {
                _healthController.OnTookDamage -= SpawnDamagedEffect;
            }
        }
    }
}