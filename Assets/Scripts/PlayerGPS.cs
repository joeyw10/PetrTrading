
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerGPS : MonoBehaviour
{
    [Header("Testing Settings")]
    public float simulatedLatitude = 33.6405f;
    public float simulatedLongitude = -117.8443f;
    public float movementSpeed = 0.0001f;

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public float Accuracy { get; private set; }
    public bool IsReady { get; private set; }

    private IEnumerator Start()
    {
        Latitude = simulatedLatitude;
        Longitude = simulatedLongitude;
        Accuracy = 1f;
        IsReady = true;

        while (true)
        {
#if UNITY_EDITOR
            float vertical = 0f;
            float horizontal = 0f;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.wKey.isPressed ||
                    Keyboard.current.upArrowKey.isPressed)
                    vertical = 1f;

                if (Keyboard.current.sKey.isPressed ||
                    Keyboard.current.downArrowKey.isPressed)
                    vertical = -1f;

                if (Keyboard.current.dKey.isPressed ||
                    Keyboard.current.rightArrowKey.isPressed)
                    horizontal = 1f;

                if (Keyboard.current.aKey.isPressed ||
                    Keyboard.current.leftArrowKey.isPressed)
                    horizontal = -1f;
            }

            simulatedLatitude +=
                vertical * movementSpeed * Time.deltaTime * 60f;

            simulatedLongitude +=
                horizontal * movementSpeed * Time.deltaTime * 60f;
#endif

            Latitude = simulatedLatitude;
            Longitude = simulatedLongitude;

            yield return null;
        }
    }

    public void SimulateMovement(float latitudeChange, float longitudeChange)
    {
        simulatedLatitude += latitudeChange;
        simulatedLongitude += longitudeChange;

        Latitude = simulatedLatitude;
        Longitude = simulatedLongitude;
    }
}
