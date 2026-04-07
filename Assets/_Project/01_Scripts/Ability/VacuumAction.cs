using Manmaru.Interaction;
using UnityEngine;

namespace Manmaru.Ability
{
    /// <summary>
    /// すいこみ処理と範囲の可視化を行うクラス
    /// </summary>
    public class VacuumAction : MonoBehaviour
    {
        [Header("すいこみパラメータ設定")]
        [SerializeField] private float _captureMaxRange = 3.0f;
        [SerializeField] private float _captureCloseRange = 1.0f;
        [SerializeField] private float _captureAngleRange = 30.0f;

        // 依存クラス
        private CaptureTargetManager _captureTargetManager;

        void Start()
        {
            _captureTargetManager = CaptureTargetManager.Instance;
        }

        /// <summary>
        /// すいこめるオブジェクトを検索し、結果をboolで返すメソッド
        /// </summary>
        public bool IsCaptureTargetDetected()
        {
            // 角度を内積に変換
            float dotThreshold = Mathf.Cos(_captureAngleRange * Mathf.Deg2Rad);

            // すいこめるオブジェクトを検索
            ICapturable target = _captureTargetManager.FindCaptureTarget(transform, _captureMaxRange, _captureCloseRange, dotThreshold);
            if (target == null) return false;

            // 検索が成功したら、ひきよせを開始させて成功報告
            target.OnCapture(transform);
            return true;
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