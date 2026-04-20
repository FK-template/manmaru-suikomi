using System;
using UnityEngine;
using UnityEngine.UI;

namespace Manmaru.UI
{
    /// <summary>
    /// 確認ダイアログの表示とボタン入力を管理する汎用クラス
    /// </summary>
    /// <remarks>
    /// このクラス自体は具体的な処理は持たず、メソッド経由で外部から渡されたコールバックを実行する
    /// </remarks>
    public class ConfirmDialogScreen : BaseScreen
    {
        [Header("ボタン設定")]
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        // 内部変数：ボタン入力で起動するメソッド入れ
        private Action _onYesClicked;

        protected override void RegisterEvents()
        {
            // ボタンの役割設定
            _yesButton.onClick.AddListener(() =>
            {
                _onYesClicked?.Invoke();
                HideUI();
            });
            _noButton.onClick.AddListener(() => HideUI());
        }

        /// <summary>
        /// Yes時の処理をセットして、ダイアログを表示するメソッド
        /// </summary>
        public void ShowDialog(Action onYes)
        {
            _onYesClicked = onYes;
            ShowUI();
        }
    }
}