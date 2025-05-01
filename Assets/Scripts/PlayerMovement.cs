using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 5f;
    private float jumpForce = 8f;

    private float lookSpeed = 0.1f;   // Speed of mouse look
    private Rigidbody rb;
    private Vector3 inputMovement;
    private Vector3 cameraRotation;
    private Vector3 playerRotation;
    private bool jumpPressed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Mouse look
        cameraRotation.x -= Mouse.current.delta.y.ReadValue() * lookSpeed; // Up and down
        cameraRotation.y += Mouse.current.delta.x.ReadValue() * lookSpeed; // Left and right
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 90f); // Clamp up and down rotation
        Camera.main.transform.localRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0f); // Apply rotation to camera
        playerRotation.y = cameraRotation.y; // Keep player rotation in sync with camera
        transform.localRotation = Quaternion.Euler(0f, playerRotation.y, 0f); // Apply rotation to player


        // Keyboard input
        inputMovement = Vector3.zero;
        jumpPressed = false;

        if (Keyboard.current.wKey.isPressed && Keyboard.current.sKey.isPressed)
            inputMovement.z = 0; // No forward/backward movement
        else if (Keyboard.current.wKey.isPressed)
            inputMovement.z = 1; // Move forward
        else if (Keyboard.current.sKey.isPressed)
            inputMovement.z = -1; // Move backward
        if (Keyboard.current.aKey.isPressed && Keyboard.current.dKey.isPressed)
            inputMovement.x = 0; // No left/right movement
        else if (Keyboard.current.aKey.isPressed)
            inputMovement.x = -1; // Move left
        else if (Keyboard.current.dKey.isPressed)
            inputMovement.x = 1; // Move right
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpPressed = true; // Jump when space is pressed
        else if (Keyboard.current.shiftKey.isPressed)
            speed = 10f; // Sprint when shift is pressed
        else
            speed = 5f; // Normal speed

    }
    void FixedUpdate()
    {
    Vector3 move = new Vector3(inputMovement.x, 0f, inputMovement.z);

    // Camera-relative movement
    Vector3 camForward = Camera.main.transform.forward;
    Vector3 camRight = Camera.main.transform.right;

    // Only care about XZ plane (flatten the Y)
    camForward.y = 0;
    camRight.y = 0;
    camForward.Normalize();
    camRight.Normalize();

    Vector3 desiredMove = (camForward * move.z + camRight * move.x).normalized;

    // Move the player
    rb.MovePosition(rb.position + desiredMove * speed * Time.fixedDeltaTime);

    // Jump
    if (jumpPressed && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    }


}
