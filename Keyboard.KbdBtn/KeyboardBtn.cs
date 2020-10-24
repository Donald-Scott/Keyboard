using System.Windows;
using System.Windows.Controls;

namespace Keyboard.KbdBtn
{
    public class KeyboardBtn : Button
    {
        private LogicalKey logicalKey;

        public KeyboardBtn()
        {
            SetValue(ChordKeysProperty, new VirtualKeyCollection());
            SetValue(ChordModifiersProperty, new VirtualKeyCollection());
        }

        public VirtualKeyCode KeyCode
        {
            get { return (VirtualKeyCode)GetValue(KeyCodeProperty); }
            set { SetValue(KeyCodeProperty, value); }
        }

        public static readonly DependencyProperty KeyCodeProperty =
            DependencyProperty.Register("KeyCode", typeof(VirtualKeyCode), typeof(KeyboardBtn), new PropertyMetadata(VirtualKeyCode.None, KeyCodeChanged));

        public KeyBehaviour KeyBehaviour
        {
            get { return (KeyBehaviour)GetValue(KeyBehaviourProperty); }
            set { SetValue(KeyBehaviourProperty, value); }
        }

        public static readonly DependencyProperty KeyBehaviourProperty =
            DependencyProperty.Register("KeyBehaviour", typeof(KeyBehaviour), typeof(KeyboardBtn), new PropertyMetadata(KeyBehaviour.None, KeyBehaviourChanged));

        public VirtualKeyCollection ChordKeys
        {
            get { return (VirtualKeyCollection)GetValue(ChordKeysProperty); }
            set { SetValue(ChordKeysProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChordKeys.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChordKeysProperty =
            DependencyProperty.Register("ChordKeys", typeof(VirtualKeyCollection), typeof(KeyboardBtn), new PropertyMetadata(null, ChordKeysChanged));

        public VirtualKeyCollection ChordModifiers
        {
            get { return (VirtualKeyCollection)GetValue(ChordModifiersProperty); }
            set { SetValue(ChordModifiersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChordModifiers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChordModifiersProperty =
            DependencyProperty.Register("ChordModifiers", typeof(VirtualKeyCollection), typeof(KeyboardBtn), new PropertyMetadata(null, ChordModifiersChanged));

        public string OutputText
        {
            get { return (string)GetValue(OutputTextProperty); }
            set { SetValue(OutputTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutputText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputTextProperty =
            DependencyProperty.Register("OutputText", typeof(string), typeof(KeyboardBtn), new PropertyMetadata(string.Empty, OutputTextChanged));

        public static readonly RoutedEvent ModifierChangedEvent = EventManager.RegisterRoutedEvent("ModifierChanged", RoutingStrategy.Bubble,
            typeof(ModifierChangedEventHandler), typeof(KeyboardBtn));

        // Provide CLR accessors for the event
        public event ModifierChangedEventHandler ModifierKeyChanged
        {
            add { AddHandler(ModifierChangedEvent, value); }
            remove { RemoveHandler(ModifierChangedEvent, value); }
        }

        public void SynchroniseKeyState()
        {
            if (logicalKey == null)
            {
                return;
            }

            if (logicalKey is ModifierKeyBase)
            {
                ModifierKeyBase modifierKey = logicalKey as ModifierKeyBase;
                RaiseModifierChangedEvent((VirtualKeyCode)modifierKey.KeyCode, modifierKey.IsInEffect);
            }
        }

        public override void EndInit()
        {
            Focusable = false;
            CreateKey();
            base.EndInit();
        }

        protected override void OnClick()
        {
            if (KeyBehaviour != KeyBehaviour.None)
            {
                logicalKey.ScreenKeyPress();
            }
            
            base.OnClick();
        }

        private void CreateKey()
        {
            if (KeyBehaviour == KeyBehaviour.None)
            {
                return;
            }

            if (KeyBehaviour == KeyBehaviour.Chord)
            {
                System.Diagnostics.Trace.WriteLine("Creating Chord Key");
            }

            Keyboard kbd = Keyboard.Instance;

            logicalKey = kbd.AddKey(KeyCode, KeyBehaviour, ChordKeys, OutputText, ChordModifiers);
            if (logicalKey == null)
            {
                return;
            }

            if (logicalKey is ModifierKeyBase)
            {
                ModifierKeyBase modifierKey = logicalKey as ModifierKeyBase;
                modifierKey.SynchroniseKeyState();
            }

            logicalKey.KeyPressed += LogicalKeyPressed;
        }

        private void LogicalKeyPressed(object sender, LogicalKeyEventArgs e)
        {
            if (sender is ModifierKeyBase modifierKey)
            {
                RaiseModifierChangedEvent((VirtualKeyCode)modifierKey.KeyCode, modifierKey.IsInEffect);
            }
        }

        private void RemoveKey()
        {
            if (logicalKey == null)
            {
                return;
            }

            Keyboard kbd = Keyboard.Instance;

            kbd.RemoveKey(logicalKey);
        }

        // This method raises the RaiseModifierChange event
        private void RaiseModifierChangedEvent(VirtualKeyCode keyCode, bool isInEffect)
        {
            ModifierChangedEventArgs newEventArgs = new ModifierChangedEventArgs(ModifierChangedEvent, keyCode, isInEffect);
            
            RaiseEvent(newEventArgs);
        }

        private static void RecreateKey(KeyboardBtn keyboardBtn)
        {
            if (keyboardBtn == null)
            {
                return;
            }
            keyboardBtn.RemoveKey();
            keyboardBtn.CreateKey();
        }

        private static void KeyCodeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RecreateKey(obj as KeyboardBtn);
        }

        private static void KeyBehaviourChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RecreateKey(obj as KeyboardBtn);
        }

        private static void ChordKeysChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RecreateKey(obj as KeyboardBtn);
        }

        private static void ChordModifiersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RecreateKey(obj as KeyboardBtn);
        }

        private static void OutputTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RecreateKey(obj as KeyboardBtn);
        }
    }
}
