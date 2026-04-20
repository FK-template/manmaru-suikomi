using Manmaru.System;
using UnityEngine;

namespace Manmaru.UI
{
    /// <summary>
    /// タイトルシーン全体の進行とUI制御を統括するクラス
    /// </summary>
    public class TitleScenePresenter : MonoBehaviour
    {
        [Header("タイトルシーンで管理する画面")]
        [SerializeField] private TitleScreen _titleScreen;
        [SerializeField] private StageSelectScreen _stageSelectScreen;
        [SerializeField] private ConfirmDialogScreen _confirmScreen;

        [Header("ゲーム終了システム")]
        [SerializeField] private AppQuitHandler _appQuitHandler;

        void Start()
        {
            // 各画面のボタンの役割設定
            _titleScreen.OnStartClicked += MoveToStageSelect;
            _stageSelectScreen.OnReturnClicked += MoveToTitle;
            _titleScreen.OnQuitClicked += () => _confirmScreen.ShowDialog(_appQuitHandler.QuitGame);
        }

        /// <summary>
        /// ステージセレクト画面用にUIの表示切替を行うメソッド
        /// </summary>
        private void MoveToStageSelect()
        {
            _titleScreen.HideUI();
            _stageSelectScreen.ShowUI();
        }

        /// <summary>
        /// タイトル画面用にUIの表示切替を行うメソッド
        /// </summary>
        private void MoveToTitle()
        {
            _stageSelectScreen.HideUI();
            _titleScreen.ShowUI();
        }
    }
}