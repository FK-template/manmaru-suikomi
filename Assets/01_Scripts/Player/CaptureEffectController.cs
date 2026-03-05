using UnityEngine;

namespace Manmaru.Player
{
    public class CaptureEffectController : MonoBehaviour
    {
        [Header("すいこみパーティクル")]
        [SerializeField] private ParticleSystem _windParticle;

        public void PlayWind()
        {
            if (_windParticle != null && !_windParticle.isPlaying)
                _windParticle.Play();
            else
                Debug.Log("すいこみパーティクル が再生できませんでした。");
        }

        public void StopWind()
        {
            if (_windParticle != null && _windParticle.isPlaying)
                _windParticle.Stop();
            else
                Debug.Log("すいこみパーティクル が停止できませんでした。");
        }
    }
}