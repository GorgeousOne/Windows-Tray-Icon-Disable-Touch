using System;
using System.Drawing;
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

		private MainWindow _main;
		
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
			_main = new MainWindow(this);
			MainWindow = _main;
			
			MainWindow.Show();
			_main.FindControlReferences();
			
			LoadSettings();
			base.OnStartup(e);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			SaveSettings();
			_notifyIcon.Dispose();
			base.OnExit(e);
		}

		private void LoadSettings() {
			ToggleTouch.Properties.Settings.Default.Reload();
			_main.GuidString = ToggleTouch.Properties.Settings.Default.DeviceGUID;
			_main.InstancePathString = ToggleTouch.Properties.Settings.Default.DeviceInstancePath;
		}
		
		public void SaveSettings()
        {
			ToggleTouch.Properties.Settings.Default.DeviceGUID = _main.GuidString;
			ToggleTouch.Properties.Settings.Default.DeviceInstancePath = _main.InstancePathString;
			ToggleTouch.Properties.Settings.Default.Save();
			ToggleTouch.Properties.Settings.Default.Reload();
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
			//touch screen GUID
			Guid touchScreenGuid;
			try
			{
				touchScreenGuid = new Guid(_main.GuidString);
			}
			catch (Exception e)
			{
				OpenConfig();
				MessageBox.Show("Please enter a valid GUID of your touch screen", "Toggle Touch", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			//touch screen instance path
			string instancePath = _main.InstancePathString;
			
			if (DeviceHelper.SetDeviceEnabled(touchScreenGuid, instancePath, !_isTouchEnabled, HandleException)) {
				_isTouchEnabled = !_isTouchEnabled;
				_notifyIcon.Icon = _isTouchEnabled ? _enabledIcon : _disabledIcon;
				_notifyIcon.Text = _isTouchEnabled ? "Touch ON" : "Touch OFF";
				_notifyIcon.ShowBalloonTip(1000, null, "Touch turned: " + (_isTouchEnabled ? "ON" : "OFF"), Forms.ToolTipIcon.None);
			}
		}

		private void HandleException(Exception e)
		{
			OpenConfig();
			MessageBox.Show("An error occured while toggling the touch screen: " + e, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		private void OnConfigureClicked(object sender, EventArgs e)
		{
			OpenConfig();
		}

		private void OpenConfig()
		{
			MainWindow.Show();
			MainWindow.WindowState = WindowState.Normal;
		}
		
		private void OnExitClicked(object sender, EventArgs e)
		{
			if (Forms.Application.MessageLoop) 
			{
				// WinForms app
				Forms.Application.Exit();
			}
			else 
			{
				// Console app
				Environment.Exit(1);
			}
		}
	}
}