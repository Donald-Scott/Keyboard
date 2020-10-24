using WindowsInput;

namespace Keyboard.KbdBtn
{
    internal abstract class ModifierKeyBase : VirtualKey
    {
        public ModifierKeyBase(IInputSimulator inputSimulator, VirtualKeyCode key) : base(inputSimulator, key)
        {

        }

        internal bool IsInEffect { get; set; }

        internal abstract void SynchroniseKeyState();
    }
}
