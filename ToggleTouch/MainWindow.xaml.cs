using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ToggleTouch
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow {

		private TextBox _inputGUID;
		private TextBox _inputInstancePath;
		
		public string GuidString { 
			get => _inputGUID.Text;
			set => _inputGUID.Text = value;
		}

		public string InstancePathString {
			get => _inputInstancePath.Text;
			set => _inputInstancePath.Text = value;
		} 
		
		public MainWindow()
		{
			InitializeComponent();
			_inputGUID = FindName("InputGuid") as TextBox;
			_inputInstancePath = FindName("InputInstancePath") as TextBox;
		}
		
		protected override void OnStateChanged(EventArgs e)
		{
			base.OnStateChanged(e);
			
			if (WindowState == WindowState.Minimized)
			{
				Hide();
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = true;
			Hide();
			base.OnClosing(e);
		}
	}
}