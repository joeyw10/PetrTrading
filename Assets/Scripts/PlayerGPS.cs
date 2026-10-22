using UnityEngine;
using System.Collections;

public class PlayerGPS : MonoBehaviour
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public float Accuracy { get; private set; }
    public bool IsReady { get; private set; }

    private IEnumerator Start()
    {
        // Check whether location services are enabled.
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning(
                "Please enable location services on your phone."
            );
            yield break;
        }

        // Request location updates.
        Input.location.Start(5f, 2f);

        int timeout = 20;

        // Wait for the first GPS reading.
        while (Input.location.status ==
               LocationServiceStatus.Initializing &&
               timeout > 0)
        {
            yield return new WaitForSeconds(1);
            timeout--;
        }

        // Check whether GPS started successfully.
        if (Input.location.status !=
            LocationServiceStatus.Running)
        {
            Debug.LogError(
                "GPS failed to start or permission was denied."
            );
            yield break;
        }

        IsReady = true;

        // Continuously read the latest location.
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
        Input.location.Stop();
    }
}