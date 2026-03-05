using UnityEngine;
using System.Collections.Generic;

namespace Manmaru.Player
{
    public class PlayerVisualController : MonoBehaviour
    {
        [Header("対象レンダラー設定")]
        [SerializeField] private List<Renderer> _renderersList = new List<Renderer>();

        [Header("状態ごとの色")]
        [SerializeField] private Color _normalColor = Color.magenta;
        [SerializeField] private Color _capturingColor = Color.red;
        [SerializeField] private Color _mouthfulColor = Color.yellow;

        [Header("状態ごとのスケール")]
        [SerializeField] private Vector3 _baseScale = Vector3.one;
        [SerializeField] private Vector3 _fatScale = new Vector3(1.2f, 1.0f, 1.0f);

        void Start()
        {
            _baseScale = transform.localScale;
        }

        public void ChangeToNormal()
        {
            ApplyColor(_normalColor);
            ApplyScale(_baseScale);
        }

        public void ChangeToCapturing()
        {
            ApplyColor(_capturingColor);
            ApplyScale(_baseScale);
        }

        public void ChangeToMouthful()
        {
            ApplyColor(_mouthfulColor);
            ApplyScale(_fatScale);
        }


        private void ApplyColor(Color argColor)
        {
            foreach (Renderer r in _renderersList)
            {
                r.material.color = argColor;
            }
        }

        private void ApplyScale(Vector3 argScale)
        {
            transform.localScale = argScale;
        }
    }
}