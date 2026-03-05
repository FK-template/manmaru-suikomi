using UnityEngine;

namespace Manmaru.Interaction
{
    public class StarBulletController : MonoBehaviour
    {
        [Header("移動設定")]
        [SerializeField] private float _moveSpeed = 0.5f;

        [Header("コリジョン設定")]
        [SerializeField] private float _hitSphereRadius = 0.75f;
        [SerializeField] private LayerMask _targetMask;

        [Header("依存クラス")]
        [SerializeField] private StarBulletMovement _bulletMovement;
        [SerializeField] private StarBulletCollision _bulletCollision;

        // 内部変数：はきだし用
        private Vector3 _shootDir = Vector3.zero;

        void Update()
        {
            // このフレームで進む予定の距離（衝突判定と実際の移動処理に使用）
            float moveDist = _moveSpeed * Time.deltaTime;

            // 衝突判定
            if (_bulletCollision.CheckHitBySphereRay(_shootDir, moveDist, _moveSpeed, _hitSphereRadius, _targetMask))
            {
                Debug.Log($"{gameObject.name}：あたりました");
                Destroy(gameObject);

                // 衝突した場合、移動処理は行わない
                return;
            }

            // 移動処理
            _bulletMovement.Move(_shootDir, moveDist);
        }

        /// <summary>
        /// 生成されたときに呼ばれる、パラメータ初期化メソッド
        /// </summary>
        public void Initialize(Vector3 dir)
        {
            _shootDir = dir;
        }

        // ----- 以下、Gemini3 Pro より出力 -----
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _hitSphereRadius);
        }
        // -----
    }
}