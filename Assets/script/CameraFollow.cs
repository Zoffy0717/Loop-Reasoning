using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    
    public float smoothSpeed = 5f;
    public Vector3 offset;

    private Bounds roomBounds;

    public Collider2D startBounds;

    public void SetRoomBounds(Collider2D bounds)
    {
        roomBounds = bounds.bounds;
    }

    void Start()
    {
        SetRoomBounds(startBounds);
    }
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        float minX = roomBounds.min.x + camWidth;
        float maxX = roomBounds.max.x - camWidth;
        float minY = roomBounds.min.y + camHeight;
        float maxY = roomBounds.max.y - camHeight;

        float clampedX = Mathf.Clamp(smoothed.x, minX, maxX);
        float clampedY = Mathf.Clamp(smoothed.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}