using System;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// ステージセレクト画面の表示制御とボタン入力を管理するクラス
    /// </summary>
    public class StageSelectScreen : BaseScreen
    {
        /// <summary>
        /// ボタンと遷移先のシーンをセットで管理するためのデータクラス
        /// </summary>
        [Serializable]
        private class StageButtonData
        {
            [SerializeField] private Button _stageButton;
            [SerializeField] private string _targetSceneName;

            public Button StageButton => _stageButton;
            public string TargetSceneName => _targetSceneName;
        }

        [Header("ボタン設定")]
        [SerializeField] private Button _returnButton;
        [SerializeField] private StageButtonData[] _stageButtonsData;

        // 公開変数：ボタン入力を伝えるイベント
        public Action OnReturnClicked;

        protected override void RegisterEvents()
        {
            // ボタンの役割設定
            _returnButton.onClick.AddListener(() => OnReturnClicked?.Invoke());

            foreach (var data in _stageButtonsData)
            {
                data.StageButton.onClick.AddListener(() => _sceneFlowController.LoadSceneByName(data.TargetSceneName));
            }
        }
    }
}