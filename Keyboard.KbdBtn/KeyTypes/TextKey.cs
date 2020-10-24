using System;
using System.Diagnostics.CodeAnalysis;
using WindowsInput;

namespace Keyboard.KbdBtn
{
    internal class TextKey : VirtualKey, IEquatable<TextKey>
    {
        private int hashCode;

        public TextKey(IInputSimulator inputSimulator, VirtualKeyCode key, string text) : base(inputSimulator, key)
        {
            Text = text;

            hashCode = KeyCode.GetHashCode();
            hashCode = (hashCode * 397) ^ Text.GetHashCode();
        }

        public string Text { get; set; }

        internal override void ScreenKeyPress()
        {
            OutputText(Text);
            //Do not call base class
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

            return Equals(obj as TextKey);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        #region IEquatable
        public bool Equals([AllowNull] TextKey other)
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
            return Text.Equals(other.Text);
        }
        #endregion

        public static bool operator ==(TextKey key1, TextKey key2)
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

        public static bool operator !=(TextKey key1, TextKey key2)
        {
            return !(key1 == key2);
        }
    }
}
