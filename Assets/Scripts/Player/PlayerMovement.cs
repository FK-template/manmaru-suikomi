using UnityEngine;

namespace Manmaru.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("デバッグ用 - 機能オンオフ")]
        [SerializeField] private bool _canFunwari = true;
        [SerializeField] private bool _canSmallJump = true;
        [SerializeField] private bool _canMaxFallSpeed = true;

        [Header("ジャンプ設定")]
        [Tooltip("ジャンプの初速度")]
        [SerializeField] private float _jumpForce = 2.5f;
        [Tooltip("上昇中に離したときの減速率（小ジャンプ処理用）")]
        [SerializeField] private float _jumpCutoffMultiplier = 0.4f;

        [Header("落下設定")]
        [Tooltip("重力")]
        [SerializeField] private float _gravity = 1.0f;
        [Tooltip("最大落下速度")]
        [SerializeField] private float _maxFallSpeed = -2.5f;

        [Header("ふわっと滞空の設定")]
        [Tooltip("ふわっと滞空を始める/終わるときの、現在速度の絶対値")]
        [SerializeField] private float _brakeThreshold = 1.0f;
        [Tooltip("ふわっと滞空時の重力減衰率")]
        [SerializeField] private float _brakeGravityMultiplier = 0.5f;

        [Header("Raycast着地判定の設定")]
        [Tooltip("Rayの始点")]
        [SerializeField] private Transform _feetPos;
        [Tooltip("Rayの長さ")]
        [SerializeField] private float _rayLength = 0.1f;
        [Tooltip("着地判定を取るレイヤー")]
        [SerializeField] private LayerMask _groundLayer;

        // 内部変数
        private Vector3 _currentVelocity;
        private bool _isGrounded;

        private void Update()
        {
            _isGrounded = CheckGrounded();
            _currentVelocity.y = CalculateYVelocity();
            MoveToFinalPos();
        }

        /// <summary>
        /// Raycastを用いた着地判定の結果を、boolで返すメソッド
        /// </summary>
        private bool CheckGrounded()
        {
            // Rayを飛ばす（デバッグ用に可視化）
            Ray ray = new Ray(_feetPos.position, Vector3.down);
            Debug.DrawRay(_feetPos.position, ray.direction *  _rayLength, _isGrounded ? Color.green : Color.red);

            return Physics.Raycast(ray, _rayLength, _groundLayer);
        }

        /// <summary>
        /// Y軸方向の速度を計算して、floatで返すメソッド
        /// </summary>
        private float CalculateYVelocity()
        {
            // 押したらグンと加速
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                return _jumpForce;
            }

            // 上昇中に離したらギュッと減速（小ジャンプ）
            if (Input.GetButtonUp("Jump") && _currentVelocity.y > 0f && _canSmallJump)
            {
                return _currentVelocity.y *= _jumpCutoffMultiplier;
            }

            // 通常減速（落下）処理
            if (!_isGrounded)
            {
                float currentGravity = _gravity;

                // ふんわり滞空のために重力減衰
                if (Mathf.Abs(_currentVelocity.y) < _brakeThreshold && _canFunwari)
                {
                    currentGravity *= _brakeGravityMultiplier;
                }

                // 落下が速くなり過ぎないように補正
                if (_currentVelocity.y < _maxFallSpeed && _canMaxFallSpeed)
                {
                    return _maxFallSpeed;
                }

                // 実際の落下処理
                return _currentVelocity.y - currentGravity * Time.deltaTime;
            }
            else
            {
                return 0f;
            }
        }

        /// <summary>
        /// 実際の座標移動を行うメソッド
        /// </summary>
        private void MoveToFinalPos()
        {
            transform.position += _currentVelocity * Time.deltaTime;
        }
    }
}