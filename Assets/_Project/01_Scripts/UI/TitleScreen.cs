using System;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// タイトル画面の表示制御とボタン入力を管理するクラス
    /// </summary>
    public class TitleScreen : BaseScreen
    {
        [Header("ボタン設定")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _quitButton;

        // 公開変数：ボタン入力を伝えるイベント
        public Action OnStartClicked;
        public Action OnQuitClicked;

        protected override void RegisterEvents()
        {
            // ボタンの役割設定
            _startButton.onClick.AddListener(() => OnStartClicked?.Invoke());
            _quitButton.onClick.AddListener(() => OnQuitClicked?.Invoke());
        }
    }
}