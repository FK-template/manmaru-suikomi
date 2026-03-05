using UnityEngine;

namespace Manmaru.Interaction
{
    public class CapturableObject : MonoBehaviour, ICapturable
    {
        [Header("すいこみオブジェクトの管理者")]
        [SerializeField] private CaptureTargetManager _captureTargetManager;

        [Header("すいこみアニメーション設定")]
        [SerializeField] private float _captureDuration = 0.5f;
        [SerializeField] private AnimationCurve _captureCurve;

        // 内部変数：はきだし用
        private bool _isShooting = false;
        private float _shootSpeed = 0.5f;
        private Vector3 _shootDir = Vector3.zero;

        // 内部変数：すいこみ用
        private bool _isCapturing = false;
        private float _captureTimer = 0f;
        private Vector3 _startPos;
        private Transform _playerTrans;

        void Start()
        {
            _captureTargetManager.RegisterTarget(this);
        }

        void Update()
        {
            if (_isCapturing)
            {
                UpdateCapturingAnimation();
            }
            else if (_isShooting)
            {
                transform.position += _shootDir * _shootSpeed;
            }
        }

        /// <summary>
        /// Transformを取得して返すメソッド
        /// </summary>
        public Transform GetTransform()
        {
            return transform;
        }

        /// <summary>
        /// すいこまれたときの位置補正と、すいこめるものリストからの自己削除を行うメソッド
        /// </summary>
        public void OnCapture(Transform playerTrans)
        {
            _isCapturing = true;
            _captureTimer = 0f;
            _startPos = transform.position;
            _playerTrans = playerTrans;

            GetComponent<Collider>().enabled = false;

            _captureTargetManager.UnregisterTarget(this);
        }

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

                transform.position = _playerTrans.position;
                transform.SetParent(_playerTrans);
            }
        }

        /// <summary>
        /// はきだされ処理の開始準備を行うメソッド
        /// </summary>
        public void OnShoot(Vector3 dir)
        {
            transform.SetParent(null);
            _isShooting = true;
            _shootDir = dir;
        }
    }
}