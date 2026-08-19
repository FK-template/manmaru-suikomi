using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Manmaru.UI
{
    /// <summary>
    /// UIの選択に関するアニメーションを制御するクラス
    /// </summary>
    public class ButtonSelectFeedback : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
    {
        [Header("演出設定（サイズ）")]
        [SerializeField] private float _selectAnimationDuration = 0.2f;
        [SerializeField] private float _deselectAnimationDuration = 0.01f;
        [SerializeField] private Vector3 _hoverScale = new Vector3(1.1f, 1.1f, 1.1f);

        [Header("演出設定（色）")]
        [SerializeField] private Color _hoverButtonColor;
        [SerializeField] private Color _hoverTextColor;

        // 元の状態を記憶
        private Vector3 _defaultScale;
        private Color _defaultButtonColor;
        private Color[] _defaultTextColors;

        // 操作対象コンポーネント
        private Image _buttonImage;
        private TextMeshProUGUI[] _buttonTexts;

        private void Awake()
        {
            _buttonImage = GetComponent<Image>();
            _buttonTexts = GetComponentsInChildren<TextMeshProUGUI>();

            // 元の状態を記憶
            _defaultScale = transform.localScale;
            _defaultButtonColor = _buttonImage.color;

            _defaultTextColors = new Color[_buttonTexts.Length];
            for (int i = 0; i < _buttonTexts.Length; i++)
            {
                _defaultTextColors[i] = _buttonTexts[i].color;
            }
        }

        // マウスが乗ったら選択（コントローラー選択とのケンカ防止）
        public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        // 選択アニメーション
        public void OnSelect(BaseEventData eventData)
        {
            // スケールアップ
            transform.DOKill();
            transform.DOScale(_hoverScale, _selectAnimationDuration).SetEase(Ease.OutBack).SetUpdate(true).SetLink(gameObject); ;

            // ボタン色変化
            _buttonImage.DOKill();
            _buttonImage.DOColor(_hoverButtonColor, _selectAnimationDuration).SetUpdate(true).SetLink(gameObject); ;

            // テキスト色変化
            foreach (var text in _buttonTexts)
            {
                text.DOKill();
                text.DOColor(_hoverTextColor, _selectAnimationDuration).SetUpdate(true).SetLink(gameObject);
            }
        }

        // 選択解除アニメーション
        public void OnDeselect(BaseEventData eventData)
        {
            // スケール戻し
            transform.DOKill();
            transform.DOScale(_defaultScale, _deselectAnimationDuration).SetEase(Ease.OutQuad).SetUpdate(true).SetLink(gameObject); ;

            // ボタン色戻し
            _buttonImage.DOKill();
            _buttonImage.DOColor(_defaultButtonColor, _deselectAnimationDuration).SetUpdate(true).SetLink(gameObject); ;

            // テキスト色戻し
            for (int i = 0; i < _buttonTexts.Length; i++)
            {
                _buttonTexts[i].DOKill();
                _buttonTexts[i].DOColor(_defaultTextColors[i], _deselectAnimationDuration).SetUpdate(true).SetLink(gameObject);
            }
        }
    }
}