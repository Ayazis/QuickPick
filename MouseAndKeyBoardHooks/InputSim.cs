using WindowsInput;
using WindowsInput.Native;

namespace MouseAndKeyBoardHooks;

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
