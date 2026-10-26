
using UnityEngine;
using UnityEngine.EventSystems;

public class MapTouchDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public PlayerGPS playerGPS;
    public RectTransform mapViewport;

    public float coordinateScale = 0.00001f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Start dragging the map.
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (playerGPS == null || mapViewport == null)
            return;

        Vector2 dragDelta = eventData.delta;

        float longitudeChange = -dragDelta.x * coordinateScale;
        float latitudeChange = -dragDelta.y * coordinateScale;

        playerGPS.SimulateMovement(latitudeChange, longitudeChange);
    }
}
