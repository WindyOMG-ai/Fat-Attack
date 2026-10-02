using System;
using System.Runtime.InteropServices;

public static class JoyShockNative
{
    private const string DLL_NAME = "JoyShockLibrary";

    [StructLayout(LayoutKind.Sequential)]
    public struct JOY_SHOCK_STATE
    {
        public int buttons;
        public float lX, lY;
        public float rX, rY;
        public float triggerL, triggerR;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMU_STATE
    {
        public float accelX, accelY, accelZ;
        public float gyroX, gyroY, gyroZ;
    }

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern int JSLConnectDevices();

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern int JSLGetConnectedDeviceHandles(int[] deviceHandles, int maxDevices);

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern void JSLDisconnectAndDisposeAll();

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern JOY_SHOCK_STATE JSLGetSimpleState(int deviceId);

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern IMU_STATE JSLGetIMUState(int deviceId);
}