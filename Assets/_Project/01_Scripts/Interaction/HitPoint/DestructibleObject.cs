using Manmaru.Effect;
using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// ダメージを受けうるオブジェクトの体力管理を行うクラス
    /// </summary>
    public class DestructibleObject : MonoBehaviour, IDamageable
    {
        [Header("体力パラメータ設定")]
        [SerializeField] private float _hitPoint = 1.0f;
        [SerializeField] private float _maxHitPoint = 1.0f;

        [Header("エフェクト設定")]
        [SerializeField] private OneShotEffectHandler _hitEffect;

        // 内部変数：すいこみ候補としての自分
        private ICapturable _capturable;

        // 内部変数：すいこみオブジェクトの管理者（リスト除名用）
        private CaptureTargetManager _captureTargetManager;

        void Start()
        {
            _hitPoint = _maxHitPoint;
            _capturable = GetComponent<ICapturable>();
            _captureTargetManager = CaptureTargetManager.Instance;
        }

        /// <summary>
        /// 任意のダメージをくらい、体力がゼロ以下になったら消滅するメソッド
        /// </summary>
        public void TakeDamage(float damageValue)
        {
            // 被ダメージ処理
            _hitPoint -= damageValue;
            Debug.Log($"くらった！：{gameObject.name}({_hitPoint}/{_maxHitPoint})");


            // エフェクト生成
            Instantiate(_hitEffect, transform.position, Quaternion.identity);

            // やられ処理
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