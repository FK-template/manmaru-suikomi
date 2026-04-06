using UnityEngine;

namespace Manmaru.VFX
{
    /// <summary>
    /// すいこみエフェクトを制御するクラス
    /// </summary>
    public class VacuumEffectHandler : MonoBehaviour
    {
        [Header("すいこみパーティクル")]
        [SerializeField] private ParticleSystem _windParticle;

        public void PlayWind()
        {
            if (_windParticle != null && !_windParticle.isPlaying)
                _windParticle.Play();
        }

        public void StopWind()
        {
            if (_windParticle != null && _windParticle.isPlaying)
                _windParticle.Stop();
        }
    }
}