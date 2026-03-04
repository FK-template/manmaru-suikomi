using Manmaru.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manmaru.Player
{
    public class PlayerAction : MonoBehaviour
    {
        [Header("すいこみオブジェクトの管理者")]
        [SerializeField] private CaptureTargetManager _captureTargetManager;

        [Header("入力設定")]
        [SerializeField] private InputActionReference _attackAction;

        void Update()
        {
            if (_attackAction.action.WasPressedThisFrame())
            {
                _captureTargetManager.FindCaptureTarget(transform).OnCapture(transform);
            }
        }
    }
}