using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// ‚Í‚«‚¾‚µ’e‚ÌˆÚ“®‚ğs‚¤ƒNƒ‰ƒX
    /// </summary>
    public class StarBulletMovement : MonoBehaviour
    {
        public void Move(Vector3 dir, float moveDist)
        {
            transform.position += dir * moveDist;
        }
    }
}