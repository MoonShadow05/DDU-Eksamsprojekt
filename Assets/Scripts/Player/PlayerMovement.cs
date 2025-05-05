using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 8f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 inputMovement;
    private Vector3 velocity;
    private bool jumpPressed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // ===== MOVEMENT INPUT =====
        Vector2 moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;

        inputMovement = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpPressed = true;
    }

    void FixedUpdate()
    {
        float currentSpeed = Keyboard.current.shiftKey.isPressed ? sprintSpeed : walkSpeed;

        // Use the camera's orientation for direction
        Transform cam = Camera.main.transform;
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camRight * inputMovement.x + camForward * inputMovement.z).normalized * currentSpeed;

        if (controller.isGrounded)
        {
            velocity.y = -1f;
            if (jumpPressed)
            {
                velocity.y = jumpForce;
                jumpPressed = false;
            }
        }
        else
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        Vector3 finalVelocity = move + Vector3.up * velocity.y;
        controller.Move(finalVelocity * Time.fixedDeltaTime);
    }
}
