using Keyboard.KbdBtn;
using NUnit.Framework;

namespace Keyboard.Tests
{
    public class KeyboardButtonTests
    {
        private MockInputSimulator inputSimulator;
        TestableButton btnA;
        TestableButton shift;
        TestableButton caps;
        TestableButton hello;
        TestableButton selectAll;

        [SetUp]
        public void Setup()
        {
            inputSimulator = new MockInputSimulator();
            KbdBtn.Keyboard.InputSimulator = inputSimulator;
            CreateButtons();
        }

        [Test]
        public void InputVirtualKeyA()
        {
            btnA.ClickButton();

            Assert.AreEqual(1, inputSimulator.KeyActions.Count);
            Assert.AreEqual(WindowsInput.Native.VirtualKeyCode.VK_A, inputSimulator.KeyActions[0].KeyCode);
        }

        [Test]
        public void InputVirtualKeyShiftA()
        {
            shift.ClickButton();
            btnA.ClickButton();

            Assert.AreEqual(3, inputSimulator.KeyActions.Count);
            Assert.AreEqual(WindowsInput.Native.VirtualKeyCode.SHIFT, inputSimulator.KeyActions[0].KeyCode);
            Assert.AreEqual(WindowsInput.Native.VirtualKeyCode.VK_A, inputSimulator.KeyActions[1].KeyCode);
            Assert.AreEqual(WindowsInput.Native.VirtualKeyCode.SHIFT, inputSimulator.KeyActions[2].KeyCode);
        }

        [Test]
        public void InputTextKeyHello()
        {
            hello.ClickButton();

            Assert.AreEqual("Hello World!", inputSimulator.GetTextInput());
        }

        [Test]
        public void InputSelectAllCommand()
        {
            selectAll.ClickButton();

            Assert.AreEqual(3, inputSimulator.KeyActions.Count);
            Assert.AreEqual(WindowsInput.Native.VirtualKeyCode.CONTROL, inputSimulator.KeyActions[0].KeyCode);
            Assert.AreEqual(WindowsInput.Native.VirtualKeyCode.VK_A, inputSimulator.KeyActions[1].KeyCode);
            Assert.AreEqual(WindowsInput.Native.VirtualKeyCode.CONTROL, inputSimulator.KeyActions[2].KeyCode);
        }

        [Test]
        public void ChangeKeyAtoZ()
        {
            btnA.ClickButton();

            Assert.AreEqual(1, inputSimulator.KeyActions.Count);
            Assert.AreEqual(WindowsInput.Native.VirtualKeyCode.VK_A, inputSimulator.KeyActions[0].KeyCode);

            inputSimulator.ClearState();

            btnA.KeyCode = VirtualKeyCode.VkZ;
            btnA.ClickButton();

            Assert.AreEqual(1, inputSimulator.KeyActions.Count);
            Assert.AreEqual(WindowsInput.Native.VirtualKeyCode.VK_Z, inputSimulator.KeyActions[0].KeyCode);
        }

        [Test]
        public void ChangeTextKey()
        {
            hello.ClickButton();

            Assert.AreEqual("Hello World!", inputSimulator.GetTextInput());

            inputSimulator.ClearState();
            hello.OutputText = "Goodbye";

            hello.ClickButton();
            Assert.AreEqual("Goodbye", inputSimulator.GetTextInput());
        }

        [Test]
        public void ChangeKeyBehaviour()
        {
            btnA.ClickButton();

            Assert.AreEqual(1, inputSimulator.KeyActions.Count);
            Assert.AreEqual(WindowsInput.Native.VirtualKeyCode.VK_A, inputSimulator.KeyActions[0].KeyCode);
            Assert.AreEqual(string.Empty, inputSimulator.GetTextInput());

            inputSimulator.ClearState();

            btnA.KeyBehaviour = KeyBehaviour.Text;
            btnA.KeyCode = VirtualKeyCode.None;
            btnA.OutputText = "Virtual Key";
            btnA.ClickButton();

            Assert.AreEqual(0, inputSimulator.KeyActions.Count);
            Assert.AreEqual("Virtual Key", inputSimulator.GetTextInput());
        }

        [Test]
        public void GetKeyCode()
        {
            VirtualKeyCode keyCode = btnA.KeyCode;
            Assert.AreEqual(VirtualKeyCode.VkA, keyCode);
        }

        private void CreateButtons()
        {
            btnA = new TestableButton();
            btnA.BeginInit();
            btnA.KeyBehaviour = KeyBehaviour.VirtualKey;
            btnA.KeyCode = VirtualKeyCode.VkA;
            btnA.EndInit();

            shift = new TestableButton();
            shift.BeginInit();
            shift.KeyBehaviour = KeyBehaviour.InstantaneousModifier;
            shift.KeyCode = VirtualKeyCode.Shift;
            shift.EndInit();

            caps = new TestableButton();
            caps.BeginInit();
            caps.KeyBehaviour = KeyBehaviour.TogglingModifier;
            caps.KeyCode = VirtualKeyCode.Capital;
            caps.EndInit();

            hello = new TestableButton();
            hello.BeginInit();
            hello.KeyBehaviour = KeyBehaviour.Text;
            hello.KeyCode = VirtualKeyCode.None;
            hello.OutputText = "Hello World!";
            hello.EndInit();

            VirtualKeyCollection chordKeys = new VirtualKeyCollection();
            chordKeys.Add(VirtualKeyCode.VkA);

            VirtualKeyCollection chordModifierKeys = new VirtualKeyCollection();
            chordModifierKeys.Add(VirtualKeyCode.Control);

            selectAll = new TestableButton();
            selectAll.BeginInit();
            selectAll.KeyBehaviour = KeyBehaviour.Chord;
            //selectAll.KeyCode = VirtualKeyCode.VkA;

            selectAll.ChordKeys = chordKeys;
            selectAll.ChordModifiers = chordModifierKeys;
            selectAll.EndInit();
        }
    }
}