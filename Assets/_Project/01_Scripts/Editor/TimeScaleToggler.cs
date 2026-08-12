#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;

[InitializeOnLoad]
public class TimeScaleToggler
{
    /// <summary>
    /// 特定のキーでゲームプレイ中の時間を倍速にするエディタ拡張クラス
    /// </summary>
    static TimeScaleToggler()
    {
        bool isFastForwarding = false;
        float originalTimeScale = 1.0f;

        float accelTimeScale = 2.0f;
        KeyCode accelKey = KeyCode.RightShift;

        EditorApplication.CallbackFunction function = () =>
        {
            // プレイモード中以外は処理を中断
            if (!Application.isPlaying) return;

            Event e = Event.current;

            // キーが押された瞬間を検知
            if (e.type == EventType.KeyDown && e.keyCode == accelKey)
            {
                if (!isFastForwarding)
                {
                    isFastForwarding = true;

                    // 現在の速度を記憶してから、2倍速に変更
                    originalTimeScale = Time.timeScale;
                    Time.timeScale = accelTimeScale;
                    Debug.Log($"<color=yellow><b>[TimeScale] 倍速モード ON ({accelTimeScale}x)</b></color>");

                    e.Use();
                }
            }

            // キーが離されたら元の速度に戻す
            if (e.type == EventType.KeyUp && e.keyCode == accelKey)
            {
                if (isFastForwarding)
                {
                    isFastForwarding = false;
                    Time.timeScale = originalTimeScale;
                    Debug.Log($"<color=cyan><b>[TimeScale] 倍速モード OFF (通常速度)</b></color>");
                }
            }
        };

        // リフレクションでグローバルイベントに登録
        FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
        EditorApplication.CallbackFunction functions = (EditorApplication.CallbackFunction)info.GetValue(null);
        functions += function;
        info.SetValue(null, (object)functions);
    }
}
#endif