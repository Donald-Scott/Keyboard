using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WindowsInput;

namespace Keyboard.KbdBtn
{
    internal class ChordKey : VirtualKey, IEquatable<ChordKey>
    {
        private List<WindowsInput.Native.VirtualKeyCode> modifierKeys;
        private List<WindowsInput.Native.VirtualKeyCode> keys;
        private int hashCode;

        public ChordKey(IInputSimulator inputSimulator, VirtualKeyCollection keys, VirtualKeyCollection modifierKeys) : base(inputSimulator, keys[0])
        {
            this.modifierKeys = new List<WindowsInput.Native.VirtualKeyCode>();
            foreach (VirtualKeyCode keyCode in modifierKeys)
            {
                this.modifierKeys.Add((WindowsInput.Native.VirtualKeyCode)keyCode);
            }

            this.keys = new List<WindowsInput.Native.VirtualKeyCode>();
            foreach(VirtualKeyCode keyCode in keys)
            {
                this.keys.Add((WindowsInput.Native.VirtualKeyCode)keyCode);
            }

            foreach (WindowsInput.Native.VirtualKeyCode keyCode in keys)
            {
                if (hashCode == 0)
                {
                    hashCode = keyCode.GetHashCode();
                }
                else
                {
                    hashCode = (hashCode * 397) ^ keyCode.GetHashCode();
                }
            }
            foreach (WindowsInput.Native.VirtualKeyCode keyCode in modifierKeys)
            {
                hashCode = (hashCode * 397) ^ keyCode.GetHashCode();
            }
        }

        internal override void ScreenKeyPress()
        {
            ModifiedKeyStrokes(modifierKeys, keys);

            List<WindowsInput.Native.VirtualKeyCode> pressedKeys = new List<WindowsInput.Native.VirtualKeyCode>();
            
            pressedKeys.AddRange(modifierKeys);
            pressedKeys.AddRange(keys);

            LogicalKeyEventArgs args = new LogicalKeyEventArgs(pressedKeys);
            OnKeyPress(args);
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

            return Equals(obj as ChordKey);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        #region IEquatable
        public bool Equals([AllowNull] ChordKey other)
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
            if (KeyCode != other.KeyCode)
            {
                return false;
            }
            return modifierKeys.SequenceEqual(other.modifierKeys);
        }
        #endregion

        public static bool operator ==(ChordKey key1, ChordKey key2)
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

        public static bool operator !=(ChordKey key1, ChordKey key2)
        {
            return !(key1 == key2);
        }
    }
}
