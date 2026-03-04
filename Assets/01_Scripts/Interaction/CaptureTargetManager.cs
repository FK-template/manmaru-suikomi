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
        public ICapturable FindCaptureTarget(Transform playerTrans, float maxRange, float closeRange, float dotRange)
        {
            // 判定用の距離（最大距離・至近距離しきい値）
            float maxDistSqr = maxRange * maxRange;
            float closeDistSqr = closeRange * closeRange;

            // 最も近いすいこみ対象の情報
            ICapturable closestTarget = null;
            float minDistSqr = float.MaxValue;

            foreach (ICapturable target in _targetList)
            {
                Transform targetTrans = target.GetTransform();

                // プレイヤーとの距離を判定
                Vector3 dirToTarget = targetTrans.position - playerTrans.position;
                float distSqr = dirToTarget.sqrMagnitude;
                if (distSqr > maxDistSqr) continue;

                // プレイヤーとの角度（内積）を判定
                float dot = Vector3.Dot(playerTrans.forward, dirToTarget);
                if (distSqr < closeDistSqr)
                {
                    // 至近距離なら、真横でも許容
                    if (dot < 0f) continue;
                }
                else
                {
                    // 通常距離なら、前方しか許容しない
                    if (dot < dotRange) continue;
                }

                // 最も近い対象を記録
                if (distSqr < minDistSqr)
                {
                    closestTarget = target;
                    minDistSqr = distSqr;
                }
            }
            return closestTarget;
        }
    }
}