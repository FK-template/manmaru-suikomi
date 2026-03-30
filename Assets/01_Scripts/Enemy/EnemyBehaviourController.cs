using Manmaru.Enemy.States;
using UnityEngine;

namespace Manmaru.Enemy
{
    /// <summary>
    /// 敵の状態遷移と振る舞いを管理する基底クラス
    /// </summary>
    /// <remarks>（※このクラス自体はアタッチせず、敵の種別ごとに継承したクラスを作成し、状態遷移の順序を設定する）</remarks>
    public abstract class EnemyBehaviourController : MonoBehaviour
    {
        [Header("敵種別パラメータデータ")]
        [SerializeField] protected EnemyDataSO _data;

        // 内部変数：現在の状態クラス
        protected IEnemyStateLogic _currentState;

        // 公開変数：他クラス参照用
        public Transform PlayerTransform { get; private set; }
        public EnemyDataSO Data => _data;
        public EnemyVisionSensor VisionSensor { get; private set; }

        protected virtual void Start()
        {
            // センサークラス生成
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            PlayerTransform = playerObj != null ? playerObj.transform : null;
            VisionSensor = new EnemyVisionSensor(this.transform, PlayerTransform, _data);
        }

        /// <summary>
        /// 現在の状態クラスのUpdate処理を呼び、行動状態に応じた期待される速度を返すメソッド
        /// </summary>
        /// <remarks>（※状態が設定されていない場合、ゼロを返す保険付き）</remarks>
        public virtual Vector3 GetDesiredVelocity()
        {
            return _currentState?.UpdateState() ?? Vector3.zero;
        }

        /// <summary>
        /// 現在の状態クラスを、任意の状態クラスに切り替えるメソッド
        /// </summary>
        protected void ChangeState(IEnemyStateLogic newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }
    }
}