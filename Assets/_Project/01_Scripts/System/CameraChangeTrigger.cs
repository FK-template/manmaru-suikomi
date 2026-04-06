using UnityEngine;
using Unity.Cinemachine;

namespace Manmaru.System
{
    /// <summary>
    /// カメラを切り替えるためのエリア干渉判定を行うクラス
    /// </summary>
    public class CameraChangeTrigger : MonoBehaviour
    {
        [Header("この区間で起動するカメラ")]
        [SerializeField] private CinemachineCamera _targetCamera;

        [Header("カメラ遷移判定を取るレイヤー")]
        [SerializeField] private LayerMask _playerLayer;

        [Header("起動時の優先度")]
        [SerializeField] private int _activePriority = 100;

        // 内部変数：元々の優先度
        private int _defaultPriority;

        void Start()
        {
            _defaultPriority = _targetCamera.Priority;
        }

        private void OnTriggerEnter(Collider other)
        {          
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
                ChangeCameraPriority(_activePriority);            
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
                ChangeCameraPriority(_defaultPriority);
        }

        /// <summary>
        /// 担当カメラの優先度を変更するメソッド
        /// </summary>
        private void ChangeCameraPriority(int targetPriority)
        {
            _targetCamera.Priority = targetPriority;
        }
    }
}