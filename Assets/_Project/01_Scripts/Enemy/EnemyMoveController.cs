using Manmaru.Collision;
using Manmaru.Movement;
using UnityEngine;

namespace Manmaru.Enemy
{
    /// <summary>
    /// 敵の移動処理全般を制御するクラス
    /// </summary>
    public class EnemyMoveController : MonoBehaviour
    {
        [Header("地形判定を取るレイヤー")]
        [SerializeField] private LayerMask _groundLayer;

        [Header("依存クラス設定")]
        [SerializeField] private EnemyBehaviourController _behaviourController;
        [SerializeField] private SingleRayGroundChecker _groundChecker;
        [SerializeField] private GroundFitter _groundFitter;
        [SerializeField] private GravityCalculator _gravityCalculator;
        [SerializeField] private WallChecker _wallChecker;
        [SerializeField] private WallFitter _wallFitter;

        // 内部変数：現在の速度
        private Vector3 _currentVelocity;

        void Start()
        {
            _gravityCalculator.SetParams(_behaviourController.Data);
        }

        void Update()
        {
            // 着地判定の保存
            bool isGrounded = _groundChecker.CheckGrounded(_currentVelocity.y, out float groundY, out Vector3 groundNormal, _groundLayer);

            // 理想の速度の取得
            Vector3 desiredVel = _behaviourController.GetDesiredVelocity();
            _currentVelocity = new Vector3(desiredVel.x, _currentVelocity.y, desiredVel.z);

            // 移動方向に合わせた回転処理（上下速度もそのまま考慮）
            transform.rotation = CalculateRotation(_currentVelocity, transform.rotation, groundNormal);

            // 重力処理
            _currentVelocity.y = _gravityCalculator.CalculateGravity(_currentVelocity.y, isGrounded);

            // 移動・補正処理
            ApplyWallSliding();
            MoveToFinalPos();

            // 移動後の地面情報を再取得して位置補正
            isGrounded = _groundChecker.CheckGrounded(_currentVelocity.y, out groundY, out groundNormal, _groundLayer);
            ApplyGroundFitting(groundY, isGrounded);
        }

        // ----- 以下、Update処理の分割メソッド群 -----

        /// <summary>
        /// 移動方向と地面の傾きに基づいて次のフレームの向きを計算し、Quaternionで返すメソッド
        /// </summary>
        /// <remarks>（※PlayerRotationから、ほぼ丸々借りて来てます）</remarks>
        public Quaternion CalculateRotation(Vector3 moveDir, Quaternion curRot, Vector3 groundNormal)
        {
            // 速度が無いなら、現在の「前」の向きを維持
            Vector3 forwardDir = moveDir.sqrMagnitude > 0.01f ? moveDir : curRot * Vector3.forward;

            // 前向きベクトルを地面に投影し、地面の傾きを反映
            float normalVelToPlane = Vector3.Dot(forwardDir, groundNormal);
            forwardDir -= normalVelToPlane * groundNormal;

            // 目標の向き＝「前方:投影したベクトル」＋「上方:地面の法線」
            Quaternion targetRot = Quaternion.LookRotation(forwardDir, groundNormal);

            // 滑らかに回転したあとの角度を返す
            return Quaternion.RotateTowards(curRot, targetRot, _behaviourController.Data.RotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 現在の速度に壁滑りを適用するメソッド
        /// </summary>
        private void ApplyWallSliding()
        {
            _currentVelocity = _wallChecker.CalculateWallSliding(_currentVelocity, _groundLayer);
            transform.position = _wallFitter.FixWallPenetration(transform.position, _behaviourController.Data.BodyRadius, _groundLayer);
        }

        /// <summary>
        /// 実際の座標移動を行うメソッド
        /// </summary>
        private void MoveToFinalPos()
        {
            transform.position += _currentVelocity * Time.deltaTime;
        }

        /// <summary>
        /// めり込み補正を適用するメソッド
        /// </summary>
        private void ApplyGroundFitting(float groundY, bool isGrounded)
        {
            // 空中 or 上昇中 なら終了
            if (!isGrounded || _currentVelocity.y > 0f) return;

            // 着地時のめり込み補正
            transform.position = _groundFitter.FitToGround(transform.position, groundY, _groundChecker.FeetPosY);
        }

        // -----
    }
}