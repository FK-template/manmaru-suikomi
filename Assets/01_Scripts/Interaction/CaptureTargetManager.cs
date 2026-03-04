using UnityEngine;
using System.Collections.Generic;

namespace Manmaru.Interaction
{
    public class CaptureTargetManager : MonoBehaviour
    {
        [Header("デバッグ用：すいこめるものリスト")]
        [SerializeField] private List<GameObject> _targetObjList = new List<GameObject>();
        
        // 内部変数：すいこめるものリスト
        private List<ICapturable> _targetList = new List<ICapturable>();

        /// <summary>
        /// 任意のすいこめるオブジェクトを、すいこめるものリストに追加するメソッド
        /// </summary>
        public void RegisterTarget(ICapturable argTarget)
        {
            if (_targetList.Contains(argTarget)) return;
            _targetList.Add(argTarget);
            _targetObjList.Add(argTarget.GetTransform().gameObject);
        }

        /// <summary>
        /// 任意のすいこめるオブジェクトを、すいこめるものリストから削除するメソッド
        /// </summary>
        public void UnregisterTarget(ICapturable argTarget)
        {
            if (!_targetList.Contains(argTarget)) return;
            _targetList.Remove(argTarget);
            _targetObjList.Remove(argTarget.GetTransform().gameObject);
        }

        /// <summary>
        /// すいこむ対象となるオブジェクトを検索して返すメソッド
        /// </summary>
        public ICapturable FindCaptureTarget(Transform playerTrans)
        {
            Debug.Log($"すいこみ！：{_targetList[0].GetTransform().gameObject.name}");
            return _targetList[0];
        }
    }
}