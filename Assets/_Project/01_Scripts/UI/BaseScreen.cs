using Manmaru.System;
using UnityEngine;

namespace Manmaru.UI
{
    /// <summary>
    /// 画面全体の表示・非表示の切り替えと、初期化フローの共通基盤を提供する、各画面UIの基底クラス
    /// </summary>
    /// <remarks>
    /// <para>（※このクラス自体はアタッチせず、継承した子クラスをUIの種類ごとに作成する。）</para>
    /// <para>（※子クラスは「UIの表示切り替え」と「各ボタンのイベント配線」のみを担うこと。）</para>
    /// </remarks>
    public abstract class BaseScreen : MonoBehaviour
    {
        [Header("表示を切り替えるUIパネル")]
        [SerializeField] protected GameObject _rootPanel;

        /// <summary>
        /// 共通の初期化処理をまとめたスタート関数
        /// </summary>
        protected virtual void Start()
        {
            RegisterEvents();
        }

        /// <summary>
        /// UI表示タイミングやボタンのイベント登録を行う抽象メソッド
        /// </summary>
        /// <remarks>（※子クラスで必ず実装させる）</remarks>
        protected abstract void RegisterEvents();

        /// <summary>
        /// UIを表示する共通メソッド
        /// </summary>
        /// <remarks>（publicの理由：外部からボタン入力などを監視して画面表示を操作するため）</remarks>
        public virtual void ShowUI()
        {
            if (_rootPanel != null) _rootPanel.SetActive(true);
        }

        /// <summary>
        /// UIを非表示にする共通メソッド
        /// </summary>
        /// <remarks>（publicの理由：外部からボタン入力などを監視して画面表示を操作するため）</remarks>
        public virtual void HideUI()
        {
            if (_rootPanel != null) _rootPanel.SetActive(false);
        }
    }
}