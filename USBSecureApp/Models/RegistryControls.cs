using Microsoft.Win32;
using System;
using USBSecureApp.Interfaces;

namespace USBSecureApp.Models
{
    public class RegistryControls : IRegistryControls
    {
        private string _regPath = "SOFTWARE\\USBSecure";

        public object GetRegistryValue(string regKey)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_regPath))
            {
                if (key != null)
                {
                    Object obj = key.GetValue(regKey);
                    if (obj != null)
                        return obj;
                }
                return null;
            }
        }

        public void SetRegistryValue<T>(T regValue, string regKey)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_regPath, true))
            {
                if (key != null)
                {
                    key.SetValue(regKey, regValue as object);
                }
            }
        }
    }
}
