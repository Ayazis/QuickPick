using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace QuickPick.Logic
{
    public static class InputSim
    {
        public static InputSimulator Simulator = new InputSimulator();

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


    }
}
