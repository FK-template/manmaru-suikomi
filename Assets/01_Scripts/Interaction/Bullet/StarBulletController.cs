using UnityEditor.PackageManager;
using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// はきだし弾のUpdate処理フローをまとめたクラス
    /// </summary>
    public class StarBulletController : MonoBehaviour
    {
        [Header("移動設定")]
        [SerializeField] private float _baseMoveSpeed = 15.0f;
        [SerializeField] private float _minMoveSpeed = 10.0f;

        [Header("コリジョン設定")]
        [SerializeField] private float _baseHitSphereRadius = 0.75f;
        [SerializeField] private LayerMask _targetMask;

        [Header("貫通弾設定")]
        [SerializeField] private int _penetrateThreshold = 2;
        [SerializeField] private float _scaleStep = 0.5f;
        [SerializeField] private float _speedStep = 1.0f;

        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _superColor = Color.green;
        [SerializeField] private Color _ultraColor = Color.orangeRed;

        [Header("依存クラス")]
        [SerializeField] private StarBulletMovement _bulletMovement;
        [SerializeField] private StarBulletCollision _bulletCollision;

        // 内部変数：はきだし用
        private Vector3 _shootDir;
        private float _hitPower;
        private float _currentMoveSpeed;
        private float _currentSphereRadius;
        private bool _canPenetrate;

        /// <summary>
        /// 生成されたときに呼ばれる、パラメータ初期設定メソッド
        /// </summary>
        public void Initialize(Vector3 dir, int capturedCount)
        {
            // 色設定
            if (capturedCount == 2)
            {
                _renderer.material.color = _superColor;
            }
            else if (capturedCount >= 3)
            {
                _renderer.material.color = _ultraColor;
            }

            // 移動方向
            _shootDir = dir;

            // すいこんだ数が攻撃力に
            _hitPower = capturedCount;

            // すいこんだ数に応じて、速度減衰とスケールアップ
            float speedDecayAmount = _speedStep * (capturedCount - 1);
            _currentMoveSpeed = Mathf.Max(_baseMoveSpeed - speedDecayAmount, _minMoveSpeed);

            float scaleMultiplier = 1.0f + _scaleStep * (capturedCount - 1);
            transform.localScale *= scaleMultiplier;
            _currentSphereRadius = _baseHitSphereRadius * scaleMultiplier;

            // 貫通の有無
            if (capturedCount >= _penetrateThreshold)
            {
                _canPenetrate = true;
            }
        }

        void Update()
        {
            // このフレームで進む予定の距離（衝突判定と実際の移動処理に使用）
            float moveDist = _currentMoveSpeed * Time.deltaTime;

            // 衝突判定
            if (_bulletCollision.CheckHitBySphereRay(_shootDir, moveDist, _currentMoveSpeed, _currentSphereRadius, _targetMask, out RaycastHit hit))
            {
                Debug.Log($"弾:[{gameObject.name}] が [{hit.transform.gameObject.name}] に あたりました");

                // 与ダメージ処理
                if (hit.collider.TryGetComponent(out IDamageable dmgTarget))
                {
                    dmgTarget.TakeDamage(_hitPower);
                }

                // 貫通or消滅処理
                if (_canPenetrate)
                {
                    hit.collider.enabled = false;
                }
                else
                {
                    Destroy(gameObject);
                }

                // 衝突した場合、移動処理は行わない
                return;
            }

            // 移動処理
            _bulletMovement.Move(_shootDir, moveDist);
        }

        // ----- 以下、Gemini3 Pro より出力 -----
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _currentSphereRadius);
        }
        // -----
    }
}