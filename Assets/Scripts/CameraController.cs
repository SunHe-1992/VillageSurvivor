using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Inst;
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float zoomSpeed = 20f;

    [Header("Zoom Limits")]
    [SerializeField] private float minHeight = 10f;
    [SerializeField] private float maxHeight = 100f;

    private void Awake()
    {
        Inst = this;
    }
    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        // Get input axis
        float horizontal = Input.GetAxis("Horizontal"); // A D or Arrow keys
        float vertical = Input.GetAxis("Vertical");     // W S or Arrow keys

        // Calculate movement vector
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        // Make movement speed independent of framerate
        moveDirection *= moveSpeed * Time.deltaTime;

        // Move the camera
        transform.position += moveDirection;
    }

    private void HandleZoom()
    {
        // Get mouse scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            // Calculate new position
            Vector3 pos = transform.position;
            pos.y -= scroll * zoomSpeed;

            // Clamp height between min and max values
            pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);

            // Apply new position
            transform.position = pos;
        }
    }
}