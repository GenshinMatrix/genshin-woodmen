using System;
using System.Collections.Generic;
using System.Threading;

namespace GenshinWoodmen.Core
{
	public class KeyboardSimulator : IKeyboardSimulator
	{
		public KeyboardSimulator(IInputSimulator inputSimulator)
		{
			if (inputSimulator == null)
			{
				throw new ArgumentNullException("inputSimulator");
			}
			_inputSimulator = inputSimulator;
			_messageDispatcher = new WindowsInputMessageDispatcher();
		}

		internal KeyboardSimulator(IInputSimulator inputSimulator, IInputMessageDispatcher messageDispatcher)
		{
			if (inputSimulator == null)
			{
				throw new ArgumentNullException("inputSimulator");
			}
			if (messageDispatcher == null)
			{
				throw new InvalidOperationException(string.Format("The {0} cannot operate with a null {1}. Please provide a valid {1} instance to use for dispatching {2} messages.", typeof(KeyboardSimulator).Name, typeof(IInputMessageDispatcher).Name, typeof(INPUT).Name));
			}
			_inputSimulator = inputSimulator;
			_messageDispatcher = messageDispatcher;
		}

		public IMouseSimulator Mouse => _inputSimulator.Mouse;

        private void ModifiersDown(InputBuilder builder, IEnumerable<VirtualKeyCode> modifierKeyCodes)
		{
			if (modifierKeyCodes == null)
			{
				return;
			}
			foreach (VirtualKeyCode keyCode in modifierKeyCodes)
			{
				builder.AddKeyDown(keyCode);
			}
		}

		private void ModifiersUp(InputBuilder builder, IEnumerable<VirtualKeyCode> modifierKeyCodes)
		{
			if (modifierKeyCodes == null)
			{
				return;
			}
			Stack<VirtualKeyCode> stack = new(modifierKeyCodes);
			while (stack.Count > 0)
			{
				builder.AddKeyUp(stack.Pop());
			}
		}

		private void KeysPress(InputBuilder builder, IEnumerable<VirtualKeyCode> keyCodes)
		{
			if (keyCodes == null)
			{
				return;
			}
			foreach (VirtualKeyCode keyCode in keyCodes)
			{
				builder.AddKeyPress(keyCode);
			}
		}

		private void SendSimulatedInput(INPUT[] inputList)
		{
			_messageDispatcher.DispatchInput(inputList);
		}

		public IKeyboardSimulator KeyDown(VirtualKeyCode keyCode)
		{
			INPUT[] inputList = new InputBuilder().AddKeyDown(keyCode).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IKeyboardSimulator KeyUp(VirtualKeyCode keyCode)
		{
			INPUT[] inputList = new InputBuilder().AddKeyUp(keyCode).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IKeyboardSimulator KeyPress(VirtualKeyCode keyCode)
		{
			INPUT[] inputList = new InputBuilder().AddKeyPress(keyCode).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IKeyboardSimulator KeyPress(params VirtualKeyCode[] keyCodes)
		{
			InputBuilder inputBuilder = new InputBuilder();
			KeysPress(inputBuilder, keyCodes);
			SendSimulatedInput(inputBuilder.ToArray());
			return this;
		}

		public IKeyboardSimulator ModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
		{
			ModifiedKeyStroke(new VirtualKeyCode[]
			{
				modifierKeyCode
			}, new VirtualKeyCode[]
			{
				keyCode
			});
			return this;
		}

		public IKeyboardSimulator ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
		{
			ModifiedKeyStroke(modifierKeyCodes, new VirtualKeyCode[]
			{
				keyCode
			});
			return this;
		}

		public IKeyboardSimulator ModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
		{
			ModifiedKeyStroke(new VirtualKeyCode[]
			{
				modifierKey
			}, keyCodes);
			return this;
		}

		public IKeyboardSimulator ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
		{
			InputBuilder inputBuilder = new();
			ModifiersDown(inputBuilder, modifierKeyCodes);
			KeysPress(inputBuilder, keyCodes);
			ModifiersUp(inputBuilder, modifierKeyCodes);
			SendSimulatedInput(inputBuilder.ToArray());
			return this;
		}

		public IKeyboardSimulator TextEntry(string text)
		{
			if (text.Length > 2147483647L)
			{
				throw new ArgumentException(string.Format("The text parameter is too long. It must be less than {0} characters.", 2147483647U), "text");
			}
			INPUT[] inputList = new InputBuilder().AddCharacters(text).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IKeyboardSimulator TextEntry(char character)
		{
			INPUT[] inputList = new InputBuilder().AddCharacter(character).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IKeyboardSimulator Sleep(int millsecondsTimeout)
		{
			Thread.Sleep(millsecondsTimeout);
			return this;
		}

		public IKeyboardSimulator Sleep(TimeSpan timeout)
		{
			Thread.Sleep(timeout);
			return this;
		}

		private readonly IInputSimulator _inputSimulator;

		private readonly IInputMessageDispatcher _messageDispatcher;
	}
}
