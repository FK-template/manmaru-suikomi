using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Manmaru.UI
{
    /// <summary>
    /// 最初に選択させておきたいUIにアタッチし、自身を設定するクラス
    /// </summary>
    public class FirstSelectedUISetter : MonoBehaviour
    {
        void OnEnable()
        {
            StartCoroutine(SetSelectedNextFrame());
        }

        /// <summary>
        /// 1f待ってからUI自動選択を実行するコルーチン
        /// </summary>
        /// <remarks>単にSetFirstSelectedUIを呼んだだけではアニメーションが再生しないケースがあり、実装した</remarks>
        private IEnumerator SetSelectedNextFrame()
        {
            yield return null;
            SetFirstSelectedUI(this.gameObject);
        }

        /// <summary>
        /// 最初に選択されているUIを設定するメソッド
        /// </summary>
        public void SetFirstSelectedUI(GameObject uiObj)
        {
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(uiObj);
            }
            else
            {
                Debug.LogWarning("【FirstSelectedUISetter】EventSystemが存在しません");
            }
        }
    }
}