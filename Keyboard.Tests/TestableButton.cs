using Keyboard.KbdBtn;

namespace Keyboard.Tests
{
    internal class TestableButton : KeyboardBtn
    {
        public void ClickButton()
        {
            OnClick();
        }
    }
}
