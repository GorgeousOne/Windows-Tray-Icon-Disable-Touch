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
		private TextBox _inputGUID;
		private TextBox _inputInstancePath;
		private Button _btnSave;
		
		public string GuidString { 
			get => _inputGUID.Text;
			set => _inputGUID.Text = value;
		}

		public string InstancePathString {
			get => _inputInstancePath.Text;
			set => _inputInstancePath.Text = value;
		} 
		
		public MainWindow(App app)
		{
			InitializeComponent();
			_app = app;
		}
		
		public void FindControlReferences()
		{
			_inputGUID = FindName("InputGuid") as TextBox;
			_inputInstancePath = FindName("InputInstancePath") as TextBox;
			_btnSave = FindName("BtnSave") as Button;
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

		private void BtnSave_OnClick(object sender, RoutedEventArgs e)
		{
			_app.SaveSettings();
			_btnSave.IsEnabled = false;
		}

		private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			_btnSave.IsEnabled = true;
		}
	}
}