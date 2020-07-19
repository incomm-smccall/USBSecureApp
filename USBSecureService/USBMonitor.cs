using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace USBSecureService
{
    public static class USBMonitor
    {
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        public static bool LookForUSB()
        {
            string usbSerialNumber = RegistryControls.GetRegistryValue("SerialNumber") as string;
            Dictionary<string, string> usbList = USBControls.DeviceList();
            if (usbList.Select(x => x.Value).Any(x => x.Contains(usbSerialNumber)))
            {
                return true;
            }
            return false;
        }

        public static void StartUSBMonitor()
        {
            while (true)
            {
                if (!USBMonitor.LookForUSB() && Convert.ToBoolean(RegistryControls.GetRegistryValue("IgnoreUSB")))
                {
                    LockWorkStation();
                }
                Thread.Sleep(3000);
            }
        }
    }
}
