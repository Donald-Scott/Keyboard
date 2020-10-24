using System;
using System.Diagnostics.CodeAnalysis;
using WindowsInput;

namespace Keyboard.KbdBtn
{
    internal class VirtualKey : LogicalKey, IEquatable<VirtualKey>
    {
        public VirtualKey(IInputSimulator inputSimulator, VirtualKeyCode key)
            : base(inputSimulator, key)
        {
        }

        /// <summary>
        /// This method invokes the base class KeyPress method to simulate the keyboard inpu
        /// of the assigned virtual key code.  It also raises an event to notify listeners
        /// of the key press event.
        /// </summary>
        internal override void ScreenKeyPress()
        {
            KeyPress();
            base.ScreenKeyPress();
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

            return Equals(obj as VirtualKey);
        }

        public override int GetHashCode()
        {
            return KeyCode.GetHashCode();
        }

        #region IEquatable
        public bool Equals([AllowNull] VirtualKey other)
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

        public static bool operator ==(VirtualKey key1, VirtualKey key2)
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

        public static bool operator !=(VirtualKey key1, VirtualKey key2)
        {
            return !(key1 == key2);
        }
    }
}
