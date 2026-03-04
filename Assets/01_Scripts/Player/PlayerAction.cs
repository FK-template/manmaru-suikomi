using Manmaru.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manmaru.Player
{
    public class PlayerAction : MonoBehaviour
    {
        [Header("すいこみパラメータ設定")]
        [SerializeField] private float _captureMaxRange = 5.0f;
        [SerializeField] private float _captureCloseRange = 1.0f;
        [SerializeField] private float _captureDotRange = 0.7f;

        [Header("入力設定")]
        [SerializeField] private InputActionReference _attackAction;

        [Header("すいこみオブジェクトの管理者")]
        [SerializeField] private CaptureTargetManager _captureTargetManager;

        void Update()
        {
            if (_attackAction.action.WasPressedThisFrame())
            {
                ICapturable target = _captureTargetManager.FindCaptureTarget(transform, _captureMaxRange, _captureCloseRange, _captureDotRange);
                target.OnCapture(transform);
            }
        }
    }
}