﻿using System;
using System.Drawing;
using System.IO;
using System.Windows;
using TestSysTray.ViewModels;
using Forms = System.Windows.Forms; 
namespace TestSysTray
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// try to create a tray icon app for windows
	/// </summary>
	public partial class App
	{
		private readonly Forms.NotifyIcon _notifyIcon;

		public App()
		{
			_notifyIcon = new Forms.NotifyIcon();
		}
		
		protected override void OnStartup(StartupEventArgs e)
		{

			string path = Directory.GetCurrentDirectory();
			Console.WriteLine(path);
			
			_notifyIcon.Icon = new System.Drawing.Icon("Resources/icon16x24x32.ico");
			_notifyIcon.Text = "Toggle Touch";
			_notifyIcon.Click += NotifyIcon_Click;
			
			// creates icon right click context menu
			_notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
			_notifyIcon.ContextMenuStrip.Items.Add("Status", Image.FromFile("Resources/icon16x24x32.ico"), OnStatusClicked);
			_notifyIcon.Visible = true;
			 
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
			_notifyIcon.ShowBalloonTip(1000, "Toggle Touch", "Be sure to subscribe.", Forms.ToolTipIcon.Info);
			// MainWindow.WindowState = WindowState.Normal;
			// MainWindow.Activate();
		}

		private void ToggleTouchScreen()
		{
			// every type of device has a hard-coded GUID, this is the one for mice
			Guid mouseGuid = new Guid("{4d36e96f-e325-11ce-bfc1-08002be10318}");
			
			// get this from the properties dialog box of this device in Device Manager
			string instancePath = @"ACPI\PNP0F03\4&3688D3F&0";

			// DeviceHelper.SetDeviceEnabled(mouseGuid, instancePath, enable);
		}

		private void OnStatusClicked(object sender, EventArgs e)
		{
			MessageBox.Show("Application is running", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}