using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace EzAFK
{
    public partial class Timer : UserControl
    {
        InputSimulator input = new InputSimulator();
        DateTime lastTick;
        int clicks = 0;
        DateTime started;

        bool noClick = false;
        public Timer(int id)
        {
            InitializeComponent();
            groupBox1.Text = "Timer " + id;
            foreach (string item in Enum.GetNames(typeof(VirtualKeyCode)))
            {
                comboBox1.Items.Add(item);
            }
            foreach (var process in Process.GetProcesses())
            {
                comboBox2.Items.Add(process.ProcessName);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!noClick)
            {
                VirtualKeyCode key;
                if (Enum.TryParse(comboBox1.Text, out key))
                {
                    foreach (var process in Process.GetProcesses())
                    {
                        if (process.ProcessName == comboBox2.Text)
                        {
                            Console.WriteLine("Swapping to " + process.ProcessName);
                            ProcessHelper.SetFocusToExternalApp(process.ProcessName);
                        }
                    }

                    clicks++;
                    Console.WriteLine("Clicking " + key);
                    input.Keyboard.KeyDown(key);
                    Thread.Sleep(50);
                    input.Keyboard.KeyUp(key);
                }
            }
            lastTick = DateTime.Now;
            Random rnd = new Random();
            int moh = rnd.Next(Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text)); // creates a number between 1 and 12

            timer1.Interval = moh * 1000;
            Console.WriteLine("Timer set to " + moh + " seconds");
            noClick = false;
        }
        public bool on;
        private void button2_Click(object sender, EventArgs e)
        {
            noClick = true;
            started = DateTime.Now;
            timer1.Enabled = true;
            timer2.Enabled = true;
            on = true;
            checkBox1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            timer1.Enabled = false;
            timer2.Enabled = false;
            on = false;
            checkBox1.Checked = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Remove self
            this.Parent.Controls.Remove(this);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Tick tak timer 2");
            if (lastTick != null && lastTick != DateTime.MinValue)
            {
                TimeSpan duration = DateTime.Now.Subtract(lastTick);
                TimeSpan runtimeDuration = DateTime.Now.Subtract(started);
                System.Console.WriteLine("timer1: " + timer1.Interval);
                System.Console.WriteLine("duration: " + duration.Ticks);
                progressBar1.Maximum = timer1.Interval;
                progressBar1.Enabled = true;


                //countdown
                label9.Text = $@"{Math.Round((timer1.Interval / 1000) - duration.TotalSeconds)}";
                label8.Text = $@"{clicks}";
                label6.Text = $@"{Math.Round(runtimeDuration.TotalSeconds)}";

                progressBar1.Value = int.Parse($@"{Math.Round(duration.TotalMilliseconds)}");
            }            
        }
    }
}
