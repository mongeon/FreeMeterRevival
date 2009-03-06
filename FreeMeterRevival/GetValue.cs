using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FreeMeter
{
	public partial class GetValue : Form
	{
		private string val;

		public string Value
		{
			get { return val; }
		}

		public GetValue(string text)
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
