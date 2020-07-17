using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using USBSecureApp.Interfaces;
using IWshRuntimeLibrary;

namespace USBSecureApp.Models
{
    public class USBControls : IUSBControls
    {
        public Dictionary<string, string> DeviceList()
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

        public void Add2StartMenu(bool startMenuStatus)
        {
            string startMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "USBSecure.lnk");
            if (startMenuStatus)
            {
                string shortCutTo = System.Reflection.Assembly.GetExecutingAssembly().Location;

                var wsh = new IWshShell_Class();
                IWshShortcut shortCut = wsh.CreateShortcut(startMenuPath) as IWshShortcut;
                shortCut.TargetPath = shortCutTo;
                shortCut.WorkingDirectory = Directory.GetParent(shortCutTo).FullName;
                shortCut.Save();
            }
            else
            {
                if (System.IO.File.Exists(startMenuPath))
                {
                    System.IO.File.Delete(startMenuPath);
                }
            }
        }

        public void Add2StartUp(bool startupStatus)
        {
            string startupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "USBSecure.lnk");
            if (startupStatus)
            {
                string shortCutTo = System.Reflection.Assembly.GetExecutingAssembly().Location;

                var wsh = new IWshShell_Class();
                IWshShortcut shortCut = wsh.CreateShortcut(startupPath) as IWshShortcut;
                shortCut.TargetPath = shortCutTo;
                shortCut.WorkingDirectory = Directory.GetParent(shortCutTo).FullName;
                shortCut.Save();
            }
            else
            {
                if (System.IO.File.Exists(startupPath))
                {
                    System.IO.File.Delete(startupPath);
                }
            }
        }
    }
}
