/*using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Ustawienia kamery")]
    public Transform target;         
    public float smoothSpeed = 0.125f; 
    public Vector3 offset;          

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
*/

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [Header("Gracze do śledzenia")]
    public List<Transform> target;

    [Header("Mapa")]
    
    public Tilemap tilemap;

    [Header("Ruch kamery")]
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothTime = 0.3f;

    [Header("Zoom kamery")]
    public float minZoom = 5f;
    public float zoomSmoothSpeed = 2f;

    [Header("Granice mapy")]
    public bool limitToBounds = true;

    private Vector2 minBounds;
    private Vector2 maxBounds;

    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        BoundsInt bounds = tilemap.cellBounds;
        Vector3 min = tilemap.CellToWorld(bounds.min);
        Vector3 max = tilemap.CellToWorld(bounds.max);

        minBounds = (Vector2)min;
        maxBounds = (Vector2)max;
    }

    void LateUpdate()
    {
        if (target.Count == 0) return;

        Move();
        Zoom();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        if (limitToBounds)
        {
            float cameraHeight = cam.orthographicSize;
            float cameraWidth = cameraHeight * cam.aspect;

            float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + cameraWidth, maxBounds.x - cameraWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + cameraHeight, maxBounds.y - cameraHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }

    void Zoom()
    {
        float maxZoomX = (maxBounds.x - minBounds.x) / 2f / cam.aspect;
        float maxZoomY = (maxBounds.y - minBounds.y) / 2f;
        float maxPossibleZoom = Mathf.Min(maxZoomX, maxZoomY);
        float distance = GetGreatestDistance();
        float targetZoom = minZoom + distance * 0.5f;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxPossibleZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSmoothSpeed);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(target[0].position, Vector3.zero);
        for (int i = 0; i < target.Count; i++)
        {
            bounds.Encapsulate(target[i].position);
        }
        return Mathf.Max(bounds.size.x, bounds.size.y);
    }

    Vector3 GetCenterPoint()
    {
        if (target.Count == 1)
            return target[0].position;

        var bounds = new Bounds(target[0].position, Vector3.zero);
        for (int i = 0; i < target.Count; i++)
        {
            bounds.Encapsulate(target[i].position);
        }
        return bounds.center;
    }
}
