using UnityEngine;

namespace TownWithoutLight.Player
{
    /// <summary>
    /// Wraps a Light component to provide a simple on/off toggle with optional sfx hooks.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class Flashlight : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip toggleClip;

        private Light _light;

        public bool IsOn => _light.enabled;

        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        public void Toggle()
        {
            SetActive(!IsOn);
        }

        public void SetActive(bool value)
        {
            if (_light.enabled == value)
            {
                return;
            }

            _light.enabled = value;
            if (audioSource != null && toggleClip != null)
            {
                audioSource.PlayOneShot(toggleClip);
            }
        }
    }
}
