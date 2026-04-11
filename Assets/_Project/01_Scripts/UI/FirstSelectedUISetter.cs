using UnityEngine;
using UnityEngine.EventSystems;

namespace Manmaru.UI
{
    /// <summary>
    /// 最初に選択させておきたいUIにアタッチし、自身を設定するクラス
    /// </summary>
    public class FirstSelectedUISetter : MonoBehaviour
    {
        private void OnEnabled()
        {
            SetFirstSelectedUI(this.gameObject);
        }

        /// <summary>
        /// 最初に選択されているUIを設定するメソッド
        /// </summary>
        private void SetFirstSelectedUI(GameObject uiObj)
        {
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(uiObj);
            }
            else
            {
                Debug.LogWarning("【FirstSelectedUISetter】EventSystemが存在しません");
            }
        }
    }
}