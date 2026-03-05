using Manmaru.Interaction;
using UnityEngine;

namespace Manmaru.Interaction
{
    public class EnemyHitPoint : MonoBehaviour, IDamageable
    {
        [Header("体力")]
        [SerializeField] private float _hitPoint = 1.0f;
        [SerializeField] private float _maxHitPoint = 1.0f;

        [Header("やられ処理用設定")]
        [Tooltip("すいこみ候補リストの管理者")]
        [SerializeField] private CaptureTargetManager _captureTargetManager;

        // 内部変数：すいこみ候補としての自分
        private ICapturable _capturable;

        void Start()
        {
            _hitPoint = _maxHitPoint;
            _capturable = GetComponent<ICapturable>();
        }

        /// <summary>
        /// 任意のダメージをくらい、体力がゼロ以下になったら消滅するメソッド
        /// </summary>
        /// <param name="damageValue"></param>
        public void TakeDamage(float damageValue)
        {
            // 被ダメージ処理
            _hitPoint -= damageValue;
            Debug.Log($"くらった！：{gameObject.name}({_hitPoint}/{_maxHitPoint})");

            if (_hitPoint <= 0)
            {
                // すいこみ候補リストからも、世界からも、消滅
                Debug.Log($"やられた！：{gameObject.name}");
                _captureTargetManager.UnregisterCapturableTarget(_capturable);
                Destroy(gameObject);
            }
        }
    }
}