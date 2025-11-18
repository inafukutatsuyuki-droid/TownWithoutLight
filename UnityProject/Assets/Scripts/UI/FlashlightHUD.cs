using UnityEngine;
using UnityEngine.UI;
using TownWithoutLight.Player;

namespace TownWithoutLight.UI
{
    /// <summary>
    /// Reflects the current flashlight power state and battery percentage on the HUD.
    /// </summary>
    public class FlashlightHUD : MonoBehaviour
    {
        [SerializeField] private Flashlight flashlight;
        [SerializeField] private Slider batterySlider;
        [SerializeField] private Image statusImage;
        [SerializeField] private Color onColor = Color.white;
        [SerializeField] private Color offColor = Color.gray;

        private void OnEnable()
        {
            if (flashlight == null)
            {
                flashlight = FindObjectOfType<Flashlight>();
            }

            if (flashlight != null)
            {
                flashlight.OnFlashlightStateChanged += HandleFlashlightStateChanged;
                HandleFlashlightStateChanged(flashlight.BatteryNormalized, flashlight.IsOn);
            }
        }

        private void OnDisable()
        {
            if (flashlight != null)
            {
                flashlight.OnFlashlightStateChanged -= HandleFlashlightStateChanged;
            }
        }

        private void HandleFlashlightStateChanged(float battery, bool isOn)
        {
            if (batterySlider != null)
            {
                batterySlider.normalizedValue = battery;
            }

            if (statusImage != null)
            {
                statusImage.color = isOn ? onColor : offColor;
            }
        }
    }
}
