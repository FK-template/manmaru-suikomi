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
        [Header("依存クラス設定")]
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private PlayerHealthController _healthController;

        void Start()
        {
            // イベント設定（引数渡し付き）
            _healthController.OnDamaged += UpdateHealthBar;
        }

        /// <summary>
        /// 最大HPと現在のHPを、HPバーUIに反映するメソッド
        /// </summary>
        private void UpdateHealthBar(float maxHP, float curHP)
        {
            _hpSlider.maxValue = maxHP;
            _hpSlider.value = curHP;
        }
    }
}