using Manmaru.Interaction;
using UnityEngine;

namespace Manmaru.Ability
{
    /// <summary>
    /// はきだし処理を行うクラス
    /// </summary>
    public class ShootAction : MonoBehaviour
    {
        [Header("はきだしパラメータ設定")]
        [SerializeField] private StarBulletController _starBullet;
        [SerializeField] private Transform _spawnTrans;

        [Header("エイムアシスト設定")]
        [SerializeField] private float _assistRadius = 1.5f;
        [SerializeField] private float _assistDistance = 15.0f;
        [SerializeField] private LayerMask _assistTargetLayer;

        /// <summary>
        /// はきだし弾を生成し、弾の初期設定を行うメソッド
        /// </summary>
        public void Shoot(int capturedCount)
        {
            // 弾の生成と初期化
            StarBulletController bullet = Instantiate(_starBullet, _spawnTrans.position, Quaternion.LookRotation(_spawnTrans.forward));
            Vector3 shootDir = CalculateAssistDirection(_spawnTrans.position, _spawnTrans.forward);
            bullet.Initialize(shootDir, capturedCount);

            Debug.Log($"はきだし！弾の強さ：Lv.{capturedCount}");
        }

        /// <summary>
        /// エイムアシストで発射方向を計算し、ベクトルを返すメソッド
        /// </summary>
        private Vector3 CalculateAssistDirection(Vector3 origin, Vector3 forward)
        {
            if(Physics.SphereCast(origin, _assistRadius, forward, out RaycastHit hit, _assistDistance, _assistTargetLayer))
            {
                if(hit.collider.TryGetComponent<IDamageable>(out _))
                {
                    Vector3 targetCenter = hit.collider.bounds.center;
                    Vector3 assistedDirection = (targetCenter - origin).normalized;

                    Debug.Log($"補正するぞ！{hit.collider.name}");
                    return assistedDirection;
                }
            }

            return forward;
        }

        // ----- 以下、Gemini 3.1 Pro より出力 -----

        /// <summary>
        /// Sceneビュー上でエイムアシストの範囲（SphereCast）を可視化するメソッド
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            // 発射口がアタッチされていない場合はエラーを防ぐために中断
            if (_spawnTrans == null) return;

            // ギズモ（描画線）の色を半透明の黄色に設定
            Gizmos.color = new Color(1.0f, 0.9f, 0.1f, 0.5f);

            Vector3 origin = _spawnTrans.position;
            Vector3 forward = _spawnTrans.forward;
            Vector3 endPos = origin + forward * _assistDistance;

            // 1. 始点と終点に球を描画
            Gizmos.DrawWireSphere(origin, _assistRadius);
            Gizmos.DrawWireSphere(endPos, _assistRadius);

            // 2. 球と球を繋ぐ4本の直線を描画して、円柱（カプセル）っぽく見せる！
            Vector3 up = _spawnTrans.up * _assistRadius;
            Vector3 right = _spawnTrans.right * _assistRadius;

            Gizmos.DrawLine(origin + up, endPos + up); // 上の線
            Gizmos.DrawLine(origin - up, endPos - up); // 下の線
            Gizmos.DrawLine(origin + right, endPos + right); // 右の線
            Gizmos.DrawLine(origin - right, endPos - right); // 左の線
        }
    }
}