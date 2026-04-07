using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// すいこみ可能なオブジェクトのインターフェース
    /// </summary>
    public interface ICapturable
    {
        int CaptureMass { get; }
        Transform GetTransform();
        Collider GetCollider();
        void OnCapture(Transform playerTrans);
    }
}