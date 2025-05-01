using System;
using UnityEngine;

public class PlayerCamShake : MonoBehaviour
{
    [Header("Toggle")]
    [SerializeField] private bool _enabled = true;

    [Header("Shake Settings")]
    [SerializeField, Range(0, 0.5f)] private float _shakeDuration = 0.15f;
    [SerializeField, Range(0, 1f)] private float _shakeMagnitude = 0.05f;

    [Header("Headbob Settings")]
    [SerializeField, Range(0, 20f)] private float _bobFrequency = 10f;
    [SerializeField, Range(0, 0.1f)] private float _bobAmplitude = 0.015f;

    [Header("References")]
    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform _cameraHolder = null;

    private Vector3 _startPos;
    private CharacterController _controller;
    private float _bobTimer;
    private bool _isShaking;
    private bool _wasGrounded;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (_camera == null) _camera = Camera.main.transform;
        _startPos = _camera.localPosition;
    }

    private void Update()
    {
        if (!_enabled || _controller == null || _camera == null) return;

        bool isGrounded = _controller.isGrounded;
        float speed = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;

        // Detect jump start and landing
        if (!_wasGrounded && isGrounded)
            TriggerShake(-_shakeMagnitude); // landing bump
        else if (_wasGrounded && !isGrounded)
            TriggerShake(_shakeMagnitude); // jump start bump

        // Walk bob if grounded and moving
        if (isGrounded && speed > 0.1f && !_isShaking)
        {
            _bobTimer += Time.deltaTime * _bobFrequency;
            float bobOffset = Mathf.Sin(_bobTimer) * _bobAmplitude;
            _camera.localPosition = _startPos + new Vector3(0, bobOffset, 0);
        }
        else if (!_isShaking)
        {
            _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, Time.deltaTime * 5f);
        }

        _wasGrounded = isGrounded;
    }

    private void TriggerShake(float direction)
    {
        if (!_isShaking)
            StartCoroutine(DoShake(direction));
    }

    private System.Collections.IEnumerator DoShake(float dir)
    {
        _isShaking = true;
        float elapsed = 0f;

        while (elapsed < _shakeDuration)
        {
            float t = elapsed / _shakeDuration;
            float curve = Mathf.Sin(t * Mathf.PI); // Smooth ease in/out
            _camera.localPosition = _startPos + new Vector3(0, dir * curve, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _isShaking = false;
    }
}
