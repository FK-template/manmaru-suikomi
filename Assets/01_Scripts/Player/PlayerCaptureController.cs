using Manmaru.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーのすいこみ・はきだし処理を制御するクラス
    /// </summary>
    public class PlayerCaptureController : MonoBehaviour
    {
        [Header("すいこみパラメータ設定")]
        [SerializeField] private float _captureMaxRange = 5.0f;
        [SerializeField] private float _captureCloseRange = 1.0f;
        [SerializeField] private float _captureAngleRange = 45f;

        [Header("入力設定")]
        [SerializeField] private InputActionReference _attackAction;

        [Header("はきだし設定")]
        [SerializeField] private StarBulletController _starBullet;
        [SerializeField] private Transform _spawnTrans;

        [Header("依存クラス設定")]
        [Tooltip("プレイヤー状態の可視化管理者")]
        [SerializeField] private PlayerVisualHandler _playerVisualController;
        [Tooltip("すいこみパーティクル管理者")]
        [SerializeField] private CaptureEffectHandler _captureEffectController;

        // 内部変数：ほおばり・はきだし用
        private bool _needToRelease = false;
        private bool _isMouthfulMode = false;
        private int _capturedCount = 0;

        // 内部変数：すいこみオブジェクトの管理者（イベント購読用）
        private CaptureTargetManager _captureTargetManager;

        void Start()
        {
            _captureTargetManager = CaptureTargetManager.Instance;

            // イベント設定
            _captureTargetManager.OnCaptureFinished += AddCapturedCount;
            _captureTargetManager.OnAllCapturesFinished += ReadyToShoot;
        }

        void Update()
        {
            // すいこみ・はきだし
            // すいこみ条件：ほおばり状態 かつ 入力ロック状態でないとき
            if (!_isMouthfulMode)
            {
                UpdateCaptureStatus();
            }
            else
            {
                UpdateShootStatus();
            }
        }

        /// <summary>
        /// 入力に応じた、すいこみ処理を行うメソッド
        /// </summary>
        private void UpdateCaptureStatus()
        {
            // 入力ロック
            if (_needToRelease)
            {
                // ボタンリリースされたら入力ロックを解除
                if (_attackAction.action.WasReleasedThisFrame())
                {
                    _needToRelease = false;
                    Debug.Log("入力ロック解除！すいこみ可能に");
                }
                // リリースされなくても終了
                return;
            }

            // Attackボタンを押している間ずっと、すいこみ判定
            if (_attackAction.action.IsPressed())
            {
                // グラフィック情報を更新
                _playerVisualController.ChangeToCapturing();
                _captureEffectController.PlayWind();

                // 角度を内積に変換
                float dotThreshold = Mathf.Cos(_captureAngleRange * Mathf.Deg2Rad);

                // すいこめるオブジェクトを検索し、存在したらすいこみ開始
                ICapturable target = _captureTargetManager.FindCaptureTarget(transform, _captureMaxRange, _captureCloseRange, dotThreshold);
                if (target == null) return;
                target.OnCapture(transform);

                Debug.Log($"すいこみ！：{target.GetTransform().gameObject.name}");
            }
            else if (_attackAction.action.WasReleasedThisFrame())
            {
                // グラフィック情報を更新
                _playerVisualController.ChangeToNormal();
                _captureEffectController.StopWind();

                // ボタンリリース必須状態を終了
                if (_needToRelease)
                {
                    _needToRelease = false;
                }
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
                Debug.Log($"はきだし！弾の強さ：Lv.{_capturedCount}");

                // グラフィック情報を更新
                _playerVisualController.ChangeToNormal();

                // 弾の生成と初期化
                StarBulletController bullet = Instantiate(_starBullet, _spawnTrans.position, Quaternion.LookRotation(transform.forward));
                bullet.Initialize(transform.forward, _capturedCount);

                // ほおばり状態の初期化
                _capturedCount = 0;
                _isMouthfulMode = false;

                // 入力ロック（ボタンリリースされるまですいこみ禁止）
                _needToRelease = true;
            }
        }

        /// <summary>
        /// すいこみ済みカウンターを増やすメソッド
        /// </summary>
        private void AddCapturedCount()
        {
            _capturedCount++;
            Debug.Log($"すいこみ完了！現在のストック：{_capturedCount}");
        }

        // すいこみ準備完了メソッド
        private void ReadyToShoot()
        {
            // グラフィック情報を更新
            _playerVisualController.ChangeToMouthful();
            _captureEffectController.StopWind();

            _isMouthfulMode = true;
            Debug.Log($"すいこみ完全完了！ほおばりモードへ");
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