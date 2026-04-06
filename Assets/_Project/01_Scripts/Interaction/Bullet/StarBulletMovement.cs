using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// はきだし弾の移動を行うクラス
    /// </summary>
    public class StarBulletMovement : MonoBehaviour
    {
        public void Move(Vector3 dir, float moveDist, float rotAngle)
        {
            transform.position += dir * moveDist;
            transform.Rotate(0f, rotAngle, rotAngle);
        }
    }
}