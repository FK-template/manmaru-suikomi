using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// 干渉した対象にダメージを与えるクラス
    /// </summary>
    public class DamageSource : MonoBehaviour
    {
        [Header("ダメージ判定パラメータ")]
        [SerializeField] private float _hitCollisionRadius;
        [SerializeField] private float _hitDamage;
        [SerializeField] private LayerMask _targetLayer;

        [Header("依存クラス設定")]
        [SerializeField] private DamageAreaDetector _damageAreaDetector;

        // 内部変数
        private Collider[] _hitColliders;

        void Start()
        {

        }

        void Update()
        {
            _hitColliders = _damageAreaDetector.GetHittingColliders(transform.position, _hitCollisionRadius, _targetLayer);
            
            foreach (Collider col in _hitColliders)
            {
                // 与ダメージ処理（与ダメできる相手なら）
                if (col.TryGetComponent(out IDamageable dmgTarget))
                {
                    dmgTarget.TakeDamage(_hitDamage);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _hitCollisionRadius);
        }
    }
}