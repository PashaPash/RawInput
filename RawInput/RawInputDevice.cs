using System;
using System.Runtime.InteropServices;

namespace RawInput
{
    public class RawInputDevice
    {
        private readonly Win32.RawInputDeviceList _rawInputDeviceList;
        private readonly string _deviceName;
        private readonly Win32.DeviceInfo _deviceInfo;

        public IntPtr DeviceHandle
        {
            get
            {
                return _rawInputDeviceList.DeviceHandle;
            }
        }

        public Win32.RawInputDeviceType DeviceType
        {
            get
            {
                return _rawInputDeviceList.DeviceType;
            }
        }

        public string DeviceName
        {
            get
            {
                return this._deviceName;
            }
        }

        public Win32.DeviceInfo DeviceInfo
        {
            get
            {
                return this._deviceInfo;
            }
        }

        public RawInputDevice(Win32.RawInputDeviceList rawInputDeviceList)
        {
            this._rawInputDeviceList = rawInputDeviceList;

            _deviceName =
                GetDeviceName(this._rawInputDeviceList.DeviceHandle);

            _deviceInfo =
                GetDeviceInfo(this._rawInputDeviceList.DeviceHandle);
        }

        private static IntPtr GetDeviceData(
            IntPtr deviceHandle,
            Win32.RawInputDeviceInfoCommand command)
        {
            uint dataSize = 0;
            var ptrData = IntPtr.Zero;

            Win32.GetRawInputDeviceInfo(
                deviceHandle,
                command,
                ptrData,
                ref dataSize);

            if (dataSize == 0) return IntPtr.Zero;

            ptrData = Marshal.AllocHGlobal((int)dataSize);

            var result = Win32.GetRawInputDeviceInfo(
                deviceHandle,
                command,
                ptrData,
                ref dataSize);

            if (result == 0)
            {
                Marshal.FreeHGlobal(ptrData);
                return IntPtr.Zero;
            }

            return ptrData;
        }

        private static string GetDeviceName(IntPtr deviceHandle)
        {
            var ptrDeviceName = GetDeviceData(
                deviceHandle,
                Win32.RawInputDeviceInfoCommand.DeviceName);

            if (ptrDeviceName == IntPtr.Zero)
            {
                return string.Empty;
            }

            var deviceName = Marshal.PtrToStringAnsi(ptrDeviceName);
            Marshal.FreeHGlobal(ptrDeviceName);
            return deviceName;
        }

        private static Win32.DeviceInfo GetDeviceInfo(IntPtr deviceHandle)
        {
            var ptrDeviceInfo = GetDeviceData(
                deviceHandle,
                Win32.RawInputDeviceInfoCommand.DeviceInfo);

            if (ptrDeviceInfo == IntPtr.Zero)
            {
                return new Win32.DeviceInfo();
            }

            var deviceInfo = (Win32.DeviceInfo)Marshal.PtrToStructure(
                ptrDeviceInfo, typeof(Win32.DeviceInfo));

            Marshal.FreeHGlobal(ptrDeviceInfo);
            return deviceInfo;
        }
    }
}
