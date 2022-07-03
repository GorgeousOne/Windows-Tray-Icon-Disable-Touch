using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToggleTouch.CustomControls
{
	/// <summary>
	/// A text box that makes it easier to enter GUIDs in the correct format.
	/// </summary>
	public class GuidBox : TextBox
	{
		private static readonly int InputPlaces = 32;
		private static readonly int[] MaskSeparatorIndices = {9, 14, 19, 24};
		private static readonly SortedDictionary<int, char> FixedMaskIndices = new SortedDictionary<int, char>
			{
				{ 0, '{' },
				{ 9, '-'},
				{14, '-'},
				{19, '-'},
				{24, '-'},
				{37, '}'},
			};
		
		private static readonly Regex NotAllowedChars = new Regex(@"[^0-9a-z]");
		private static readonly Regex Whitespace = new Regex(@"\s");
		
		private bool _silenceTextChange;
		private bool _silenceCaretMove;
		private bool _isFirstUpdate = true;
		private bool _correctInput;
		
		
		/// <summary>
		/// Removes invalid characters and keeps text mask in shape. Also moves caret past "-" separators while typing.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			if (_silenceTextChange)
			{
				_silenceTextChange = false;
				return;
			}
			int oldCaretIndex = CaretIndex;
			Console.WriteLine($"change ({CaretIndex}) " + Text);

			//skips next text change event unless it is on program start, where no 2nd event is fired
			_silenceTextChange = !_isFirstUpdate;
			Text = PasteInputToGuidMask(Text);
			CaretIndex = oldCaretIndex;
			_isFirstUpdate = false;
			
			if (_correctInput)
			{
				UpdateCaret();
				_correctInput = false;
			}
		}

		/// <summary>
		/// Moves caret to first free place unless the text is being marked/seleceted.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSelectionChanged(RoutedEventArgs e)
		{
			if (SelectedText != String.Empty)
			{
				return;
			}
			if (_silenceCaretMove)
			{
				_silenceCaretMove = false;
				return;
			}
			_silenceCaretMove = true;
			CaretIndex = Math.Min(CaretIndex, GetMaxCaretPos());
		}

		/// <summary>
		/// Moves caret past "-" separators.
		/// </summary>
		private void UpdateCaret()
		{
			if (Array.IndexOf(MaskSeparatorIndices, CaretIndex - 1) != -1)
			{
				_silenceCaretMove = true;
				CaretIndex += 1;
			}
		}
		
		/// <summary>
		/// Sets flag is a valid char was typed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			_correctInput = !NotAllowedChars.Match(e.Key.ToString().ToLower()).Success;
		}

		/// <summary>
		/// Shapes input text into GUID mask
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private string PasteInputToGuidMask(string input)
		{
			string digits = ExtractGuidChars(input);
			string exactInput = RightPadTrim(digits, InputPlaces);
			
			foreach(KeyValuePair<int, char> entry in FixedMaskIndices)
			{
				Console.WriteLine(entry.Key);
				exactInput = exactInput.Insert(entry.Key, entry.Value.ToString());
			}
			return exactInput;
		}

		private string ExtractGuidChars(string input)
		{
			return NotAllowedChars.Replace(input.ToLower(), string.Empty);
		}

		private string RightPadTrim(string input, int exactLength)
		{
			if (input.Length > exactLength)
			{
				return input.Substring(0, exactLength);
			}
			return input.PadRight(exactLength);
		}
		
		private int GetMaxCaretPos()
		{
			Match match = Whitespace.Match(Text);
			if (match.Success)
			{
				return match.Index;
			}
			return InputPlaces + FixedMaskIndices.Count;
		}
	}
}