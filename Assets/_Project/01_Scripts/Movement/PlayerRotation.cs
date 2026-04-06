using Manmaru.Player;
using UnityEngine;

namespace Manmaru.Movement
{
    /// <summary>
    /// プレイヤーの回転制御を行うクラス
    /// </summary>
    public class PlayerRotation : MonoBehaviour
    {
        // 内部変数：パラメータ
        private PlayerMoveParametersSO _currentParams;

        /// <summary>
        /// 新しくパラメータを設定するメソッド
        /// </summary>
        public void SetParams(PlayerMoveParametersSO newParams)
        {
            _currentParams = newParams;
        }

        /// <summary>
        /// 入力と地面の傾きに基づいて次のフレームの向きを計算し、Quaternionで返すメソッド
        /// </summary>
        public Quaternion CalculateRotation(Vector3 inputDir, Quaternion curRot, Vector3 groundNormal)
        {
            // 入力が無いなら、現在の「前」の向きを維持
            Vector3 forwardDir = inputDir.sqrMagnitude > 0.01f ? inputDir : curRot * Vector3.forward;

            // 前向きベクトルを地面に投影し、地面の傾きを反映
            float normalVelToPlane = Vector3.Dot(forwardDir, groundNormal);
            forwardDir -= normalVelToPlane * groundNormal;

            // 目標の向き＝「前方:投影したベクトル」＋「上方:地面の法線」
            Quaternion targetRot = Quaternion.LookRotation(forwardDir, groundNormal);

            // 滑らかに回転したあとの角度を返す
            return Quaternion.RotateTowards(curRot, targetRot, _currentParams.RotationSpeed * Time.deltaTime);
        }
    }
}