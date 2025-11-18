using UnityEngine;
using TownWithoutLight.Interaction;

namespace TownWithoutLight.Player
{
    /// <summary>
    /// Handles WASD character movement, jumping, and flashlight toggling while delegating camera rotation to <see cref="PlayerLook"/>.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4.5f;
        [SerializeField] private float sprintMultiplier = 1.6f;
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float gravity = -9.81f;

        [Header("References")]
        [SerializeField] private Flashlight flashlight;
        [SerializeField] private Interactor interactor;

        private CharacterController _controller;
        private Vector3 _verticalVelocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleMovement();
            HandleFlashlight();
        }

        private void HandleMovement()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            input = Vector2.ClampMagnitude(input, 1f);

            float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * sprintMultiplier : moveSpeed;
            Vector3 move = (transform.right * input.x + transform.forward * input.y) * speed;
            _controller.Move(move * Time.deltaTime);

            if (_controller.isGrounded && _verticalVelocity.y < 0f)
            {
                _verticalVelocity.y = -2f;
            }

            if (_controller.isGrounded && Input.GetButtonDown("Jump"))
            {
                _verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            _verticalVelocity.y += gravity * Time.deltaTime;
            _controller.Move(_verticalVelocity * Time.deltaTime);
        }

        private void HandleFlashlight()
        {
            if (flashlight == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                flashlight.Toggle();
            }
        }
    }
}
