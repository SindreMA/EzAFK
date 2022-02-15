using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace EzAFK
{
    public partial class Form1 : Form
    {
        KeyboardHook hook = new KeyboardHook();
        public Form1()
        {
            InitializeComponent();
            foreach (string item in Enum.GetNames(typeof(VirtualKeyCode)))
            {
                comboBox1.Items.Add(item);
            }
            hook.KeyPressed +=
          new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            hook.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Alt,
                Keys.F12);
        }
        public sealed class KeyboardHook : IDisposable
        {
            // Registers a hot key with Windows.
            [DllImport("user32.dll")]
            private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
            // Unregisters the hot key with Windows.
            [DllImport("user32.dll")]
            private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

            /// <summary>
            /// Represents the window that is used internally to get the messages.
            /// </summary>
            private class Window : NativeWindow, IDisposable
            {
                private static int WM_HOTKEY = 0x0312;

                public Window()
                {
                    // create the handle for the window.
                    this.CreateHandle(new CreateParams());
                }

                /// <summary>
                /// Overridden to get the notifications.
                /// </summary>
                /// <param name="m"></param>
                protected override void WndProc(ref Message m)
                {
                    base.WndProc(ref m);

                    // check if we got a hot key pressed.
                    if (m.Msg == WM_HOTKEY)
                    {
                        // get the keys.
                        Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                        ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                        // invoke the event to notify the parent.
                        if (KeyPressed != null)
                            KeyPressed(this, new KeyPressedEventArgs(modifier, key));
                    }
                }

                public event EventHandler<KeyPressedEventArgs> KeyPressed;

                #region IDisposable Members

                public void Dispose()
                {
                    this.DestroyHandle();
                }

                #endregion
            }

            private Window _window = new Window();
            private int _currentId;

            public KeyboardHook()
            {
                // register the event of the inner native window.
                _window.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
                {
                    if (KeyPressed != null)
                        KeyPressed(this, args);
                };
            }

            /// <summary>
            /// Registers a hot key in the system.
            /// </summary>
            /// <param name="modifier">The modifiers that are associated with the hot key.</param>
            /// <param name="key">The key itself that is associated with the hot key.</param>
            public void RegisterHotKey(ModifierKeys modifier, Keys key)
            {
                // increment the counter.
                _currentId = _currentId + 1;

                // register the hot key.
                if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                    throw new InvalidOperationException("Couldn’t register the hot key.");
            }

            /// <summary>
            /// A hot key has been pressed.
            /// </summary>
            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                // unregister all the registered hot keys.
                for (int i = _currentId; i > 0; i--)
                {
                    UnregisterHotKey(_window.Handle, i);
                }

                // dispose the inner native window.
                _window.Dispose();
            }

            #endregion
        }

        /// <summary>
        /// Event Args for the event that is fired after the hot key has been pressed.
        /// </summary>
        public class KeyPressedEventArgs : EventArgs
        {
            private ModifierKeys _modifier;
            private Keys _key;

            internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
            {
                _modifier = modifier;
                _key = key;
            }

            public ModifierKeys Modifier
            {
                get { return _modifier; }
            }

            public Keys Key
            {
                get { return _key; }
            }
        }

        /// <summary>
        /// The enumeration of possible modifiers.
        /// </summary>
        [Flags]
        public enum ModifierKeys : uint
        {
            Alt = 1,
            Control = 2,
            Shift = 4,
            Win = 8
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // show the keys pressed in a label.
            //label1.Text = e.Modifier.ToString() + " + " + e.Key.ToString();
            //MessageBox.Show("");
            if (on == true)
            {
                timer1.Enabled = false;
                on = false;

                var icon = new NotifyIcon();
                icon.ShowBalloonTip(1000, "Status", "EzAFK is deactivated", ToolTipIcon.None);
             
     

            }
            else if (on == false)
            {
                timer1.Enabled = true;
                on = true;
                notifyIcon1.ShowBalloonTip(1000, "Status", "EzAFK is activated", ToolTipIcon.None);

            }
        }
        public bool on;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        [DllImport("User32.dll")]
        static extern bool SetForegroundWindow(IntPtr point);
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        InputSimulator input = new InputSimulator();

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }

            if (textBox1.Text.Contains("["))
            {

                //SendKeys.Send(textBox3.Text);
            }
            //SendKeys.SendWait(textBox3.Text);
            VirtualKeyCode key;
            if (Enum.TryParse(comboBox1.Text, out key))
            {
                input.Keyboard.KeyDown(key);
                Thread.Sleep(50);
                input.Keyboard.KeyUp(key);
            }
            Random rnd = new Random();
            int moh = rnd.Next(Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text)); // creates a number between 1 and 12

            timer1.Interval = moh * 1000;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            on = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            timer1.Enabled = false;
            on = false;
        }
    }
}
