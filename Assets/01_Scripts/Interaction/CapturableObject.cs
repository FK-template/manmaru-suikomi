using UnityEngine;

namespace Manmaru.Interaction
{
    public class CapturableObject : MonoBehaviour, ICapturable
    {
        [Header("すいこみオブジェクトの管理者")]
        [SerializeField] private CaptureTargetManager _captureTargetManager;

        void Start()
        {
            _captureTargetManager.RegisterTarget(this);
        }

        void Update()
        {

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
    }
}