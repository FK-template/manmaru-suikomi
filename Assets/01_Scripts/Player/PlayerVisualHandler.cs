using UnityEngine;
using System.Collections.Generic;

namespace Manmaru.Player
{
    /// <summary>
    /// プレイヤーの見た目の変化を制御するクラス
    /// </summary>
    public class PlayerVisualHandler : MonoBehaviour
    {
        [Header("対象レンダラー設定")]
        [SerializeField] private List<Renderer> _renderersList = new List<Renderer>();

        [Header("状態ごとの色")]
        [SerializeField] private Color _normalColor = Color.magenta;
        [SerializeField] private Color _vacuumingColor = Color.red;
        [SerializeField] private Color _mouthfulColor = Color.yellow;

        [Header("状態ごとのスケール")]
        [SerializeField] private Vector3 _baseScale = Vector3.one;
        [SerializeField] private Vector3 _fatScale = new Vector3(1.2f, 1.0f, 1.0f);

        [Header("点滅設定")]
        [SerializeField] private float _flashSpeed = 10.0f;
        [SerializeField] private float _flashColorMaxRatio = 0.5f;
        [SerializeField] private Color _flashLerpColor = Color.white;

        [Header("依存クラス設定")]
        [SerializeField] private PlayerHealthController _healthController;

        // 内部変数：点滅用
        private bool _isFlashing = false;
        private Color _baseColor;

        void Start()
        {
            ChangeToNormal();
            _baseScale = transform.localScale;

            // イベント購読設定
            _healthController.OnNoDamageStarted += StartFlashing;
            _healthController.OnNoDamageFinished += FinishFlashing;
        }

        void Update()
        {
            UpdateFlashing();
        }

        /// <summary>
        /// 通常時の見た目に変更するメソッド
        /// </summary>
        public void ChangeToNormal()
        {
            ApplyColor(_normalColor);
            ApplyScale(_baseScale);
            _baseColor = _renderersList[0].material.color;
        }

        /// <summary>
        /// すいこみ中の見た目に変更するメソッド
        /// </summary>
        public void ChangeToCapturing()
        {
            ApplyColor(_vacuumingColor);
            ApplyScale(_baseScale);
            _baseColor = _renderersList[0].material.color;
        }

        /// <summary>
        /// ほおばり中の見た目に変更するメソッド
        /// </summary>
        public void ChangeToMouthful()
        {
            ApplyColor(_mouthfulColor);
            ApplyScale(_fatScale);
            _baseColor = _renderersList[0].material.color;
        }

        /// <summary>
        /// 白点滅を開始するための初期設定メソッド
        /// </summary>
        private void StartFlashing()
        {
            _isFlashing = true;
            _baseColor = _renderersList[0].material.color;
        }

        /// <summary>
        /// 毎フレーム呼び出し、点滅フラグがオンなら白点滅処理を更新するメソッド
        /// </summary>
        private void UpdateFlashing()
        {
            if (!_isFlashing)
            {
                return;
            }

            // 点滅処理（Time.timeで経過時間を参照 -> 開始地点は選べてない）
            float blendRatio = Mathf.PingPong(Time.time * _flashSpeed, _flashColorMaxRatio);
            ApplyColor(Color.Lerp(_baseColor, _flashLerpColor, blendRatio));
        }

        /// <summary>
        /// 白点滅を停止するときの再設定メソッド
        /// </summary>
        private void FinishFlashing()
        {
            _isFlashing = false;
            ApplyColor(_baseColor);
        }

        /// <summary>
        /// 体色を変更するメソッド
        /// </summary>
        private void ApplyColor(Color argColor)
        {
            foreach (Renderer r in _renderersList)
            {
                r.material.color = argColor;
            }
        }

        /// <summary>
        /// 体のスケールを変更するメソッド
        /// </summary>
        private void ApplyScale(Vector3 argScale)
        {
            transform.localScale = argScale;
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (_healthController != null)
            {
                _healthController.OnNoDamageStarted -= StartFlashing;
                _healthController.OnNoDamageFinished -= FinishFlashing;
            }
        }
    }
}