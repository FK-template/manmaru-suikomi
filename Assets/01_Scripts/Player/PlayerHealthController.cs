using Manmaru.Interaction;
using System;
using UnityEngine;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーの体力処理全般を制御するクラス
    /// </summary>
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

        // 状態遷移イベント：体力を返す
        public Action<float, float> OnDamaged;

        // 状態遷移イベント：点滅タイミング
        public Action OnNoDamageStarted;
        public Action OnNoDamageFinished;

        void Start()
        {
            _currentHitPoint = _maxHitPoint;
            OnDamaged.Invoke(_maxHitPoint, _currentHitPoint);
        }

        void Update()
        {
            if (_noDamageTimer > 0f)
            {
                _noDamageTimer -= Time.deltaTime;
                if (_noDamageTimer <= 0f) OnNoDamageFinished.Invoke();
            }
        }

        /// <summary>
        /// 任意のダメージをくらい、体力がゼロ以下になったら消滅するメソッド
        /// </summary>
        public void TakeDamage(float damageValue)
        {
            // 無敵タイマーが動いている or 体力ゼロ なら、ダメージを受けない
            if (_noDamageTimer > 0f || _currentHitPoint <= 0) return;

            // HP減算処理
            _currentHitPoint -= damageValue;
            Debug.Log($"くらった！：{gameObject.name}({_currentHitPoint}/{_maxHitPoint})");

            // UI更新Action
            OnDamaged.Invoke(_maxHitPoint, _currentHitPoint);

            // やられ処理
            if (_currentHitPoint <= 0)
            {
                OnDeath();
                return;
            }

            // 無敵タイマー起動
            _noDamageTimer = _noDamageFullTime;

            // 色替えAction
            OnNoDamageStarted.Invoke();

            // 状態遷移
            _playerStateManager.ChangeState(PlayerStateManager.PlayerState.Damaged);
        }

        /// <summary>
        /// やられ状態に移行するメソッド
        /// </summary>
        private void OnDeath()
        {
            _playerStateManager.ChangeState(PlayerStateManager.PlayerState.Dead);
        }
    }
}