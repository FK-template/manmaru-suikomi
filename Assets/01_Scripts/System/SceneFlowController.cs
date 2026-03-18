using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manmaru.System
{
    public class SceneFlowController : MonoBehaviour
    {
        [Header("遷移先シーン名設定")]
        [SerializeField] private string _nextSceneName;
        [SerializeField] private string _titleSceneName;

        /// <summary>
        /// 現在のシーンを再ロードするメソッド
        /// </summary>
        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// 次のステージシーンをロードするメソッド
        /// </summary>
        public void MoveToNextScene()
        {
            SceneManager.LoadScene(_nextSceneName);
        }

        /// <summary>
        /// タイトルシーンをロードするメソッド
        /// </summary>
        public void MoveToTitleScene()
        {
            SceneManager.LoadScene(_titleSceneName);
        }
    }
}