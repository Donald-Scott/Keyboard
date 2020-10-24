using System;
using System.Collections.Generic;

namespace Keyboard.KbdBtn
{
    internal class LogicalKeyEventArgs : EventArgs
    {
        /// <summary>
        /// Create an new LogicalKeyEventArgs with the collection of keyCode values.
        /// </summary>
        /// <param name="keyCodes"></param>
        internal LogicalKeyEventArgs(IEnumerable<WindowsInput.Native.VirtualKeyCode> keyCodes)
        {
            KeyCodes = new List<WindowsInput.Native.VirtualKeyCode>(keyCodes);
        }
        /// <summary>
        /// Create an new LogicalKeyEventArgs with the single keyCode value
        /// </summary>
        /// <param name="keyCode"></param>
        internal LogicalKeyEventArgs(WindowsInput.Native.VirtualKeyCode keyCode)
        {
            List<WindowsInput.Native.VirtualKeyCode> tempList = new List<WindowsInput.Native.VirtualKeyCode>
            {
                keyCode
            };

            KeyCodes = tempList;
        }

        internal IEnumerable<WindowsInput.Native.VirtualKeyCode> KeyCodes { get; }
    }
}
