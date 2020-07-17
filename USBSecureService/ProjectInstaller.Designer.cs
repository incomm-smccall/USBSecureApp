namespace USBSecureService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.USBSecureProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.UsbServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // USBSecureProcessInstaller
            // 
            this.USBSecureProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.USBSecureProcessInstaller.Password = null;
            this.USBSecureProcessInstaller.Username = null;
            // 
            // UsbServiceInstaller
            // 
            this.UsbServiceInstaller.Description = "Monitor USB Presente";
            this.UsbServiceInstaller.DisplayName = "USBSecureService";
            this.UsbServiceInstaller.ServiceName = "USBSecure";
            this.UsbServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.UsbServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.UsbServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.USBSecureProcessInstaller,
            this.UsbServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller USBSecureProcessInstaller;
        private System.ServiceProcess.ServiceInstaller UsbServiceInstaller;
    }
}