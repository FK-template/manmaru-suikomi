using UnityEngine;

namespace Manmaru.Collision
{
    public class GroundFitter : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーのy座標を地面の高さに補正して、めり込みを防ぐメソッド
        /// </summary>
        public Vector3 FitToGround(Vector3 playerPos, float groundY, float feetPosY)
        {
            // 地面の表面 + Rayの長さ + (プレイヤーの原点 - プレイヤーの足元)
            float heightOffset = playerPos.y - feetPosY;
            float correctY = groundY + heightOffset;

            // 正しい位置でなければ、補正
            if (Mathf.Abs(playerPos.y - correctY) > 0.001f)
            {
                Debug.Log($"めり込み補正！ -> y={correctY}");
                return new Vector3(playerPos.x, correctY, playerPos.z);
            }
            else return playerPos;
        }
    }
}