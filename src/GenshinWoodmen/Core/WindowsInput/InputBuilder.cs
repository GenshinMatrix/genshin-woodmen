using System;
using System.Collections;
using System.Collections.Generic;

namespace GenshinWoodmen.Core
{
	internal class InputBuilder : IEnumerable<INPUT>, IEnumerable
	{
		public InputBuilder()
		{
			_inputList = new List<INPUT>();
		}
		
		public INPUT[] ToArray()
		{
			return _inputList.ToArray();
		}

		public IEnumerator<INPUT> GetEnumerator()
		{
			return _inputList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		// Token: 0x1700000A RID: 10
		public INPUT this[int position]
		{
			get
			{
				return _inputList[position];
			}
		}

		public static bool IsExtendedKey(VirtualKeyCode keyCode)
		{
			return keyCode == VirtualKeyCode.MENU || keyCode == VirtualKeyCode.LMENU || keyCode == VirtualKeyCode.RMENU || keyCode == VirtualKeyCode.CONTROL || keyCode == VirtualKeyCode.RCONTROL || keyCode == VirtualKeyCode.INSERT || keyCode == VirtualKeyCode.DELETE || keyCode == VirtualKeyCode.HOME || keyCode == VirtualKeyCode.END || keyCode == VirtualKeyCode.PRIOR || keyCode == VirtualKeyCode.NEXT || keyCode == VirtualKeyCode.RIGHT || keyCode == VirtualKeyCode.UP || keyCode == VirtualKeyCode.LEFT || keyCode == VirtualKeyCode.DOWN || keyCode == VirtualKeyCode.NUMLOCK || keyCode == VirtualKeyCode.CANCEL || keyCode == VirtualKeyCode.SNAPSHOT || keyCode == VirtualKeyCode.DIVIDE;
		}

		public InputBuilder AddKeyDown(VirtualKeyCode keyCode)
		{
			INPUT input = default(INPUT);
			input.Type = 1U;
			input.Data.Keyboard = new KEYBDINPUT
			{
				KeyCode = (ushort)keyCode,
				Scan = 0,
				Flags = IsExtendedKey(keyCode) ? 1U : 0U,
				Time = 0U,
				ExtraInfo = IntPtr.Zero
			};
			INPUT item = input;
			_inputList.Add(item);
			return this;
		}

		public InputBuilder AddKeyUp(VirtualKeyCode keyCode)
		{
			INPUT input = default;
			input.Type = 1U;
			input.Data.Keyboard = new KEYBDINPUT()
			{
				KeyCode = (ushort)keyCode,
				Scan = 0,
				Flags = IsExtendedKey(keyCode) ? 3U : 2U,
				Time = 0U,
				ExtraInfo = IntPtr.Zero
			};
			INPUT item = input;
			_inputList.Add(item);
			return this;
		}

		public InputBuilder AddKeyPress(VirtualKeyCode keyCode)
		{
			AddKeyDown(keyCode);
			AddKeyUp(keyCode);
			return this;
		}

		public InputBuilder AddCharacter(char character)
		{
			INPUT input = default(INPUT);
			input.Type = 1U;
			input.Data.Keyboard = new KEYBDINPUT
			{
				KeyCode = 0,
				Scan = character,
				Flags = 4U,
				Time = 0U,
				ExtraInfo = IntPtr.Zero
			};
			INPUT item = input;
			INPUT input2 = default;
			input2.Type = 1U;
			input2.Data.Keyboard = new KEYBDINPUT
			{
				KeyCode = 0,
				Scan = character,
				Flags = 6U,
				Time = 0U,
				ExtraInfo = IntPtr.Zero
			};
			INPUT item2 = input2;
			if ((character & '＀') == '')
			{
				item.Data.Keyboard.Flags = item.Data.Keyboard.Flags | 1U;
				item2.Data.Keyboard.Flags = item2.Data.Keyboard.Flags | 1U;
			}
			_inputList.Add(item);
			_inputList.Add(item2);
			return this;
		}

		public InputBuilder AddCharacters(IEnumerable<char> characters)
		{
			foreach (char character in characters)
			{
				AddCharacter(character);
			}
			return this;
		}

		public InputBuilder AddCharacters(string characters)
		{
			return AddCharacters(characters.ToCharArray());
		}

		public InputBuilder AddRelativeMouseMovement(int x, int y)
		{
			INPUT item = new()
			{
				Type = 0U
			};
			item.Data.Mouse.Flags = 1U;
			item.Data.Mouse.X = x;
			item.Data.Mouse.Y = y;
			_inputList.Add(item);
			return this;
		}

		public InputBuilder AddAbsoluteMouseMovement(int absoluteX, int absoluteY)
		{
			INPUT item = new()
			{
				Type = 0U
			};
			item.Data.Mouse.Flags = 32769U;
			item.Data.Mouse.X = absoluteX;
			item.Data.Mouse.Y = absoluteY;
			_inputList.Add(item);
			return this;
		}

		public InputBuilder AddAbsoluteMouseMovementOnVirtualDesktop(int absoluteX, int absoluteY)
		{
			INPUT item = new()
			{
				Type = 0U
			};
			item.Data.Mouse.Flags = 49153U;
			item.Data.Mouse.X = absoluteX;
			item.Data.Mouse.Y = absoluteY;
			_inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseButtonDown(MouseButton button)
		{
			INPUT item = new()
			{
				Type = 0U
			};
			item.Data.Mouse.Flags = (uint)ToMouseButtonDownFlag(button);
			this._inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseXButtonDown(int xButtonId)
		{
			INPUT item = new()
			{
				Type = 0U
			};
			item.Data.Mouse.Flags = 128U;
			item.Data.Mouse.MouseData = (uint)xButtonId;
			_inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseButtonUp(MouseButton button)
		{
			INPUT item = new()
			{
				Type = 0U
			};
			item.Data.Mouse.Flags = (uint)ToMouseButtonUpFlag(button);
			_inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseXButtonUp(int xButtonId)
		{
			INPUT item = new INPUT
			{
				Type = 0U
			};
			item.Data.Mouse.Flags = 256U;
			item.Data.Mouse.MouseData = (uint)xButtonId;
			_inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseButtonClick(MouseButton button)
		{
			return AddMouseButtonDown(button).AddMouseButtonUp(button);
		}

		public InputBuilder AddMouseXButtonClick(int xButtonId)
		{
			return AddMouseXButtonDown(xButtonId).AddMouseXButtonUp(xButtonId);
		}

		public InputBuilder AddMouseButtonDoubleClick(MouseButton button)
		{
			return AddMouseButtonClick(button).AddMouseButtonClick(button);
		}

		public InputBuilder AddMouseXButtonDoubleClick(int xButtonId)
		{
			return AddMouseXButtonClick(xButtonId).AddMouseXButtonClick(xButtonId);
		}

		public InputBuilder AddMouseVerticalWheelScroll(int scrollAmount)
		{
			INPUT item = new()
			{
				Type = 0U
			};
			item.Data.Mouse.Flags = 2048U;
			item.Data.Mouse.MouseData = (uint)scrollAmount;
			_inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseHorizontalWheelScroll(int scrollAmount)
		{
			INPUT item = new()
			{
				Type = 0U
			};
			item.Data.Mouse.Flags = 4096U;
			item.Data.Mouse.MouseData = (uint)scrollAmount;
			_inputList.Add(item);
			return this;
		}

		private static MouseFlag ToMouseButtonDownFlag(MouseButton button)
		{
			switch (button)
			{
			case MouseButton.LeftButton:
				return MouseFlag.LeftDown;
			case MouseButton.MiddleButton:
				return MouseFlag.MiddleDown;
			case MouseButton.RightButton:
				return MouseFlag.RightDown;
			default:
				return MouseFlag.LeftDown;
			}
		}

		private static MouseFlag ToMouseButtonUpFlag(MouseButton button)
		{
			switch (button)
			{
			case MouseButton.LeftButton:
				return MouseFlag.LeftUp;
			case MouseButton.MiddleButton:
				return MouseFlag.MiddleUp;
			case MouseButton.RightButton:
				return MouseFlag.RightUp;
			default:
				return MouseFlag.LeftUp;
			}
		}

		private readonly List<INPUT> _inputList;
	}
}
