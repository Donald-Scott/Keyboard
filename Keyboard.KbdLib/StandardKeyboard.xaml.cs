using Keyboard.KbdBtn;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Keyboard.KbdLib
{
    /// <summary>
    /// Interaction logic for StandardKeyboard.xaml
    /// </summary>
    public partial class StandardKeyboard : UserControl
    {
        private List<KeyboardBtn> shiftSensitiveButtons;
        private List<KeyboardBtn> capsSinsitiveButtons;
        private List<KeyboardBtn> modifierKeys;

        public StandardKeyboard()
        {
            shiftSensitiveButtons = new List<KeyboardBtn>();
            capsSinsitiveButtons = new List<KeyboardBtn>();
            modifierKeys = new List<KeyboardBtn>();

            InitializeComponent();
            Loaded += StandardKeyboardLoaded;
        }

        private void StandardKeyboardLoaded(object sender, RoutedEventArgs e)
        {
            AddHandler(KeyboardBtn.ModifierChangedEvent, new ModifierChangedEventHandler(ModifierKeyChanged), true);
            
            foreach (UIElement child in  MainGrid.Children)
            {
                if (child is KeyboardBtn)
                {
                    SaveKeyboardButton(child as KeyboardBtn);
                }
                if (child is Panel)
                {
                    Panel panel = child as Panel;
                    foreach(UIElement decendent in panel.Children)
                    {
                        if (!(decendent is KeyboardBtn))
                        {
                            continue;
                        }

                        SaveKeyboardButton(decendent as KeyboardBtn);
                    }
                }
            }

            foreach(KeyboardBtn btn in modifierKeys)
            {
                btn.SynchroniseKeyState();
            }
        }

        private void ModifierKeyChanged(object sender, ModifierChangedEventArgs e)
        {
            switch(e.virtualKeyCode)
            {
                case VirtualKeyCode.Shift:
                    if (e.IsInEffect)
                    {
                        ProcessShiftInEffect();
                    }
                    else
                    {
                        ProcessShiftNotInEffect();
                    }
                    break;
                case VirtualKeyCode.Capital:
                    if (e.IsInEffect)
                    {
                        ProcessCapsInEffect();
                    }
                    else
                    {
                        ProcessCapsNotInEffect();
                    }
                    break;
            }
            
        }

        private void ProcessShiftInEffect()
        {
            foreach (KeyboardBtn btn in shiftSensitiveButtons)
            {
                object modifiedContent = btn.GetValue(KeyProperties.ModifiedContentProperty);
                btn.SetValue(ContentProperty, modifiedContent);
            }
            foreach (KeyboardBtn btn in capsSinsitiveButtons)
            {
                object modifiedContent = btn.GetValue(KeyProperties.ModifiedContentProperty);
                btn.SetValue(ContentProperty, modifiedContent);
            }
        }

        private void ProcessShiftNotInEffect()
        {
            foreach (KeyboardBtn btn in shiftSensitiveButtons)
            {
                object unmodifiedContent = btn.GetValue(KeyProperties.UnmodifiedContentProperty);
                btn.SetValue(ContentProperty, unmodifiedContent);
            }
            foreach (KeyboardBtn btn in capsSinsitiveButtons)
            {
                object unmodifiedContent = btn.GetValue(KeyProperties.UnmodifiedContentProperty);
                btn.SetValue(ContentProperty, unmodifiedContent);
            }
        }

        private void ProcessCapsInEffect()
        {
            foreach (KeyboardBtn btn in capsSinsitiveButtons)
            {
                object modifiedContent = btn.GetValue(KeyProperties.ModifiedContentProperty);
                btn.SetValue(ContentProperty, modifiedContent);
            }
        }

        private void ProcessCapsNotInEffect()
        {
            foreach (KeyboardBtn btn in capsSinsitiveButtons)
            {
                object unmodifiedContent = btn.GetValue(KeyProperties.UnmodifiedContentProperty);
                btn.SetValue(ContentProperty, unmodifiedContent);
            }
        }

        private void SaveKeyboardButton(KeyboardBtn btn)
        {
            if (btn.KeyBehaviour == KeyBehaviour.InstantaneousModifier || btn.KeyBehaviour == KeyBehaviour.TogglingModifier)
            {
                modifierKeys.Add(btn);
            }

            ModifierSensitivity sensitiveType = (ModifierSensitivity)btn.GetValue(KeyProperties.SensitivityTypeProperty);
            switch (sensitiveType)
            {
                case ModifierSensitivity.Caps:
                    capsSinsitiveButtons.Add(btn as KeyboardBtn);
                    //shiftSensitiveButtons.Add(decendent as KeyboardBtn);
                    break;
                case ModifierSensitivity.Shift:
                    shiftSensitiveButtons.Add(btn as KeyboardBtn);
                    break;
            }
        }
    }
}
