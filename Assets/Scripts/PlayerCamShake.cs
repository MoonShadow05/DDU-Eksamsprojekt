using UnityEngine;

public class PlayerCamShake : MonoBehaviour
{
    [Header("Headbob Settings")]
    [Range(0.001f, 0.05f)] public float shakeAmount = 0.02f;
    [Range(1f, 30f)] public float frequency = 8.0f;
    [Range(10f, 100f)] public float smooth = 10.0f;

    private CharacterController _controller;
    private Vector3 _startPos;
    private float _bobTimer;

    void Start()
    {
        _controller = GetComponentInParent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("CharacterController not found in parent hierarchy!");
            enabled = false;
            return;
        }

        _startPos = transform.localPosition;
    }

    void Update()
    {
        ApplySideToSideHeadbob();
    }

    private void ApplySideToSideHeadbob()
    {
        float horizontalSpeed = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z).magnitude;

        if (_controller.isGrounded && horizontalSpeed > 0.1f)
        {
            _bobTimer += Time.deltaTime * frequency;

            Vector3 bobOffset = Vector3.zero;
            bobOffset.x = Mathf.Sin(_bobTimer) * shakeAmount;

            transform.localPosition = _startPos + bobOffset;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _startPos, Time.deltaTime * smooth);
            _bobTimer = 0f; // Reset timer to sync when movement starts again
        }
    }
}
