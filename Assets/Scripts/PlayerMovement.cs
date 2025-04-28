using UnityEngine;
using UnityEngine.InputSystem; // <-- Important for the new Input System

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    private Vector2 inputMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // New Input System uses Keyboard.current
        inputMovement = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            inputMovement.y = +speed;
        if (Keyboard.current.sKey.isPressed)
            inputMovement.y = -speed;
        if (Keyboard.current.aKey.isPressed)
            inputMovement.x = -speed;
        if (Keyboard.current.dKey.isPressed)
            inputMovement.x = +speed;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(inputMovement.x, 0f, inputMovement.y);
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
