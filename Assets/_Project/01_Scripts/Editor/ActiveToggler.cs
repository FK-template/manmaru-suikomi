#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Manmaru.Editor
{
    [InitializeOnLoad]
    public class ActiveToggler
    {
        /// <summary>
        /// 特定のキーでオブジェクトのアクティブ状態を切り替えるエディタ拡張クラス
        /// </summary>
        static ActiveToggler()
        {
            bool isCommaDown = false;

            EditorApplication.CallbackFunction function = () =>
            {
                Event e = Event.current;

                // キーを押された瞬間を検知
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Comma)
                {
                    // 「Alt」+「,」が入力されたらHierarchyで選択しているオブジェクトのアクティブ状態を反転させる
                    if (!isCommaDown && e.alt && Selection.activeGameObject != null)
                    {
                        isCommaDown = true;

                        foreach (var go in Selection.gameObjects)
                        {
                            Undo.RecordObject(go, go.name + ".activeSelf");
                            go.SetActive(!go.activeSelf);
                        }
                    }
                }

                if (e.type == EventType.KeyUp && e.keyCode == KeyCode.Comma)
                {
                    isCommaDown = false;
                }
            };

            // リフレクション（エディタのどこでも使えるように）
            FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            EditorApplication.CallbackFunction functions = (EditorApplication.CallbackFunction)info.GetValue(null);
            functions += function;
            info.SetValue(null, (object)functions);
        }
    }
}
#endif