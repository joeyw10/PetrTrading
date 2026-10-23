
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerGPS : MonoBehaviour
{
    [Header("Testing Settings")]
    public bool useKeyboardSimulation = true;
    public float simulatedLatitude = 33.6405f;
    public float simulatedLongitude = -117.8443f;
    public float movementSpeed = 0.0001f;

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public float Accuracy { get; private set; }
    public bool IsReady { get; private set; }

    private IEnumerator Start()
    {
#if UNITY_EDITOR
        if (useKeyboardSimulation)
        {
            IsReady = true;
            Accuracy = 1f;

            while (true)
            {
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

                Latitude = simulatedLatitude;
                Longitude = simulatedLongitude;

                Debug.Log(
                    "Simulated GPS: " + Latitude + ", " + Longitude
                );

                yield return null;
            }
        }
#endif

        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning(
                "Please enable location services on your phone."
            );
            yield break;
        }

        Input.location.Start(5f, 2f);

        int timeout = 20;

        while (Input.location.status ==
               LocationServiceStatus.Initializing && timeout > 0)
        {
            yield return new WaitForSeconds(1);
            timeout--;
        }

        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogError(
                "GPS failed to start or permission was denied."
            );
            yield break;
        }

        IsReady = true;

        while (true)
        {
            LocationInfo location = Input.location.lastData;

            Latitude = location.latitude;
            Longitude = location.longitude;
            Accuracy = location.horizontalAccuracy;

            Debug.Log(
                "Latitude: " + Latitude +
                ", Longitude: " + Longitude +
                ", Accuracy: " + Accuracy + " meters"
            );

            yield return new WaitForSeconds(1);
        }
    }

    private void OnDestroy()
    {
#if !UNITY_EDITOR
        Input.location.Stop();
#endif
    }
}
