using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RightClickDragCamera : MonoBehaviour
{
    [Header("�϶�����")]
    [SerializeField] private float dragSpeed = 2f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private bool invertHorizontal = false;
    [SerializeField] private bool invertVertical = false;

    [Header("�߽�����")]
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
        // �����Ҽ�ʱ��¼��ʼλ��
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        // ��ס�Ҽ�ʱ�ƶ����
        if (Input.GetMouseButton(1))
        {
            Vector3 currentPos = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPos;

            // Ӧ�÷�ת����
            difference.x = invertHorizontal ? -difference.x : difference.x;
            difference.y = invertVertical ? -difference.y : difference.y;

            // ����Ŀ��λ��
            Vector3 targetPosition = transform.position + difference * dragSpeed;

            // Ӧ�ñ߽�����
            if (enableBounds)
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x + cam.orthographicSize *cam.aspect, maxBounds.x - cam.orthographicSize * cam.aspect);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y + cam.orthographicSize, maxBounds.y - cam.orthographicSize);
            }

            // ƽ���ƶ�
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    // �ڱ༭���п��ӻ��߽�
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

    // ��������߽磨��������ʱ���ã�
    public void SetCameraBounds(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
        enableBounds = true;
    }

    // ����/���ñ߽�����
    public void EnableBounds(bool enabled)
    {
        enableBounds = enabled;
    }
}
