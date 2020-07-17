using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using USBSecureApp.Interfaces;
using USBSecureApp.Models;
using Windows.Devices.Enumeration;

namespace USBSecureApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        private DeviceWatcher _watcher;
        private List<DeviceInformation> interfaces = new List<DeviceInformation>();
        private bool isEnumerationComplete = false;
        private IUSBControls _usbControls;
        private IRegistryControls _regControls;

        public ICommand ChangeUSB { get; set; }

        private string _usbSerialNumber;
        public string UsbSerialNumber
        {
            get { return _usbSerialNumber; }
            set
            {
                if (_usbSerialNumber != value)
                {
                    _usbSerialNumber = value;
                    OnPropertyChanged("UsbSerialNumber");
                }
            }
        }

        private string _isUSBPluggedIn;
        public string IsUSBPluggedIn
        {
            get { return _isUSBPluggedIn; }
            set
            {
                if (_isUSBPluggedIn != value)
                {
                    _isUSBPluggedIn = value;
                    OnPropertyChanged("IsUSBPluggedIn");
                }
            }
        }

        private bool _ignoreUSB;
        public bool IgnoreUSB
        {
            get { return _ignoreUSB; }
            set
            {
                if (_ignoreUSB != value)
                {
                    _ignoreUSB = value;
                    _regControls.SetRegistryValue<bool>(_ignoreUSB, "IgnoreUSB");
                    OnPropertyChanged("IgnoreUSB");
                }
            }
        }

        private bool _addToStartup;
        public bool AddToStartup
        {
            get { return _addToStartup; }
            set
            {
                if (_addToStartup != value)
                {
                    _addToStartup = value;
                    _usbControls.Add2StartUp(_addToStartup);
                    _regControls.SetRegistryValue(_addToStartup, "AddedToStartup");
                    OnPropertyChanged("AddToStartup");
                }
            }
        }

        private bool _addToStartMenu;
        public bool AddToStartMenu
        {
            get { return _addToStartMenu; }
            set
            {
                if (_addToStartMenu != value)
                {
                    _addToStartMenu = value;
                    _usbControls.Add2StartMenu(_addToStartMenu);
                    _regControls.SetRegistryValue(_addToStartMenu, "AddedToStartMenu");
                    OnPropertyChanged("AddToStartMenu");
                }
            }
        }

        private ObservableCollection<string> _usbList;
        public ObservableCollection<string> USBList
        {
            get { return _usbList; }
            set
            {
                if (_usbList != value)
                {
                    _usbList = value;
                    OnPropertyChanged("USBList");
                }
            }
        }

        private string _selectedUSB;
        public string SelectedUSB
        {
            get { return _selectedUSB; }
            set
            {
                if (_selectedUSB != value)
                {
                    _selectedUSB = value;
                    OnPropertyChanged("SelectedUSB");
                }
            }
        }

        public MainWindowViewModel()
        {
            _regControls = new RegistryControls();
            _usbControls = new USBControls();
            UsbSerialNumber = _regControls.GetRegistryValue("SerialNumber") as string;
            IgnoreUSB = Convert.ToBoolean(_regControls.GetRegistryValue("IgnoreUSB"));
            AddToStartup = Convert.ToBoolean(_regControls.GetRegistryValue("AddedToStartup"));
            AddToStartMenu = Convert.ToBoolean(_regControls.GetRegistryValue("AddedToStartMenu"));

            ChangeUSB = new RelayCommand(ChangeUSBDevice);
            USBList = new ObservableCollection<string>();
            GetUSBList();

            try
            {
                _watcher = DeviceInformation.CreateWatcher();
                _watcher.Added += Watcher_Added;
                _watcher.Removed += Watcher_Removed;
                _watcher.Updated += Watcher_Updated;
                _watcher.EnumerationCompleted += Watcher_EnumerationCompleted;
                _watcher.Stopped += Watcher_Stopped;
                _watcher.Start();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            Task.Factory.StartNew(() => StartUSBMonitor());
        }
        
        private void StartUSBMonitor()
        {
            while (true)
            {
                if (!LookForUSB() && !IgnoreUSB)
                {
                    LockWorkStation();
                }
                Thread.Sleep(3000);
            }
        }
        
        private void Watcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            if (isEnumerationComplete)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    GetUSBList();
                });
            }
        }

        private void Watcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                GetUSBList();
            });
        }

        private void Watcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                GetUSBList();
            });
        }

        private void Watcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            isEnumerationComplete = true;
        }

        private void Watcher_Stopped(DeviceWatcher sender, object args)
        {
            try
            {
                if (_watcher.Status == DeviceWatcherStatus.Stopped)
                {
                    
                }
                else
                    _watcher.Stop();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        private bool LookForUSB()
        {
            bool retIsPresent = false;
            Dictionary<string, string> usbList = _usbControls.DeviceList();
            foreach (KeyValuePair<string, string> usb in usbList)
            {
                if (usb.Value == UsbSerialNumber)
                {
                    IsUSBPluggedIn = "Present";
                    retIsPresent = true;
                }
            }
            if (!retIsPresent)
                IsUSBPluggedIn = "";

            return retIsPresent;
        }

        private void ChangeUSBDevice(object obj)
        {
            string serialNum = SelectedUSB.Split('-')[1].Trim();
            UsbSerialNumber = serialNum;
            _regControls.SetRegistryValue<string>(UsbSerialNumber, "SerialNumber");
        }

        private void GetUSBList()
        {
            USBList.Clear();
            Dictionary<string, string> usbList = _usbControls.DeviceList();
            foreach(KeyValuePair<string, string> usb in usbList)
            {
                string uObj = $"{usb.Key} - {usb.Value}";
                USBList.Add(uObj);
            }
        }
    }
}
