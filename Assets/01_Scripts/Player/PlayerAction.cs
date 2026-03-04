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
        [SerializeField] private float _captureAngleRange = 45f;

        [Header("入力設定")]
        [SerializeField] private InputActionReference _attackAction;

        [Header("すいこみオブジェクトの管理者")]
        [SerializeField] private CaptureTargetManager _captureTargetManager;

        // 内部変数
        private ICapturable _currentCapturedTarget = null;

        void Update()
        {
            // すいこみ・はきだし
            // （null判定はメソッド内ですると、同フレームで両方の処理が行われる可能性がある）
            if (_currentCapturedTarget == null)
                UpdateCaptureStatus();
            else
                UpdateShootStatus();
        }

        /// <summary>
        /// 入力に応じた、すいこみ処理を行うメソッド
        /// </summary>
        private void UpdateCaptureStatus()
        {
            // Attackボタンを押している間ずっと、すいこみ判定
            if (_attackAction.action.IsPressed())
            {
                // 角度を内積に変換
                float dotThreshold = Mathf.Cos(_captureAngleRange * Mathf.Deg2Rad);

                ICapturable target = _captureTargetManager.FindCaptureTarget(transform, _captureMaxRange, _captureCloseRange, dotThreshold);
                if (target == null) return;

                target.OnCapture(transform);
                _currentCapturedTarget = target;

                Debug.Log($"すいこみ！：{target.GetTransform().gameObject.name}");
            }
        }

        /// <summary>
        /// 入力に応じた、はきだし処理を行うメソッド
        /// </summary>
        private void UpdateShootStatus()
        {
            // Attackボタンを押した瞬間に、はきだし処理を開始
            if (_attackAction.action.WasPressedThisFrame())
            {
                Debug.Log($"はきだし！：{_currentCapturedTarget.GetTransform().gameObject.name}");

                _currentCapturedTarget.OnShoot(transform.forward);
                _currentCapturedTarget = null;
            }
        }

        // ----- 以下、Gemini3 Pro より出力 -----
        private void OnDrawGizmosSelected()
        {
            // 全て水色に統一
            Gizmos.color = Color.cyan;

            Vector3 forwardLineMax = transform.forward * _captureMaxRange;
            Vector3 forwardLineClose = transform.forward * _captureCloseRange;

            // ==========================================
            // 1. 通常のすいこみ扇形範囲（外側の扇）
            // ==========================================
            Vector3 rightEdge = Quaternion.Euler(0, _captureAngleRange, 0) * forwardLineMax;
            Vector3 leftEdge = Quaternion.Euler(0, -_captureAngleRange, 0) * forwardLineMax;
            Gizmos.DrawRay(transform.position, rightEdge);
            Gizmos.DrawRay(transform.position, leftEdge);

            int segments = 20; // 弧の滑らかさ
            float angleStep = (_captureAngleRange * 2) / segments;
            Vector3 previousPoint = transform.position + leftEdge;

            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = -_captureAngleRange + (angleStep * i);
                Vector3 currentDir = Quaternion.Euler(0, currentAngle, 0) * forwardLineMax;
                Vector3 currentPoint = transform.position + currentDir;
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }

            // ==========================================
            // 2. 至近距離の半円（内積0以上 ＝ 前方180度）
            // ==========================================
            float closeAngleRange = 90f; // -90度〜90度（半円）

            // 半円の左右の直線（真横への線）
            Vector3 closeRightEdge = Quaternion.Euler(0, closeAngleRange, 0) * forwardLineClose;
            Vector3 closeLeftEdge = Quaternion.Euler(0, -closeAngleRange, 0) * forwardLineClose;
            Gizmos.DrawRay(transform.position, closeRightEdge);
            Gizmos.DrawRay(transform.position, closeLeftEdge);

            float closeAngleStep = (closeAngleRange * 2) / segments;
            Vector3 closePrevPoint = transform.position + closeLeftEdge;

            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = -closeAngleRange + (closeAngleStep * i);
                Vector3 currentDir = Quaternion.Euler(0, currentAngle, 0) * forwardLineClose;
                Vector3 currentPoint = transform.position + currentDir;
                Gizmos.DrawLine(closePrevPoint, currentPoint);
                closePrevPoint = currentPoint;
            }
        }
        // ----- 以上 -----
    }
}