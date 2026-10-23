
using UnityEngine;

public class MapGPSController : MonoBehaviour
{
    public PlayerGPS playerGPS;
    public RectTransform mapTransform;
    public RectTransform mapViewport;

    [Header("Map Movement")]
    public float mapMovementScale = 500000f;

    private double previousLatitude;
    private double previousLongitude;
    private bool hasPreviousPosition;

    void Update()
    {
        if (playerGPS == null || mapTransform == null)
            return;

        if (!playerGPS.IsReady)
            return;

        double latitude = playerGPS.Latitude;
        double longitude = playerGPS.Longitude;

        if (!hasPreviousPosition)
        {
            previousLatitude = latitude;
            previousLongitude = longitude;
            hasPreviousPosition = true;
            return;
        }

        float latitudeChange =
            (float)(latitude - previousLatitude);

        float longitudeChange =
            (float)(longitude - previousLongitude);

        Vector2 movement = new Vector2(
            -longitudeChange * mapMovementScale,
            -latitudeChange * mapMovementScale
        );

        mapTransform.anchoredPosition += movement;

   

        previousLatitude = latitude;
        previousLongitude = longitude;
    }

    void KeepMapInsideViewport()
    {
        Vector3[] mapCorners = new Vector3[4];
        Vector3[] viewportCorners = new Vector3[4];

        mapTransform.GetWorldCorners(mapCorners);
        mapViewport.GetWorldCorners(viewportCorners);

        Vector2 position = mapTransform.anchoredPosition;

        float mapLeft = mapCorners[0].x;
        float mapRight = mapCorners[2].x;
        float mapBottom = mapCorners[0].y;
        float mapTop = mapCorners[2].y;

        float viewLeft = viewportCorners[0].x;
        float viewRight = viewportCorners[2].x;
        float viewBottom = viewportCorners[0].y;
        float viewTop = viewportCorners[2].y;

        if (mapLeft > viewLeft)
            position.x -= mapLeft - viewLeft;

        if (mapRight < viewRight)
            position.x += viewRight - mapRight;

        if (mapBottom > viewBottom)
            position.y -= mapBottom - viewBottom;

        if (mapTop < viewTop)
            position.y += viewTop - mapTop;

        mapTransform.anchoredPosition = position;
    }
}
