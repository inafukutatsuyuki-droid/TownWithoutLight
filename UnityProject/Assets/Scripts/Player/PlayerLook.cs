using UnityEngine;
using TownWithoutLight.Systems;

namespace TownWithoutLight.Player
{
    /// <summary>
    /// Handles first-person mouse look rotation with clamped vertical angles.
    /// </summary>
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private float sensitivity = 1.5f;
        [SerializeField] private float verticalClamp = 85f;

        private float _pitch;

        private void Reset()
        {
            cameraRoot = GetComponentInChildren<Camera>()?.transform;
        }

        private void Start()
        {
            ApplyCursorState(true);
        }

        private void Update()
        {
            var stateManager = GameStateManager.Instance;
            if (stateManager != null && stateManager.CurrentState != GameState.Exploring)
            {
                return;
            }

            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            transform.Rotate(Vector3.up * mouseX);

            _pitch -= mouseY;
            _pitch = Mathf.Clamp(_pitch, -verticalClamp, verticalClamp);

            if (cameraRoot != null)
            {
                cameraRoot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            }
        }

        public void SetSensitivity(float value)
        {
            sensitivity = Mathf.Max(0.01f, value);
        }

        public float GetSensitivity() => sensitivity;

        public void ApplyCursorState(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
    }
}
