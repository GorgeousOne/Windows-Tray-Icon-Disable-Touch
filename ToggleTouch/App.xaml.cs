using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using ToggleTouch.Lib;
using ToggleTouch.ViewModels;
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
			_notifyIcon.Icon = _enabledIcon;
			_notifyIcon.Text = "Touch Enabled";
			_notifyIcon.Click += NotifyIcon_Click;
			
			// creates icon right click context menu
			_notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
			_notifyIcon.ContextMenuStrip.Items.Add("Configure", null, OnConfigureClicked);
			_notifyIcon.ContextMenuStrip.Items.Add("Exit", null, OnExitClicked);
			_notifyIcon.Visible = true;

			// creates a button click listener for the window?
			MainWindow = new MainWindow();
			MainWindow.DataContext = new NotifyViewModel(_notifyIcon);
			
			MainWindow.Show();
			base.OnStartup(e);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			_notifyIcon.Dispose();
			base.OnExit(e);
		}
		
		private void NotifyIcon_Click(object sender, EventArgs e)
		{
			Forms.MouseEventArgs eventArgs = (Forms.MouseEventArgs)e;
			
			if (eventArgs.Button == Forms.MouseButtons.Left)
			{
				ToggleTouchScreen();
			}
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
		
		private void OnConfigureClicked(object sender, EventArgs e)
		{
			MainWindow.Show();
			MainWindow.WindowState = WindowState.Normal;
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