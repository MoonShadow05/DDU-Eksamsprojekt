using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public float lookSpeed = 0.1f;
    public Transform playerBody;
    private float xRotation = 0f;

    private bool isFrozen = false; // Add this flag

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isFrozen) return; // Skip look input if frozen

        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * lookSpeed;

        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseDelta.x);
    }

    public void SetFrozen(bool freeze)
    {
        isFrozen = freeze;
    }
}
