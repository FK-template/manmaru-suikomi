using UnityEngine;

namespace Manmaru.Interaction
{
    public class StarBulletController : MonoBehaviour
    {
        // “à•”•Ï”F‚Í‚«‚¾‚µ—p
        private float _shootSpeed = 0.5f;
        private Vector3 _shootDir = Vector3.zero;

        void Update()
        {
            transform.position += _shootDir * _shootSpeed;
        }

        public void Initialize(Vector3 dir)
        {
            _shootDir = dir;
        }
    }
}