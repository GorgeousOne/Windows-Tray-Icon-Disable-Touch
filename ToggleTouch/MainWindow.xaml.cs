using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
		}
		
		public void FindControlReferences()
		{
			_inputGuid = FindName("InputGuid") as TextBox;
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

		private bool _skipChange;
		
		private void InputGuid_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (_skipChange)
			{
				_skipChange = false;
				return;
			}
			int oldCaretIndex = _inputGuid.CaretIndex;
			_skipChange = true;
			_btnSave.IsEnabled = true;
			_inputGuid.Text = PasteInputToGuidMask(_inputGuid.Text);
			_inputGuid.CaretIndex = oldCaretIndex;
		}
		
		private void InputInstancePath_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			_btnSave.IsEnabled = true;
		}
		
		private string PasteInputToGuidMask(string input)
		{
			string digits = ExtractGuidChars(input);
			string exactInput = RightPadTrim(digits, 32);
			return exactInput
				.Insert(32, "}")
				.Insert(24, "-")
				.Insert(20, "-")
				.Insert(16, "-")
				.Insert(0, "{");
		}

		private string ExtractGuidChars(string input)
		{
			return new Regex(@"[^a-z0-9]").Replace(input.ToLower(), string.Empty);
		}

		private string RightPadTrim(string input, int exactLength)
		{
			if (input.Length > exactLength)
			{
				return input.Substring(0, exactLength);
			}
			return input.PadRight(exactLength);
		}
	}
}