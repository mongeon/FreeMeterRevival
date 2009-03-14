using System.Windows.Forms;
using System;

namespace FreeMeterRevival.Forms
{
	public partial class GetValueForm : Form
	{
        private TextBox textBox1;
		private string val;

		public string Value
		{
			get { return val; }
		}

		public GetValueForm(string text)
		{
			InitializeComponent();

			this.Text = text;
			this.CenterToScreen();
		}

		private void GetValue_FormClosing(object sender, FormClosingEventArgs e)
		{
			val = textBox1.Text;
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}



       

    
	}
}
