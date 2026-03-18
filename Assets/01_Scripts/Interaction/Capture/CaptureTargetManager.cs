using UnityEngine;
using System.Collections.Generic;
using System;

namespace Manmaru.Interaction
{
    /// <summary>
    /// すいこみ候補オブジェクトの総合管理を行うシングルトンクラス
    /// </summary>
    public class CaptureTargetManager : MonoBehaviour
    {
        [Header("デバッグ用：すいこめるものリスト")]
        [SerializeField] private List<GameObject> _targetObjList = new List<GameObject>();

        // 内部変数：すいこめるものリスト
        private List<ICapturable> _targetList = new List<ICapturable>();

        // 内部変数：すいこみ中リスト
        private List<ICapturable> _capturingList = new List<ICapturable>();

        // すいこみ完了イベント
        public event Action OnCaptureFinished;
        public event Action OnAllCapturesFinished;

        // インスタンス設定
        public static CaptureTargetManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        /// <summary>
        /// 任意のすいこめるオブジェクトを、すいこめるものリストに追加するメソッド
        /// </summary>
        public void RegisterCapturableTarget(ICapturable argTarget)
        {
            if (_targetList.Contains(argTarget)) return;
            _targetList.Add(argTarget);
            _targetObjList.Add(argTarget.GetTransform().gameObject);
        }

        /// <summary>
        /// 任意のすいこめるオブジェクトを、すいこめるものリストから削除するメソッド
        /// </summary>
        public void UnregisterCapturableTarget(ICapturable argTarget)
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

        /// <summary>
        /// 任意のオブジェクトを、すいこみ中リストに追加するメソッド
        /// </summary>
        public void RegisterCapturingTarget(ICapturable argTarget)
        {
            if (_capturingList.Contains(argTarget)) return;
            _capturingList.Add(argTarget);
        }

        /// <summary>
        /// 自身のすいこみの完了を知らせ、リスト削除やイベント発動を行うメソッド
        /// </summary>
        public void NotifyCaptureCompleted(ICapturable argTarget)
        {
            // すいこみ中リストから自身を削除
            UnregisterCapturingTarget(argTarget);

            // すいこみ済みカウンターを増やすためのイベント発動
            OnCaptureFinished.Invoke();

            // すいこみ中リストが空になったら、完了イベント発動
            if (_capturingList.Count == 0)
            {
                OnAllCapturesFinished.Invoke();
            }
        }

        /// <summary>
        /// 任意のオブジェクトを、すいこみ中リストから削除するメソッド
        /// </summary>
        private void UnregisterCapturingTarget(ICapturable argTarget)
        {
            if (!_capturingList.Contains(argTarget)) return;
            _capturingList.Remove(argTarget);
        }
    }
}