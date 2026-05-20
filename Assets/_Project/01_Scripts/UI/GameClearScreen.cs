using Manmaru.System;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// ゲームクリア画面の表示制御とボタン入力を管理するクラス
    /// </summary>
    /// <remarks>（※最初のイベント登録があるため、シーン開始時、本クラスをアタッチしたオブジェクトはアクティブ状態にしておくこと）</remarks>
    public class GameClearScreen : BaseScreen
    {
        [Header("ボタン設定")]
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _titleButton;

        [Header("シーン管理クラス")]
        [SerializeField] private SceneFlowController _sceneFlowController;

        protected override void RegisterEvents()
        {
            // イベント購読設定
            GameStateManager.Instance.OnGameClearState += ShowUI;

            // ボタンの役割設定
            _retryButton.onClick.AddListener(_sceneFlowController.ReloadCurrentScene);
            _nextButton.onClick.AddListener(()=> _sceneFlowController.LoadSceneByName(_sceneFlowController.NextSceneName));
            _titleButton.onClick.AddListener(() => _sceneFlowController.LoadSceneByName(_sceneFlowController.TitleSceneName));

            // 最初は非表示にしておく
            HideUI();
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnGameClearState -= ShowUI;
            }
        }
    }
}