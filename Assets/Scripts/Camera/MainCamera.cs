using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [Header("Camera Settings")]
    [SerializeField]
    private Vector3 offset = new Vector3(0, 6, -3);

    [SerializeField]
    private Vector3 lookAtOffset = new Vector3(0, 1.5f, 0);

    [Header("Rotation Settings")]
    [SerializeField]
    private float rotateSpeedX = 180f;

    [SerializeField]
    private float rotateSpeedY = 180f;

    [SerializeField]
    private float minPitch = -45f;

    [SerializeField]
    private float maxPitch = 10f;

    private float _yaw;
    public float Yaw => _yaw;
    private float _pitch;

    private Vector2 _prevMousePosition;
    private bool _isDragging;

    private void Update()
    {
        HandleInput();
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _prevMousePosition = Input.mousePosition;
        }

        if (_isDragging && Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = Input.mousePosition;

            Vector2 delta = currentMousePosition - _prevMousePosition;

            _yaw += rotateSpeedY * (delta.x / Screen.width);

            _pitch -= rotateSpeedX * (delta.y / Screen.height);

            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            _prevMousePosition = currentMousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }

    private void FollowTarget()
    {
        if (target == null)
            return;

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);

        Vector3 rotatedOffset = rotation * offset;

        transform.position = target.position + rotatedOffset;

        transform.LookAt(target.position + lookAtOffset);
    }
}
