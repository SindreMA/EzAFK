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
            flowLayoutPanel1.Controls.Add(new Timer(flowLayoutPanel1.Controls.Count + 1));
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Add new timer
            flowLayoutPanel1.Controls.Add(new Timer(flowLayoutPanel1.Controls.Count + 1));
        }
    }
}
