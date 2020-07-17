namespace USBSecureApp.Interfaces
{
    public interface IRegistryControls
    {
        object GetRegistryValue(string value);
        void SetRegistryValue<T>(T regValue, string regKey);
    }
}
