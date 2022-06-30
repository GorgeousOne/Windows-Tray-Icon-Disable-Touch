using System;
using System.Drawing;
using System.IO;
using System.Windows;
using ToggleTouch.Lib;
using Forms = System.Windows.Forms; 
namespace ToggleTouch
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// try to create a tray icon app for windows
	/// </summary>
	public partial class App
	{
		private readonly Forms.NotifyIcon _notifyIcon;
		private bool _isTouchEnabled = true;

		private Icon _enabledIcon;
		private Icon _disabledIcon;
		
		public App()
		{
			_notifyIcon = new Forms.NotifyIcon();
			_enabledIcon = new Icon("Resources/touch-enabled.ico");
			_disabledIcon = new Icon("Resources/touch-disabled.ico");
		}
		
		protected override void OnStartup(StartupEventArgs e)
		{
			string path = Directory.GetCurrentDirectory();
			Console.WriteLine(path);

			_notifyIcon.Icon = _enabledIcon;
			_notifyIcon.Text = "Touch Enabled";
			_notifyIcon.Click += NotifyIcon_Click;
			
			// creates icon right click context menu
			_notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
			// _notifyIcon.ContextMenuStrip.Items.Add("Exit", Image.FromFile("Resources/icon16x24x32.ico"), OnExitClicked);
			_notifyIcon.ContextMenuStrip.Items.Add("Exit", null, OnExitClicked);
			_notifyIcon.Visible = true;
			
			// string platform = Environment.Is64BitProcess ? "x64" : "x86";
			// _notifyIcon.ShowBalloonTip(1000, "Toggle Touch", platform , Forms.ToolTipIcon.Info);

			// creates a button click listener for the window?
			// MainWindow = new MainWindow();
			// MainWindow.DataContext = new NotifyViewModel(_notifyIcon);
			// MainWindow.WindowState = WindowState.Minimized;
			// MainWindow.Show();
			base.OnStartup(e);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			_notifyIcon.Dispose();
			base.OnExit(e);
		}

		private void NotifyIcon_Click(object sender, EventArgs e)
		{
			Forms.MouseEventArgs mouseEventArgs = (Forms.MouseEventArgs)e;
			
			if (mouseEventArgs.Button == Forms.MouseButtons.Left)
			{
				ToggleTouchScreen();
			}
			
			// MainWindow.WindowState = WindowState.Normal;
			// MainWindow.Activate();
		}
		
		public void ToggleTouchScreen()
		{
			_isTouchEnabled = !_isTouchEnabled;
			_notifyIcon.Icon = _isTouchEnabled ? _enabledIcon : _disabledIcon;
			_notifyIcon.Text = _isTouchEnabled ? "Touch Enabled" : "Touch Disabled";

			// every type of device has a hard-coded GUID, this is the one for mice
			// Guid mouseGuid = new Guid("{4d36e96f-e325-11ce-bfc1-08002be10318}");

			//touch screen GUID
			Guid touchScreenGuid = new Guid("{745a17a0-74d3-11d0-b6fe-00a0c90f57da}");

			// get this from the properties dialog box of this device in Device Manager
			// under "details > property: device instance path"
			// (german) "Details > Eigenschaft: Geräteinstanzpfad"
			
			// string instancePath = @"ACPI\PNP0F03\4&3688D3F&0";
			string instancePath = @"HID\VID_056A&PID_50A0&COL01\6&22485568&3&0000";
			DeviceHelper.SetDeviceEnabled(touchScreenGuid, instancePath, _isTouchEnabled);
		}
		
		private void OnExitClicked(object sender, EventArgs e)
		{
			// WinForms app
			if (Forms.Application.MessageLoop) 
			{
				Forms.Application.Exit();
			}
			// Console app
			else {
				Environment.Exit(1);
			}
		}
	}
}