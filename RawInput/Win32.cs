using System;
using System.Runtime.InteropServices;

namespace RawInput
{
public static class Win32
    {
        public enum RawInputDeviceType : uint
        {
            Mouse = 0,
            Keyboard = 1,
            HumanInterfaceDevice = 2
        }
 
        public enum RawInputDeviceInfoCommand : uint
        {
            PreparsedData = 0x20000005,
            DeviceName = 0x20000007,
            DeviceInfo = 0x2000000b,
        }
 
        [StructLayout(LayoutKind.Explicit)]
        public struct DeviceInfo
        {
            [FieldOffset(0)]
            public int Size;
            [FieldOffset(4)]
            public int Type;
            [FieldOffset(8)]
            public DeviceInfoMouse MouseInfo;
            [FieldOffset(8)]
            public DeviceInfoKeyboard KeyboardInfo;
            [FieldOffset(8)]
            public DeviceInfoHID HIDInfo;
        }
 
        public struct DeviceInfoMouse
        {
            public uint ID;
            public uint NumberOfButtons;
            public uint SampleRate;
        }
 
        public struct DeviceInfoKeyboard
        {
            public uint Type;
            public uint SubType;
            public uint KeyboardMode;
            public uint NumberOfFunctionKeys;
            public uint NumberOfIndicators;
            public uint NumberOfKeysTotal;
            public bool IsUSBKeboard
            {
                get
                {
                    return this.Type == 81; // http://msdn.microsoft.com/en-us/library/windows/desktop/ms724336%28v=vs.85%29.aspx
                }
            }
        }
 
        public struct DeviceInfoHID
        {
            public uint VendorID;
            public uint ProductID;
            public uint VersionNumber;
            public ushort UsagePage;
            public ushort Usage;
        }
 
        [StructLayout(LayoutKind.Sequential)]
        public struct RawInputDeviceList
        {
            public IntPtr DeviceHandle;
            public RawInputDeviceType DeviceType;
        }
 
        [DllImport("User32.dll", SetLastError = true)]
        public static extern uint GetRawInputDeviceList(
            IntPtr pRawInputDeviceList,
            ref uint uiNumDevices,
            uint cbSize);
 
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetRawInputDeviceInfo(
            IntPtr hDevice,
            RawInputDeviceInfoCommand uiCommand,
            IntPtr data,
            ref uint size);
    }
}
