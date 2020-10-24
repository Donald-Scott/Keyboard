using System;
using System.Diagnostics.CodeAnalysis;
using WindowsInput;

namespace Keyboard.KbdBtn
{
    internal class InstantaneousModifierKey : ModifierKeyBase, IEquatable<InstantaneousModifierKey>
    {
        public InstantaneousModifierKey(IInputSimulator inputSimulator, VirtualKeyCode key) : base(inputSimulator, key)
        {
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

            return Equals(obj as InstantaneousModifierKey);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal override void ScreenKeyPress()
        {
            if (IsInEffect)
            {
                KeyUp();
            }
            else
            {
                KeyDown();
            }

            IsInEffect = IsHardwareKeyDown();
            OnKeyPress(new LogicalKeyEventArgs(KeyCode));
        }

        internal override void SynchroniseKeyState()
        {
            IsInEffect = IsHardwareKeyDown();
        }

        #region IEquatable
        public bool Equals([AllowNull] InstantaneousModifierKey other)
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

        public static bool operator ==(InstantaneousModifierKey key1, InstantaneousModifierKey key2)
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

        public static bool operator !=(InstantaneousModifierKey key1, InstantaneousModifierKey key2)
        {
            return !(key1 == key2);
        }
    }
}
