using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Inst;
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float dragSpeed = 5f;

    [Header("Zoom Limits")]
    [SerializeField] private float minHeight = 10f;
    [SerializeField] private float maxHeight = 100f;
    [SerializeField] private float minX = -100f;
    [SerializeField] private float maxX = 100f;
    [SerializeField] private float minZ = -100f;
    [SerializeField] private float maxZ = 100f;

    private void Awake()
    {
        Inst = this;
    }
    private void Update()
    {
        HandleMovement();
        HandleMouseDrag();
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

    private Vector3 lastMousePosition;
    private void HandleMouseDrag()
    {
        // 鼠标右键拖拽
        if (Input.GetMouseButtonDown(1))
        {
            // 记录初始鼠标位置
            lastMousePosition = Input.mousePosition;
        }

        // 持续右键拖拽
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // 计算移动，使用相机坐标系
            Vector3 moveDirection = new Vector3(-delta.x, 0, -delta.y);
            Vector3 move = moveDirection * dragSpeed * Time.deltaTime;

            // 应用移动
            Vector3 newPosition = transform.position + move;

            // 边界限制
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

            transform.position = newPosition;

            // 更新上一帧鼠标位置
            lastMousePosition = Input.mousePosition;
        }
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