using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCamShake : MonoBehaviour
{
    [Header("Headbob Settings")]
    [Range(0.001f, 0.01f)]
    public float shakeAmount = 0.003f;

    [Range(1f, 30f)]
    public float frequency = 10.0f;

    [Range(10f, 100f)]
    public float smooth = 10.0f;

    private CharacterController _controller;
    private Vector3 _startPos;
    private float _bobTimer;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _startPos = transform.localPosition;
    }

    void Update()
    {
        ApplyHeadbobIfMoving();
    }

    private void ApplyHeadbobIfMoving()
    {
        // Only check horizontal movement, not falling or jumping
        float horizontalSpeed = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z).magnitude;

        if (_controller.isGrounded && horizontalSpeed > 0.1f)
        {
            _bobTimer += Time.deltaTime * frequency;

            Vector3 bobOffset = Vector3.zero;
            bobOffset.y = Mathf.Sin(_bobTimer) * shakeAmount * 1.4f;
            bobOffset.x = Mathf.Cos(_bobTimer * 0.5f) * shakeAmount;

            // Apply head bobbing only to the camera's local position
            transform.localPosition = _startPos + bobOffset;
        }
        else
        {
            // Smoothly return the camera to its original position when not moving
            transform.localPosition = Vector3.Lerp(transform.localPosition, _startPos, Time.deltaTime * smooth);
        }
    }
}