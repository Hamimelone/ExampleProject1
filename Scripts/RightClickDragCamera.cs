using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RightClickDragCamera : MonoBehaviour
{
    [Header("拖动设置")]
    [SerializeField] private float dragSpeed = 2f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private bool invertHorizontal = false;
    [SerializeField] private bool invertVertical = false;

    [Header("边界限制")]
    [SerializeField] private bool enableBounds = false;
    [SerializeField] private Vector2 minBounds = new Vector2(-10, -10);
    [SerializeField] private Vector2 maxBounds = new Vector2(10, 10);

    private Vector3 dragOrigin;
    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleRightClickDrag();
    }

    private void HandleRightClickDrag()
    {
        // 按下右键时记录起始位置
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        // 按住右键时移动相机
        if (Input.GetMouseButton(1))
        {
            Vector3 currentPos = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPos;

            // 应用反转设置
            difference.x = invertHorizontal ? -difference.x : difference.x;
            difference.y = invertVertical ? -difference.y : difference.y;

            // 计算目标位置
            Vector3 targetPosition = transform.position + difference * dragSpeed;

            // 应用边界限制
            if (enableBounds)
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x + cam.orthographicSize *cam.aspect, maxBounds.x - cam.orthographicSize * cam.aspect);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y + cam.orthographicSize, maxBounds.y - cam.orthographicSize);
            }

            // 平滑移动
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    // 在编辑器中可视化边界
    private void OnDrawGizmosSelected()
    {
        if (enableBounds)
        {
            Gizmos.color = Color.green;
            Vector3 center = new Vector3(
                (minBounds.x + maxBounds.x) * 0.5f,
                (minBounds.y + maxBounds.y) * 0.5f,
                transform.position.z
            );
            Vector3 size = new Vector3(
                maxBounds.x - minBounds.x,
                maxBounds.y - minBounds.y,
                0.1f
            );
            Gizmos.DrawWireCube(center, size);
        }
    }

    // 设置相机边界（可在运行时调用）
    public void SetCameraBounds(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
        enableBounds = true;
    }

    // 启用/禁用边界限制
    public void EnableBounds(bool enabled)
    {
        enableBounds = enabled;
    }
}
