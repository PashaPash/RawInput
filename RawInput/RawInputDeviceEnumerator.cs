using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace RawInput
{
    public class RawInputDeviceEnumerator
    {
        private readonly RawInputDevice[] _devices;

        public IEnumerable<RawInputDevice> Devices
        {
            get
            {
                return this._devices;
            }
        }

        public RawInputDeviceEnumerator()
        {
            uint deviceCount = 0;
            var deviceSize =
                (uint)Marshal.SizeOf(typeof(Win32.RawInputDeviceList));

            // first call retrieves the number of raw input devices
            var result = Win32.GetRawInputDeviceList(
                IntPtr.Zero,
                ref deviceCount,
                deviceSize);

            _devices = new RawInputDevice[deviceCount];

            if ((int)result == -1 || deviceCount == 0)
            {
                // call failed, or no devices found
                return;
            }

            // allocates memory for an array of Win32.RawInputDeviceList
            IntPtr ptrDeviceList =
              Marshal.AllocHGlobal((int)(deviceSize * deviceCount));

            result = Win32.GetRawInputDeviceList(
                ptrDeviceList,
                ref deviceCount,
                deviceSize);

            if ((int)result != -1)
            {
                // enumerates array of Win32.RawInputDeviceList,
                // and populates array of managed RawInputDevice objects
                for (var index = 0; index < deviceCount; index++)
                {
                    var rawInputDeviceList =
                        (Win32.RawInputDeviceList)Marshal.PtrToStructure(
                        new IntPtr((ptrDeviceList.ToInt32() +
                            (deviceSize * index))),
                        typeof(Win32.RawInputDeviceList));

                    _devices[index] =
                        new RawInputDevice(rawInputDeviceList);
                }
            }

            Marshal.FreeHGlobal(ptrDeviceList);
        }

        public int UsbKeyboardsCount
        {
            get
            {
                return this.Devices.Count(d => d.DeviceType == Win32.RawInputDeviceType.Keyboard && d.DeviceInfo.KeyboardInfo.IsUSBKeboard);
            }
        }

    }
}
