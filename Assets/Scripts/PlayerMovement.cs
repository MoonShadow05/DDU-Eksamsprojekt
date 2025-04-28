using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private Vector3 inputMovement;
    private bool jumpPressed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputMovement = Vector3.zero;
        jumpPressed = false;

        if (Keyboard.current.wKey.isPressed)
            inputMovement.z = 1; // Move forward
        if (Keyboard.current.sKey.isPressed)
            inputMovement.z = -1; // Move backward
        if (Keyboard.current.aKey.isPressed)
            inputMovement.x = -1; // Move left
        if (Keyboard.current.dKey.isPressed)
            inputMovement.x = 1; // Move right
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpPressed = true; // Only trigger once per keypress
    }

    void FixedUpdate()
    {
        // Move left-right-forward-back
        Vector3 move = new Vector3(inputMovement.x, 0f, inputMovement.z);
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);

        // Jump
        if (jumpPressed && Mathf.Abs(rb.linearVelocity.y) < 0.01f) // Check if almost grounded
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
