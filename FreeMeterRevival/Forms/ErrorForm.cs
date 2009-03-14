using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FreeMeterRevival.Forms
{
    public partial class ErrorForm : Form
    {
        private Exception ex;

        public ErrorForm(Exception ex)
        {
            InitializeComponent();
            this.ex = ex;
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtMessage.Text);
        }

        private void Error_Load(object sender, EventArgs e)
        {
            txtMessage.Text = string.Format("Message: {0}\r\nStack trace:\r\n\r\n{1}", ex.Message, ex.StackTrace);
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