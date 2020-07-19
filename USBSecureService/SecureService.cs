using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace USBSecureService
{
    public partial class SecureService : ServiceBase
    {
        private ManagementEventWatcher watcher = null;
        private Dictionary<string, string> deviceList = new Dictionary<string, string>();

        public SecureService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            deviceList = USBControls.DeviceList();
            AddRemoveUSBHandler();
            AddInsertUSBHandle();
            Task.Factory.StartNew(() => USBMonitor.StartUSBMonitor());
        }

        protected override void OnStop()
        {
        }

        private void AddInsertUSBHandle()
        {
            WqlEventQuery query;
            ManagementScope scope = new ManagementScope("root\\CIMV2");
            scope.Options.EnablePrivileges = true;
            try
            {
                query = new WqlEventQuery();
                query.EventClassName = "__InstanceDeletionEvent";
                query.WithinInterval = new TimeSpan(0, 0, 3);
                query.Condition = @"TargetInstance ISA 'Win32_USBHub'";
                watcher = new ManagementEventWatcher(scope, query);
                watcher.EventArrived += new EventArrivedEventHandler(USBControl);
                watcher.Start();
            }
            catch (Exception)
            {

            }
        }

        private void AddRemoveUSBHandler()
        {
            WqlEventQuery query;
            ManagementScope scope = new ManagementScope("root\\CIMV2");
            scope.Options.EnablePrivileges = true;
            try
            {
                query = new WqlEventQuery();
                query.EventClassName = "__InstanceDeletionEvent";
                query.WithinInterval = new TimeSpan(0, 0, 3);
                query.Condition = @"TargetInstance ISA 'Win32_USBHub'";
                watcher = new ManagementEventWatcher(scope, query);
                watcher.EventArrived += new EventArrivedEventHandler(USBControl);
                watcher.Start();
            }
            catch (Exception)
            {

            }
        }

        private void USBControl(object sender, EventArgs e)
        {
            deviceList = USBControls.DeviceList();
        }
    }
}
