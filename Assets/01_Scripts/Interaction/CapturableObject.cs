using UnityEngine;

namespace Manmaru.Interaction
{
    public class CapturableObject : MonoBehaviour, ICapturable
    {
        [Header("すいこみオブジェクトの管理者")]
        [SerializeField] private CaptureTargetManager _captureTargetManager;

        // 内部変数
        private bool _isShooting = false;
        private float _shootSpeed = 0.5f;
        private Vector3 _shootDir = Vector3.zero;

        void Start()
        {
            _captureTargetManager.RegisterTarget(this);
        }

        void Update()
        {
            if(_isShooting)
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
            transform.position = playerTrans.position + Vector3.up * 1.0f;
            transform.SetParent(playerTrans);

            _captureTargetManager.UnregisterTarget(this);
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