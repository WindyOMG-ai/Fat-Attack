using UnityEngine;

public class JoyConManager : MonoBehaviour
{
    private int deviceHandle = -1;

    void Start()
    {
        // 1. Sök efter och anslut kontroll
        int count = JoyShockNative.JSLConnectDevices();
        if (count > 0)
        {
            int[] handles = new int[count];
            JoyShockNative.JSLGetConnectedDeviceHandles(handles, count);
            deviceHandle = handles[0];
            Debug.Log("Joy-Con ansluten!");
        }
    }

    void Update()
    {
        if (deviceHandle == -1) return;

        // 2. Läs styrspak & knappar
        JoyShockNative.JOY_SHOCK_STATE state = JoyShockNative.JSLGetSimpleState(deviceHandle);
        Vector2 stick = new Vector2(state.lX, state.lY);

        // 3. Läs rörelsesensorer
        JoyShockNative.IMU_STATE imu = JoyShockNative.JSLGetIMUState(deviceHandle);
        Vector3 gyro = new Vector3(imu.gyroX, imu.gyroY, imu.gyroZ);

        // Skriv ut datan i konsolen
        if (stick.magnitude > 0.1f) Debug.Log($"Spak: {stick}");
        Debug.Log($"Gyro: {gyro}");
    }

    void OnApplicationQuit()
    {
        // 4. Koppla från vid avslut
        JoyShockNative.JSLDisconnectAndDisposeAll();
    }
}