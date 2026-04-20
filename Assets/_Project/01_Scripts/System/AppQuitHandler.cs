using UnityEngine;

namespace Manmaru.System
{
    /// <summary>
    /// ゲームの実行終了を専用に扱うシステムクラス
    /// </summary>
    public class AppQuitHandler : MonoBehaviour
    {
        /// <summary>
        /// ゲームの実行を終了するメソッド
        /// </summary>
        /// <remarks>エディターなら実行停止、実行ファイルならアプリケーションの停止</remarks>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }
}