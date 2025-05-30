using UnityEngine;

public class PerformanceSettings : MonoBehaviour
{
    void Awake()
    {
        // Set target frame rate to 60 FPS
        Application.targetFrameRate = 60;

        // Prevent the device from going to sleep
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
