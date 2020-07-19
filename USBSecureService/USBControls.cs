using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace USBSecureService
{
    public static class USBControls
    {
        public static Dictionary<string, string> DeviceList()
        {
            Dictionary<string, string> usbs = new Dictionary<string, string>();
            ManagementObjectSearcher search = new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'");
            foreach (ManagementObject usbObj in search.Get())
            {
                ManagementObject usb = new ManagementObject("Win32_PhysicalMedia.Tag='" + usbObj["DeviceID"] + "'");
                usbs.Add(usbObj["Model"].ToString(), usb["SerialNumber"].ToString());
            }
            return usbs;
        }
    }
}
