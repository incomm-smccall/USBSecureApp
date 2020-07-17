using System.Collections.Generic;

namespace USBSecureApp.Interfaces
{
    public interface IUSBControls
    {
        Dictionary<string, string> DeviceList();
        void Add2StartMenu(bool startMenuStatus);
        void Add2StartUp(bool startupStatus);
    }
}
