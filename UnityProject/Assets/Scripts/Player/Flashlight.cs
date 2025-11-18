using System;
using UnityEngine;

namespace TownWithoutLight.Player
{
    /// <summary>
    /// Wraps a Light component to provide a simple on/off toggle with optional sfx hooks
    /// and a rechargeable battery simulation that drains while the flashlight is enabled.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class Flashlight : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip toggleClip;

        [Header("Battery")]
        [SerializeField] private bool infiniteBattery;
        [SerializeField] private float maxBatteryLife = 180f;
        [SerializeField] private float drainPerSecond = 1f;
        [SerializeField] private float rechargePerSecond = 0.75f;
        [SerializeField] private float minTogglePercent = 0.05f;

        private Light _light;
        private float _currentBattery;

        public bool IsOn => _light.enabled;
        public float BatteryNormalized => maxBatteryLife <= 0f ? 1f : Mathf.Clamp01(_currentBattery / maxBatteryLife);

        public event Action<float, bool> OnFlashlightStateChanged;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _currentBattery = Mathf.Max(maxBatteryLife, 0.01f);
            NotifyListeners();
        }

        private void Update()
        {
            UpdateBattery();
        }

        public void Toggle()
        {
            SetActive(!IsOn);
        }

        public void SetActive(bool value)
        {
            if (IsOn == value)
            {
                return;
            }

            if (value && !CanEnable())
            {
                return;
            }

            _light.enabled = value;
            PlayToggleSound();
            NotifyListeners();
        }

        public void RestoreBattery(float normalizedValue)
        {
            if (infiniteBattery)
            {
                return;
            }

            normalizedValue = Mathf.Clamp01(normalizedValue);
            _currentBattery = maxBatteryLife * normalizedValue;
            NotifyListeners();
        }

        private void UpdateBattery()
        {
            if (infiniteBattery || maxBatteryLife <= 0f)
            {
                return;
            }

            float previousBattery = _currentBattery;
            bool previousState = _light.enabled;

            if (IsOn)
            {
                _currentBattery -= drainPerSecond * Time.deltaTime;
                if (_currentBattery <= 0f)
                {
                    _currentBattery = 0f;
                    _light.enabled = false;
                    PlayToggleSound();
                }
            }
            else
            {
                _currentBattery = Mathf.Min(maxBatteryLife, _currentBattery + rechargePerSecond * Time.deltaTime);
            }

            if (!Mathf.Approximately(previousBattery, _currentBattery) || previousState != _light.enabled)
            {
                NotifyListeners();
            }
        }

        private bool CanEnable()
        {
            if (infiniteBattery || maxBatteryLife <= 0f)
            {
                return true;
            }

            return BatteryNormalized >= minTogglePercent;
        }

        private void NotifyListeners()
        {
            OnFlashlightStateChanged?.Invoke(BatteryNormalized, IsOn);
        }

        private void PlayToggleSound()
        {
            if (audioSource != null && toggleClip != null)
            {
                audioSource.PlayOneShot(toggleClip);
            }
        }
    }
}
