using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Tilemap tilemap;

    float minX;
    float maxX;

    void Start()
    {
        Bounds bounds = tilemap.GetComponent<TilemapRenderer>().bounds;

        minX = bounds.min.x+0.3f;
        maxX = bounds.max.x-0.3f;
    }

    void LateUpdate()
    {
        float camHalfWidth = Camera.main.orthographicSize * (float)Screen.width / Screen.height;

        float targetX = target.position.x;

        float minCamX = minX + camHalfWidth;
        float maxCamX = maxX - camHalfWidth;

        float finalX;

        if (minCamX > maxCamX)
            finalX = (minX + maxX) / 2f;
        else
            finalX = Mathf.Clamp(targetX, minCamX, maxCamX);

        Vector3 pos = transform.position;
        pos.x = finalX;
        transform.position = pos;
    }
}