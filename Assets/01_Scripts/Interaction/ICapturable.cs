using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// すいこみ可能なオブジェクトのインターフェース
    /// </summary>
    public interface ICapturable
    {
        Transform GetTransform();
        void OnCapture(Transform playerTrans);
    }
}