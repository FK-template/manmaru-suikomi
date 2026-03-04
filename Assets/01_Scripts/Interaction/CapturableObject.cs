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

        public Transform GetTransform()
        {
            return transform;
        }

        public void OnCapture(Transform playerTrans)
        {
            transform.position = playerTrans.position + Vector3.up * 1.0f;
            transform.SetParent(playerTrans);

            _captureTargetManager.UnregisterTarget(this);
        }
    }
}