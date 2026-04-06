using Manmaru.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// プレイヤーのHPに関するUI処理を制御するクラス
    /// </summary>
    public class PlayerHealthUI : MonoBehaviour
    {
        [Header("反映するUI")]
        [SerializeField] private Slider _hpSlider;

        [Header("依存クラス設定")]
        [SerializeField] private PlayerHealthController _healthController;

        void Start()
        {
            // イベント購読設定（引数渡し付き）
            _healthController.OnHealthChanged += UpdateHealthBar;
        }

        /// <summary>
        /// 最大HPと現在のHPを、HPバーUIに反映するメソッド
        /// </summary>
        private void UpdateHealthBar(float maxHP, float curHP)
        {
            _hpSlider.maxValue = maxHP;
            _hpSlider.value = curHP;
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (_healthController != null)
            {
                _healthController.OnHealthChanged -= UpdateHealthBar;
            }
        }
    }
}