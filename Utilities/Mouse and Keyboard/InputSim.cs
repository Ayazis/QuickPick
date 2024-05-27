using WindowsInput;
using WindowsInput.Native;

namespace QuickPick.Logic
{
	public static class InputSim
	{
		public static InputSimulator Simulator = new InputSimulator();

		public static void WinD()
		{
			   Simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_D);
		}


        public static void CtrlAltBreak()
		{
			Simulator.Keyboard.ModifiedKeyStroke(new VirtualKeyCode[]
			{
				VirtualKeyCode.LCONTROL,
				VirtualKeyCode.MENU,
				VirtualKeyCode.PAUSE
			}, VirtualKeyCode.NONAME);
		}


		public static void VolummeUp()
		{
			Simulator.Keyboard.KeyPress(VirtualKeyCode.VOLUME_UP);
		}

		public static void VolummeDown()
		{
			Simulator.Keyboard.KeyPress(VirtualKeyCode.VOLUME_DOWN);
		}
		public static void CtrlN()
		{
			Simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_N);
		}

		public static void Paste()
		{
			Simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_V);
		}

		public static void F5()
		{
			Simulator.Keyboard.KeyDown(VirtualKeyCode.F5);
		}

		public static void PlayPause()
		{
			Simulator.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PLAY_PAUSE);
		}

		public static void ToggleMute()
		{
			Simulator.Keyboard.KeyPress(VirtualKeyCode.VOLUME_MUTE);
		}		
	}
}
