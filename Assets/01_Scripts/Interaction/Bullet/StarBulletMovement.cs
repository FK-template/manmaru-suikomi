using UnityEngine;

namespace Manmaru.Interaction
{
    public class StarBulletMovement : MonoBehaviour
    {
        public void Move(Vector3 dir, float moveDist)
        {
            transform.position += dir * moveDist;
        }
    }
}