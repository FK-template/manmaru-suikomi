using Manmaru.Interaction;
using UnityEngine;

namespace Manmaru.Player
{
    public class PlayerHealthController : MonoBehaviour, IDamageable
    {
        [Header("体力")]
        [SerializeField] private float _currentHitPoint = 1.0f;
        [SerializeField] private float _maxHitPoint = 1.0f;

        [Header("無敵")]
        [SerializeField] private float _noDamageTimer = 0f;
        [SerializeField] private float _noDamageFullTime = 3.0f;

        [Header("依存クラス設定")]
        [SerializeField] private PlayerStateManager _playerStateManager;

        void Start()
        {
            _currentHitPoint = _maxHitPoint;
        }

        void Update()
        {
            if (_noDamageTimer > 0f) 
            {
                _noDamageTimer -= Time.deltaTime;
            }
        }

        /// <summary>
        /// 任意のダメージをくらい、体力がゼロ以下になったら消滅するメソッド
        /// </summary>
        public void TakeDamage(float damageValue)
        {
            // 無敵タイマーが動いていたら、ダメージを受けない
            if (_noDamageTimer > 0f) return;

            // 被ダメージ処理
            _currentHitPoint -= damageValue;
            Debug.Log($"くらった！：{gameObject.name}({_currentHitPoint}/{_maxHitPoint})");

            // やられ処理
            if (_currentHitPoint <= 0)
            {
                OnDeath();
                return;
            }

            // 無敵タイマー起動
            _noDamageTimer = _noDamageFullTime;

            // 状態遷移
            _playerStateManager.ChangeState(PlayerStateManager.PlayerState.Damaged);
        }

        /// <summary>
        /// 消滅メソッド
        /// </summary>
        private void OnDeath()
        {
            Debug.Log($"やられた！：{gameObject.name}");
            Destroy(gameObject);
        }
    }
}