using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                d => d.DeviceType == Win32.RawInputDeviceType.Keyboard && d.DeviceInfo.KeyboardInfo.IsSingleUSBKeboard))
            {
                Console.WriteLine(
                    "{0}:\n\tName = {1}\n\tHandle: = {2}\n",
                    rawInputDevice.DeviceType,
                    rawInputDevice.DeviceName,
                    rawInputDevice.DeviceHandle);
            }

            Console.WriteLine("Is Single USB Keyboard Present: " + rawDeviceEnumerator.IsSingleUSBKeyboardPresent);
        }
    }
}
