using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace FreeMeter
{
    public partial class Error : Form
    {
        private Exception ex;

        public Error(Exception ex)
        {
            InitializeComponent();
            this.ex = ex;
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }

        private void Error_Load(object sender, EventArgs e)
        {
            textBox1.Text = string.Format("Message: {0}\r\nStack trace:\r\n\r\n{1}", ex.Message, ex.StackTrace);
        }

        private void button_restart_Click(object sender, EventArgs e)
        {
            Process.Start(Application.ExecutablePath);
            Application.Exit();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}