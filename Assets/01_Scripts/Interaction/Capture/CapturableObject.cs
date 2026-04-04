using Manmaru.Enemy;
using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// すいこまれるオブジェクトの処理をまとめたクラス
    /// </summary>
    public class CapturableObject : MonoBehaviour, ICapturable
    {
        [Header("すいこみ質量")]
        [SerializeField] private int _captureMass = 1;

        [Header("すいこみアニメーション設定")]
        [SerializeField] private float _captureDuration = 0.5f;
        [SerializeField] private AnimationCurve _captureCurve;

        // PlayerCaptureControllerから参照するためのプロパティ
        public int CaptureMass => _captureMass;

        // 内部変数：すいこみ用
        private bool _isCapturing = false;
        private float _captureTimer = 0f;
        private Vector3 _startPos;
        private Transform _playerTrans;

        // 内部変数：すいこみオブジェクトの管理者（リスト除名・記名用）
        private CaptureTargetManager _captureTargetManager;

        void Start()
        {
            _captureTargetManager = CaptureTargetManager.Instance;
            _captureTargetManager.RegisterCapturableTarget(this);
        }

        void Update()
        {
            if (_isCapturing)
            {
                UpdateCapturingAnimation();
            }
        }

        /// <summary>
        /// Transformを取得して返すメソッド
        /// </summary>
        /// <remarks>外部からインターフェースを用いて検索をかけるため、改めてTransformを返す必要がある</remarks>
        public Transform GetTransform()
        {
            return transform;
        }

        /// <summary>
        /// すいこまれ始めたときのセットアップを行うメソッド
        /// </summary>
        public void OnCapture(Transform playerTrans)
        {
            // すいこみ中処理のセットアップ
            _isCapturing = true;
            _captureTimer = 0f;
            _startPos = transform.position;
            _playerTrans = playerTrans;

            // 無力化（当たり判定、移動、ダメージ判定など）
            if (TryGetComponent<Collider>(out var col)) col.enabled = false;
            if (TryGetComponent<EnemyMoveController>(out var mover)) mover.enabled = false;
            if (TryGetComponent<EnemyBehaviourController>(out var brain)) brain.enabled = false;
            if (TryGetComponent<DamageSource>(out var dmgSrc)) dmgSrc.enabled = false;

            // すいこみ候補リストから、すいこみ中リストへ移動
            _captureTargetManager.UnregisterCapturableTarget(this);
            _captureTargetManager.RegisterCapturingTarget(this);
        }

        /// <summary>
        /// すいこみ中のアニメーション処理を行うメソッド
        /// </summary>
        private void UpdateCapturingAnimation()
        {
            // 経過時間に対応したカーブの値を取得
            _captureTimer += Time.deltaTime;
            float clampTime = Mathf.Clamp01(_captureTimer / _captureDuration);
            float curveValue = _captureCurve.Evaluate(clampTime);

            // カーブの値に応じた移動
            transform.position = Vector3.LerpUnclamped(_startPos, _playerTrans.position, curveValue);

            // すいこみ完了処理
            if (curveValue >= 1.0f)
            {
                _isCapturing = false;
                _captureTargetManager.NotifyCaptureCompleted(this);
                Destroy(gameObject);
            }
        }
    }
}