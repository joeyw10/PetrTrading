using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Debug helper: logs the UI object under the pointer whenever it changes.
public class RaycastDebugger : MonoBehaviour
{
    private string _last = "";

    private void Update()
    {
        if (EventSystem.current == null || Pointer.current == null) return;

        var data = new PointerEventData(EventSystem.current)
        {
            position = Pointer.current.position.ReadValue()
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        string top = results.Count > 0 ? results[0].gameObject.name : "(nothing)";
        if (top != _last)
        {
            _last = top;
            Debug.Log("Topmost UI under pointer: " + top);
        }
    }
}
