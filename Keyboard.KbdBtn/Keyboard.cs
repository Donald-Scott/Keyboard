using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WindowsInput;

namespace Keyboard.KbdBtn
{
    internal sealed class Keyboard
    {
        private static Keyboard instance = null;
        private static readonly object padlock = new object();

        private static List<LogicalKey> keys;
        private static List<ModifierKeyBase> modifierKeys;
        private static IInputSimulator inputSimulator;

        Keyboard()
        {
            keys = new List<LogicalKey>();
            modifierKeys = new List<ModifierKeyBase>();
        }

        internal static Keyboard Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Keyboard();
                    }
                    return instance;
                }
            }
        }

        internal static IInputSimulator InputSimulator
        {
            get
            {
                if (inputSimulator == null)
                {
                    inputSimulator = new InputSimulator();
                }
                return inputSimulator;
            }

            set
            {
                inputSimulator = value;
            }
        }

        internal LogicalKey AddKey(VirtualKeyCode keyCode, KeyBehaviour keyType, VirtualKeyCollection chordKeys, string outputText, VirtualKeyCollection chordModifiers)
        {
            LogicalKey logicalKey = CreateKey(keyCode, InputSimulator, keyType, chordKeys, outputText, chordModifiers);

            if (logicalKey == null)
            {
                return null;
            }

            LogicalKey existingKey = keys.Where(k => k.Equals(logicalKey)).FirstOrDefault();
            if (existingKey != null)
            {
                return existingKey;
            }

            keys.Add(logicalKey);
            logicalKey.KeyPressed += LogicalKeyPressed;
            if (logicalKey is ModifierKeyBase)
            {
                modifierKeys.Add(logicalKey as ModifierKeyBase);
            }

            return logicalKey;
        }

        internal void RemoveKey(LogicalKey key)
        {
            if (key is ModifierKeyBase)
            {
                modifierKeys.Remove(key as ModifierKeyBase);
            }
            key.KeyPressed -= LogicalKeyPressed;
            keys.Remove(key);
        }

        /// <summary>
        /// Instruct all modifier keys to synchronise their state with the operating system\hardware.
        /// </summary>
        internal void SynchroniseModifierKeyState()
        {
            modifierKeys.ToList().ForEach(x => x.SynchroniseKeyState());
        }

        private void LogicalKeyPressed(object sender, LogicalKeyEventArgs e)
        {
            if (!(sender is ModifierKeyBase modifierKey)) // if not a modifier key
            {
                ResetInstantaneousModifierKeys();
            }
            modifierKeys.OfType<InstantaneousModifierKey>().ToList().ForEach(x => x.SynchroniseKeyState());
        }

        /// <summary>
        /// If an Instantaneous Modifier Key is in effect then press it to turn it off
        /// </summary>
        private void ResetInstantaneousModifierKeys()
        {
            modifierKeys.OfType<InstantaneousModifierKey>().ToList().ForEach(key =>
            {
                if (key.IsInEffect)
                {
                    key.ScreenKeyPress();
                }
            });
        }

        private static LogicalKey CreateKey(VirtualKeyCode keyCode, IInputSimulator inputSimulator, KeyBehaviour keyType, VirtualKeyCollection chordKeys, string outputText, VirtualKeyCollection chordModifiers)
        {
            LogicalKey result = null;

            switch (keyType)
            {
                case KeyBehaviour.VirtualKey:
                    result = CreateVirtualKey(keyCode, inputSimulator);
                    break;
                case KeyBehaviour.Chord:
                    result = CreateChordKey(keyCode, inputSimulator, chordKeys, chordModifiers);
                    break;
                case KeyBehaviour.Text:
                    result = CreateTextKey(keyCode, inputSimulator, outputText);
                    break;
                case KeyBehaviour.InstantaneousModifier:
                    result = CreateInstantaneousModifierKey(keyCode, inputSimulator);
                    break;
                case KeyBehaviour.TogglingModifier:
                    result = CreateTogglingModifierKey(keyCode, inputSimulator);
                    break;
                default:
                    break;
            }

            return result;
        }

        private static VirtualKey CreateVirtualKey(VirtualKeyCode keyCode, IInputSimulator inputSimulator)
        {
            if (keyCode == VirtualKeyCode.None)
            {
                return null;
            }
            return new VirtualKey(inputSimulator, keyCode);
        }

        private static ChordKey CreateChordKey(VirtualKeyCode keyCode, IInputSimulator inputSimulator, VirtualKeyCollection chordKeys, VirtualKeyCollection chordModifiers)
        {
            if (chordKeys == null || !chordKeys.Any())
            {
                return null;
            }
            return new ChordKey(inputSimulator, chordKeys, chordModifiers);
        }

        private static TextKey CreateTextKey(VirtualKeyCode keyCode, IInputSimulator inputSimulator, string outputText)
        {
            if (string.IsNullOrEmpty(outputText))
            {
                return null;
            }
            return new TextKey(inputSimulator, keyCode, outputText);
        }

        private static InstantaneousModifierKey CreateInstantaneousModifierKey(VirtualKeyCode keyCode, IInputSimulator inputSimulator)
        {
            if (keyCode == VirtualKeyCode.None)
            {
                return null;
            }
            return new InstantaneousModifierKey(inputSimulator, keyCode);
        }

        private static TogglingModifierKey CreateTogglingModifierKey(VirtualKeyCode keyCode, IInputSimulator inputSimulator)
        {
            if (keyCode == VirtualKeyCode.None)
            {
                return null;
            }
            return new TogglingModifierKey(inputSimulator, keyCode);
        }
    }
}
