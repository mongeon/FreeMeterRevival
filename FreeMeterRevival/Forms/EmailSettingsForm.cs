using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace FreeMeterRevival.Forms
{
    public class EmailSettingsForm : Form
    {
        public MainForm MyParentForm;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private NumericUpDown numericUpDown1;
        private CheckBox checkBox1;
        private Label label1;
        private Label labelt;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private GroupBox groupBox1;
        public string Server
        {
            get { return comboBox1.SelectedItem.ToString(); }
        }
        public string User
        {
            get { return textBox2.Text; }
        }
        public string Pass
        {
            get { return textBox3.Text; }
        }
        public int Time
        {
            get { return (int)numericUpDown1.Value; }
        }

        public EmailSettingsForm()
        {
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            numericUpDown1 = new NumericUpDown();
            checkBox1 = new CheckBox();
            label1 = new Label();
            labelt = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            groupBox1 = new GroupBox();

            // comboBox1
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(103, 9);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(153, 20);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            // comboBox2
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(103, 33);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(60, 20);
            comboBox2.TabIndex = 1;
            comboBox2.SelectedIndexChanged += new EventHandler(comboBox2_SelectedIndexChanged);
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.Add("POP3");
            comboBox2.Items.Add("IMAP");

            // textBox1
            textBox1.Location = new Point(103, 57);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(133, 20);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += new EventHandler(textBox1_TextChanged);

            // textBox2
            textBox2.Location = new Point(103, 81);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 20);
            textBox2.TabIndex = 3;
            textBox2.TextChanged += new EventHandler(textBox2_TextChanged);

            // textBox3
            textBox3.Location = new Point(103, 105);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 20);
            textBox3.TabIndex = 4;
            textBox3.UseSystemPasswordChar = true;
            textBox3.TextChanged += new EventHandler(textBox3_TextChanged);

            // checkbox1
            checkBox1.Location = new Point(103, 127);
            checkBox1.Name = "checkbox1";
            checkBox1.TabIndex = 5;
            checkBox1.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);

            // numericUpDown1
            numericUpDown1.Location = new Point(145, 168);
            numericUpDown1.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(40, 20);
            numericUpDown1.TabIndex = 6;
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // label1
            label1.Location = new Point(3, 12);
            label1.Name = "label1";
            label1.Font = new Font("Tahoma", 8);
            label1.Size = new Size(95, 13);
            label1.Text = "Display Name";
            label1.TextAlign = ContentAlignment.MiddleRight;
            label1.BackColor = Color.White;

            // labelt
            labelt.Location = new Point(3, 36);
            labelt.Name = "labelt";
            labelt.Font = new Font("Tahoma", 8);
            labelt.Size = new Size(95, 13);
            labelt.Text = "Server Type";
            labelt.TextAlign = ContentAlignment.MiddleRight;
            labelt.BackColor = Color.White;

            // label2
            label2.Location = new Point(3, 60);
            label2.Name = "label2";
            label2.Font = new Font("Tahoma", 8);
            label2.Size = new Size(95, 13);
            label2.Text = "Server Host";
            label2.TextAlign = ContentAlignment.MiddleRight;
            label2.BackColor = Color.White;

            // label3
            label3.Location = new Point(3, 84);
            label3.Name = "label3";
            label3.Font = new Font("Tahoma", 8);
            label3.Size = new Size(95, 13);
            label3.Text = "Username";
            label3.TextAlign = ContentAlignment.MiddleRight;
            label3.BackColor = Color.White;

            // label4
            label4.Location = new Point(3, 108);
            label4.Name = "label4";
            label4.Font = new Font("Tahoma", 8);
            label4.Size = new Size(95, 13);
            label4.Text = "Password";
            label4.TextAlign = ContentAlignment.MiddleRight;
            label4.BackColor = Color.White;

            // label5
            label5.Location = new Point(3, 132);
            label5.Name = "label5";
            label5.Font = new Font("Tahoma", 8);
            label5.Size = new Size(95, 13);
            label5.Text = "Enabled";
            label5.TextAlign = ContentAlignment.MiddleRight;
            label5.BackColor = Color.White;

            // label6
            label6.Location = new Point(17, 171);
            label6.Name = "label6";
            label6.Font = new Font("Tahoma", 8);
            label6.Size = new Size(125, 13);
            label6.Text = "Check Email Every (min)";
            label6.TextAlign = ContentAlignment.MiddleRight;

            // add button (button4)
            button4.Location = new Point(20, 196);
            button4.Name = "button3";
            button4.Size = new Size(60, 17);
            button4.TabIndex = 7;
            button4.Text = "Add New";
            button4.Click += new EventHandler(button4_Click);

            // button1
            button1.DialogResult = DialogResult.OK;
            button1.Location = new Point(85, 196);
            button1.Name = "button1";
            button1.Size = new Size(55, 17);
            button1.TabIndex = 8;
            button1.Text = "Save";

            // button2
            button2.DialogResult = DialogResult.Cancel;
            button2.Location = new Point(145, 196);
            button2.Name = "button2";
            button2.Size = new Size(55, 17);
            button2.TabIndex = 9;
            button2.Text = "Cancel";

            // button3
            button3.Location = new Point(205, 196);
            button3.Name = "button3";
            button3.Size = new Size(55, 17);
            button3.TabIndex = 10;
            button3.Text = "Delete";
            button3.Click += new EventHandler(button3_Click);

            // groupBox1
            groupBox1.Location = new Point(10, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(260, 154);
            groupBox1.TabStop = false;
            groupBox1.Text = "";
            groupBox1.SendToBack();

            // EmailSettings_Form
            CancelButton = button2;
            ClientSize = new Size(280, 224);
            ControlBox = false;
            groupBox1.Controls.AddRange(new Control[] { label1, labelt, label2, label3, label4, label5, comboBox1, comboBox2, textBox1, textBox2, textBox3, checkBox1 });
            Controls.AddRange(new Control[] { groupBox1, label6, numericUpDown1, button4, button1, button2, button3 });
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EmailSettings_Form";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Email Server Settings";
            Load += new System.EventHandler(EmailSettings_Form_Load);
        }

        //comboBox fuctionality and events to synch data to main form
        public ArrayList har = new ArrayList();
        public ArrayList uar = new ArrayList();
        public ArrayList par = new ArrayList();
        public ArrayList ear = new ArrayList();
        public ArrayList tar = new ArrayList();
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                textBox1.Text = har[comboBox1.SelectedIndex].ToString();
                textBox2.Text = uar[comboBox1.SelectedIndex].ToString();
                textBox3.Text = par[comboBox1.SelectedIndex].ToString();
                if (tar[comboBox1.SelectedIndex].ToString() == "1")
                    comboBox2.SelectedIndex = 1;
                else
                    comboBox2.SelectedIndex = 0;
                checkBox1.Checked = (bool)ear[comboBox1.SelectedIndex];
            }
            else
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
        }
        void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tar[comboBox1.SelectedIndex] = comboBox2.SelectedIndex;
        }
        void textBox1_TextChanged(object sender, EventArgs e)
        {
            har[comboBox1.SelectedIndex] = textBox1.Text;
            comboBox1.Items[comboBox1.SelectedIndex] = textBox1.Text;
        }
        void textBox2_TextChanged(object sender, EventArgs e)
        {
            uar[comboBox1.SelectedIndex] = textBox2.Text;
        }
        void textBox3_TextChanged(object sender, EventArgs e)
        {
            par[comboBox1.SelectedIndex] = textBox3.Text;
        }
        void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ear[comboBox1.SelectedIndex] = checkBox1.Checked;
        }
        //delete mail server
        void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count > 1)
            {
                har.RemoveAt(comboBox1.SelectedIndex);
                uar.RemoveAt(comboBox1.SelectedIndex);
                par.RemoveAt(comboBox1.SelectedIndex);
                ear.RemoveAt(comboBox1.SelectedIndex);
                tar.RemoveAt(comboBox1.SelectedIndex);
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Zero is not enough for me!\n\nIf you don't want to use me, then just\nuncheck Email Notify in the main menu.", "Cannot remove only entry.");
            }
        }
        //add mail server
        void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count < 5)
            {
                har.Add("mail.exampleserver.com");
                uar.Add("username");
                par.Add("password");
                ear.Add(true);
                tar.Add(0);
                this.comboBox1.Items.Add("mail.exampleserver.com");
                this.comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                this.comboBox2.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Five is enough for me!\n\nI can't display more in a balloon tip.", "Reached Maximum Entries.");
            }
        }

        private void EmailSettings_Form_Load(object sender, System.EventArgs e)
        {
            foreach (MailServer s in ((MainForm)MyParentForm).MailServers)
            {
                har.Add(s.Host);
                uar.Add(s.User);
                par.Add(s.Pass);
                ear.Add(s.Enabled);
                tar.Add(s.Type);
                this.comboBox1.Items.Add(s.Host);
            }
            this.comboBox1.SelectedIndex = 0;
            this.numericUpDown1.Value = new decimal(new int[] { (((MainForm)MyParentForm).MailTimer.Interval / 60 / 1000), 0, 0, 0 });
        }
    }
}
