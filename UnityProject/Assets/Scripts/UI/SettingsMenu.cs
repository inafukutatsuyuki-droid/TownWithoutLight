using UnityEngine;
using UnityEngine.UI;
using TownWithoutLight.Player;
using TownWithoutLight.Systems;

namespace TownWithoutLight.UI
{
    /// <summary>
    /// Handles runtime tuning of volume and mouse sensitivity.
    /// </summary>
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private PlayerLook playerLook;

        private void OnEnable()
        {
            if (playerLook == null)
            {
                playerLook = FindObjectOfType<PlayerLook>();
            }

            if (masterVolumeSlider != null)
            {
                masterVolumeSlider.value = AudioManager.Instance != null ? AudioManager.Instance.MasterVolume : 1f;
            }

            if (sensitivitySlider != null && playerLook != null)
            {
                sensitivitySlider.value = playerLook.GetSensitivity();
            }
        }

        public void Apply()
        {
            if (AudioManager.Instance != null && masterVolumeSlider != null)
            {
                AudioManager.Instance.SetMasterVolume(masterVolumeSlider.value);
            }

            if (playerLook != null && sensitivitySlider != null)
            {
                playerLook.SetSensitivity(sensitivitySlider.value);
            }
        }
    }
}
