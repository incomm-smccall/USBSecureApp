using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBSecureService
{
    public static class RegistryControls
    {
        private static readonly string regPath = "SOFTWARE\\USBSecure";
        public static object GetRegistryValue(string regKey)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regPath))
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

        public static void SetRegistryValue<T>(T regValue, string regKey)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regPath, true))
            {
                if (key != null)
                {
                    key.SetValue(regKey, regValue as object);
                }
            }
        }
    }
}
