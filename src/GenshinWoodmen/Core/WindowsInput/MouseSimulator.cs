using System;
using System.Threading;

namespace GenshinWoodmen.Core
{
	public class MouseSimulator : IMouseSimulator
	{
		public MouseSimulator(IInputSimulator inputSimulator)
		{
			if (inputSimulator == null)
			{
				throw new ArgumentNullException("inputSimulator");
			}
			_inputSimulator = inputSimulator;
			_messageDispatcher = new WindowsInputMessageDispatcher();
		}

		internal MouseSimulator(IInputSimulator inputSimulator, IInputMessageDispatcher messageDispatcher)
		{
			if (inputSimulator == null)
			{
				throw new ArgumentNullException("inputSimulator");
			}
			if (messageDispatcher == null)
			{
				throw new InvalidOperationException(string.Format("The {0} cannot operate with a null {1}. Please provide a valid {1} instance to use for dispatching {2} messages.", typeof(MouseSimulator).Name, typeof(IInputMessageDispatcher).Name, typeof(INPUT).Name));
			}
			_inputSimulator = inputSimulator;
			_messageDispatcher = messageDispatcher;
		}

		public IKeyboardSimulator Keyboard => _inputSimulator.Keyboard;

        private void SendSimulatedInput(INPUT[] inputList)
		{
			_messageDispatcher.DispatchInput(inputList);
		}

		public IMouseSimulator MoveMouseBy(int pixelDeltaX, int pixelDeltaY)
		{
			INPUT[] inputList = new InputBuilder().AddRelativeMouseMovement(pixelDeltaX, pixelDeltaY).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator MoveMouseTo(double absoluteX, double absoluteY)
		{
			INPUT[] inputList = new InputBuilder().AddAbsoluteMouseMovement((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator MoveMouseToPositionOnVirtualDesktop(double absoluteX, double absoluteY)
		{
			INPUT[] inputList = new InputBuilder().AddAbsoluteMouseMovementOnVirtualDesktop((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator LeftButtonDown()
		{
			INPUT[] inputList = new InputBuilder().AddMouseButtonDown(MouseButton.LeftButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator LeftButtonUp()
		{
			INPUT[] inputList = new InputBuilder().AddMouseButtonUp(MouseButton.LeftButton).ToArray();
			this.SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator LeftButtonClick()
		{
			INPUT[] inputList = new InputBuilder().AddMouseButtonClick(MouseButton.LeftButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator LeftButtonDoubleClick()
		{
			INPUT[] inputList = new InputBuilder().AddMouseButtonDoubleClick(MouseButton.LeftButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator RightButtonDown()
		{
			INPUT[] inputList = new InputBuilder().AddMouseButtonDown(MouseButton.RightButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator RightButtonUp()
		{
			INPUT[] inputList = new InputBuilder().AddMouseButtonUp(MouseButton.RightButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator RightButtonClick()
		{
			INPUT[] inputList = new InputBuilder().AddMouseButtonClick(MouseButton.RightButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator RightButtonDoubleClick()
		{
			INPUT[] inputList = new InputBuilder().AddMouseButtonDoubleClick(MouseButton.RightButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator XButtonDown(int buttonId)
		{
			INPUT[] inputList = new InputBuilder().AddMouseXButtonDown(buttonId).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator XButtonUp(int buttonId)
		{
			INPUT[] inputList = new InputBuilder().AddMouseXButtonUp(buttonId).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator XButtonClick(int buttonId)
		{
			INPUT[] inputList = new InputBuilder().AddMouseXButtonClick(buttonId).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator XButtonDoubleClick(int buttonId)
		{
			INPUT[] inputList = new InputBuilder().AddMouseXButtonDoubleClick(buttonId).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator VerticalScroll(int scrollAmountInClicks)
		{
			INPUT[] inputList = new InputBuilder().AddMouseVerticalWheelScroll(scrollAmountInClicks * 120).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator HorizontalScroll(int scrollAmountInClicks)
		{
			INPUT[] inputList = new InputBuilder().AddMouseHorizontalWheelScroll(scrollAmountInClicks * 120).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator Sleep(int millsecondsTimeout)
		{
			Thread.Sleep(millsecondsTimeout);
			return this;
		}

		public IMouseSimulator Sleep(TimeSpan timeout)
		{
			Thread.Sleep(timeout);
			return this;
		}

		private readonly IInputSimulator _inputSimulator;

		private readonly IInputMessageDispatcher _messageDispatcher;
	}
}
