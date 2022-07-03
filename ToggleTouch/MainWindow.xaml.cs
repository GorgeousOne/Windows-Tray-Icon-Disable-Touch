using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ToggleTouch
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private App _app;
		private TextBox _inputGuid;
		private TextBox _inputInstancePath;
		private Button _btnSave;
		
		public string GuidString { 
			get => _inputGuid.Text;
			set => _inputGuid.Text = value;
		}

		public string InstancePathString {
			get => _inputInstancePath.Text;
			set => _inputInstancePath.Text = value;
		} 
		
		public MainWindow(App app)
		{
			InitializeComponent();
			_app = app;
			FindControlReferences();
		}
		
		public void FindControlReferences()
		{
			_inputGuid = FindName("InputGuid") as TextBox;
			_inputInstancePath = FindName("InputInstancePath") as TextBox;
			_btnSave = FindName("BtnSave") as Button;
		}
		
		/// <summary>
		/// Hides application to tray on minimizing
		/// </summary>
		/// <param name="e"></param>
		protected override void OnStateChanged(EventArgs e)
		{
			base.OnStateChanged(e);
			
			if (WindowState == WindowState.Minimized)
			{
				Hide();
			}
		}

		/// <summary>
		/// Hides application to tray instead of exiting
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = true;
			Hide();
			base.OnClosing(e);
		}

		private void BtnSave_OnClick(object sender, RoutedEventArgs e)
		{
			_app.SaveSettings();
			_btnSave.IsEnabled = false;
		}

		/// <summary>
		/// Enables save button when input fields are being changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InputInstancePath_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			_btnSave.IsEnabled = true;
		}
	}
}