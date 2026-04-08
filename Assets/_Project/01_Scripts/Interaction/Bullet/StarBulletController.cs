using Manmaru.Effect;
using Unity.VisualScripting;
using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// はきだし弾のUpdate処理フローをまとめたクラス
    /// </summary>
    public class StarBulletController : MonoBehaviour
    {
        [Header("移動パラメータ設定")]
        [SerializeField] private float _baseMoveSpeed = 15.0f;
        [SerializeField] private float _minMoveSpeed = 10.0f;
        [SerializeField] private float _rotateAngle = 0.5f;

        [Header("コリジョン設定")]
        [SerializeField] private float _baseHitSphereRadius = 0.75f;
        [SerializeField] private LayerMask _targetMask;

        [Header("貫通弾パラメータ設定")]
        [SerializeField] private int _penetrateThreshold = 2;
        [SerializeField] private float _scaleStep = 0.5f;
        [SerializeField] private float _speedStep = 1.0f;
        [Space(5)]
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _superColor = Color.green;
        [SerializeField] private Color _ultraColor = Color.orangeRed;

        [Header("エフェクト設定")]
        [SerializeField] private OneShotEffectHandler _impactEffect;

        [Header("依存クラス設定")]
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

            // 地面にぶつからないように少し高めに移動
            transform.Translate(0f, _currentSphereRadius, 0f);
        }

        void Update()
        {
            // このフレームで進む予定の距離（衝突判定と実際の移動処理に使用）
            float moveDist = _currentMoveSpeed * Time.deltaTime;

            // 衝突処理
            if (_bulletCollision.CheckHitsBySphereRay(_shootDir, moveDist, _currentMoveSpeed, _currentSphereRadius, _targetMask, out RaycastHit[] hits))
            {
                bool shouldDestroy = HitsProcess(hits);

                if (shouldDestroy)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            // 移動処理（消滅するならスルー）
            _bulletMovement.Move(_shootDir, moveDist, _rotateAngle);
        }

        /// <summary>
        /// 衝突処理を衝突相手ぶんだけ繰り返し、自身を消滅させるべきかをboolで返すメソッド
        /// </summary>
        private bool HitsProcess(RaycastHit[] hits)
        {
            // ※無貫通はきだし弾であっても、同時に複数体に当たった場合はいずれの対象にも与ダメする
            bool isDestroy = false;

            foreach (RaycastHit hit in hits)
            {
                // 一体でも「消滅すべき」と判断したらフラグオン（もうオフにはならない）
                if (SingleHitProcess(hit))
                {
                    isDestroy = true;
                }
            }

            return isDestroy;
        }

        /// <summary>
        /// 各衝突相手への衝突時の処理を行い、消滅すべきかをboolで返すメソッド
        /// </summary>
        private bool SingleHitProcess(RaycastHit hit)
        {
            Debug.Log($"弾:[{gameObject.name}] が [{hit.transform.gameObject.name}] に あたりました");

            // 与ダメージ処理（与ダメできる相手なら）
            if (hit.collider.TryGetComponent(out IDamageable dmgTarget))
            {
                dmgTarget.TakeDamage(_hitPower);

                // 貫通 or 消滅
                if (_canPenetrate)
                {
                    // 衝突判定が重複しないよう、相手のコリジョンをオフに
                    hit.collider.enabled = false;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                // ダメージを与えられない相手なら、エフェクトを生成して消滅する
                SpawnImpactEffect();
                return true;
            }
        }

        /// <summary>
        /// ダメージを与えられない相手に当たった時のエフェクトを生成するメソッド
        /// </summary>
        private void SpawnImpactEffect()
        {
            Instantiate(_impactEffect, transform.position, Quaternion.identity);
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