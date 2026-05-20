using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manmaru.System
{
    /// <summary>
    /// シーン遷移処理を制御するクラス
    /// </summary>
    public class SceneFlowController : MonoBehaviour
    {
        [Header("遷移先シーン名設定")]
        [SerializeField] private string _nextSceneName;
        [SerializeField] private string _titleSceneName;

        // 内部変数
        private bool _isLoading = false;

        // プロパティ
        public string NextSceneName => _nextSceneName;
        public string TitleSceneName => _titleSceneName;

        /// <summary>
        /// 指定された名前のシーンをロードするメソッド
        /// </summary>
        public void LoadSceneByName(string sceneName)
        {
            // 重複ロード対策
            if(_isLoading) return;
            _isLoading = true;

            // ロード処理
            SceneManager.LoadScene(sceneName);
            _isLoading = false;
        }

        /// <summary>
        /// 現在のシーンを再ロードするメソッド
        /// </summary>
        public void ReloadCurrentScene()
        {
            LoadSceneByName(SceneManager.GetActiveScene().name);
        }
    }
}