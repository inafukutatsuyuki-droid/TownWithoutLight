using UnityEngine;

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
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
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
    }
}
