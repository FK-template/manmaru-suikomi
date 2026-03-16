using Manmaru.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    // UIコンポーネントとアクション監視
    // UpdateHealthBarで、現在HPと最大HPを取得し、maxValueとvalueを指定
    // （OnDestroyにアクションの耳塞ぎを入れる。これいる？）

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
        /// <param name="maxHP"></param>
        /// <param name="curHP"></param>
        private void UpdateHealthBar(float maxHP, float curHP)
        {
            _hpSlider.maxValue = maxHP;
            _hpSlider.value = curHP;
        }
    }
}