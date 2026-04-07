using System;
using Manmaru.Interaction;
using UnityEngine;

namespace Manmaru.Ability
{
    /// <summary>
    /// ほおばりストックカウンターを管理するクラス
    /// </summary>
    /// <remarks>（※Serializable運用なので、コンストラクタは未作成）</remarks>
    [Serializable]
    public class MouthfulStock
    {
        // 内部変数
        [Header("デバッグ用")]
        [SerializeField] private int _capturedCount = 0;
        [SerializeField] private int _capturedCountLimit;

        // プロパティ
        public int CapturedCount => _capturedCount;

        public void SetCountLimit(int countLimit)
        {
            _capturedCountLimit = countLimit;
        }

        /// <summary>
        /// 上限を超えない範囲ですいこみ済みカウンターを増やすメソッド
        /// </summary>
        public void AddCapturedCount(ICapturable captureTarget)
        {
            _capturedCount = Mathf.Min(_capturedCount + captureTarget.CaptureMass, _capturedCountLimit);
        }

        /// <summary>
        /// すいこみ済みカウンターをリセットするメソッド
        /// </summary>
        public void ResetCapturedCount()
        {
            _capturedCount = 0;
        }
    }
}