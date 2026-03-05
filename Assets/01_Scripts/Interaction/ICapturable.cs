using UnityEngine;

namespace Manmaru.Interaction
{
    public interface ICapturable
    {
        Transform GetTransform();
        void OnCapture(Transform playerTrans);
    }
}