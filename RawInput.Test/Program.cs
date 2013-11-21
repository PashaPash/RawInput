using System;
using System.Linq;

namespace RawInput.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var rawDeviceEnumerator = new RawInputDeviceEnumerator();

            // enumerates raw input devices of Keyboard type
            foreach (var rawInputDevice in
                rawDeviceEnumerator.Devices.Where(
                d => d.DeviceType == Win32.RawInputDeviceType.Keyboard && d.DeviceInfo.KeyboardInfo.IsUSBKeboard))
            {
                Console.WriteLine(
                    "{0}:\n\tName = {1}\n\tHandle: = {2}\n",
                    rawInputDevice.DeviceType,
                    rawInputDevice.DeviceName,
                    rawInputDevice.DeviceHandle);
            }

            Console.WriteLine("Usb Keyboards Count: " + rawDeviceEnumerator.UsbKeyboardsCount);
        }
    }
}
