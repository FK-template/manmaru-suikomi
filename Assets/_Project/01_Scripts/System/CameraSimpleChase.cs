using UnityEngine;

namespace Manmaru.System
{
    public class CameraSimpleChase : MonoBehaviour
    {
        [SerializeField] private Transform _chaseTarget;

        private Vector3 _startCameraPos;
        private Vector3 _startTargetPos;

        void Awake()
        {
            _startCameraPos = transform.position;
            _startTargetPos = _chaseTarget.position;
        }

        void Update()
        {
            transform.position = _startCameraPos + (_chaseTarget.position - _startTargetPos);
        }
    }
}