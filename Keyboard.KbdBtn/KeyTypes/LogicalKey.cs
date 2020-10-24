using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using WindowsInput;

namespace Keyboard.KbdBtn
{
    /// <summary>
    /// This is the base class for all keyboard keys types.  This class contains the functionality
    /// to simulate keyboard input to the OS.  It also contains an event that can be raised
    /// to notify listeners that a key press event occured.
    /// </summary>
    internal abstract class LogicalKey : IEquatable<LogicalKey>
    {
        private readonly IKeyboardSimulator keyboard;
        private readonly IInputDeviceStateAdaptor inputDeviceState;
        internal event EventHandler<LogicalKeyEventArgs> KeyPressed;

        internal LogicalKey(IInputSimulator inputSimulator, VirtualKeyCode key)
        {
            keyboard = inputSimulator.Keyboard;
            inputDeviceState = inputSimulator.InputDeviceState;
            KeyCode = (WindowsInput.Native.VirtualKeyCode)key;

        }
        public WindowsInput.Native.VirtualKeyCode KeyCode { get; private set; }

        /// <summary>
        /// Causes the KeyPressed event to be raised.
        /// </summary>
        internal virtual void ScreenKeyPress()
        {
            OnKeyPress(new LogicalKeyEventArgs(KeyCode));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as LogicalKey);
        }

        public override int GetHashCode()
        {
            return KeyCode.GetHashCode();
        }

        /// <summary>
        /// Initiates an input into the OS so simulate a key down action.
        /// </summary>
        protected void KeyDown()
        {
            keyboard.KeyDown(KeyCode);
        }

        /// <summary>
        ///  Initiates an input into the OS so simulate a key up action.
        /// </summary>
        protected void KeyUp()
        {
            keyboard.KeyUp(KeyCode);
        }

        /// <summary>
        ///  Initiates an input into the OS so simulate a key press action.
        /// </summary>
        protected void KeyPress()
        {
            keyboard.KeyPress(KeyCode);
        }

        /// <summary>
        /// Initiates the input of one or more modifier keys and an additional virtual key code, ex: Ctrl+C
        /// </summary>
        /// <param name="modifierKeys"></param>
        protected void ModifiedKeyStrokes(IList<WindowsInput.Native.VirtualKeyCode> modifierKeys)
        {
            keyboard.ModifiedKeyStroke(modifierKeys, KeyCode);
        }

        /// <summary>
        /// Initiates the input of one or more modifier keys and one or more virtual key code, ex: Ctrl+C
        /// </summary>
        /// <param name="modifierKeys"></param>
        /// <param name="keyCodes"></param>
        protected void ModifiedKeyStrokes(IList<WindowsInput.Native.VirtualKeyCode> modifierKeys, IList<WindowsInput.Native.VirtualKeyCode> keyCodes)
        {
            keyboard.ModifiedKeyStroke(modifierKeys, keyCodes);
        }

        /// <summary>
        /// Initiates the input of one or more characters
        /// </summary>
        /// <param name="text"></param>
        protected void OutputText(string text)
        {
            keyboard.TextEntry(text);
        }

        /// <summary>
        /// Determines if a keyboard key is in the down state
        /// </summary>
        /// <returns></returns>
        protected bool IsHardwareKeyDown()
        {
            return inputDeviceState.IsHardwareKeyDown(KeyCode);
        }

        /// <summary>
        /// Determines if a toggling key, such as the caps lock key, is in effect.
        /// </summary>
        /// <returns></returns>
        protected bool IsTogglingKeyInEffect()
        {
            return inputDeviceState.IsTogglingKeyInEffect(KeyCode);
        }

        /// <summary>
        /// Raises the KeyPressed event.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnKeyPress(LogicalKeyEventArgs args)
        {
            // Get the event handler containing the registered listeners.
            EventHandler<LogicalKeyEventArgs> handler = KeyPressed;

            // If there are no listeners,
            if (handler == null)
            {
                // Nothing to do.
                return;
            }

            // Extract the list of registered listeners.
            Delegate[] listeners = handler.GetInvocationList();
            EventHandler<LogicalKeyEventArgs> stateChangeListener;

            // For each registered listener in the list,
            foreach (Delegate listener in listeners)
            {
                try
                {
                    // Get the state change listener.
                    stateChangeListener = listener as EventHandler<LogicalKeyEventArgs>;

                    // Broadcast the event with the arguments provided.
                    stateChangeListener?.Invoke(this, args);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    System.Diagnostics.Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "KeyPressed listener threw exception: {0}", ex));
                    // Don't rethrow the exception .
                }
            }
        }

        #region IEquatable
        public bool Equals([AllowNull] LogicalKey other)
        {
            // If parameter is null, return false.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (GetType() != other.GetType())
            {
                return false;
            }

            return KeyCode == other.KeyCode;
        }
        #endregion

        public static bool operator ==(LogicalKey key1, LogicalKey key2)
        {
            // Check for null on left side.
            if (ReferenceEquals(key1, null))
            {
                if (ReferenceEquals(key2, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return key1.Equals(key2);
        }

        public static bool operator !=(LogicalKey key1, LogicalKey key2)
        {
            return !(key1 == key2);
        }
    }
}
