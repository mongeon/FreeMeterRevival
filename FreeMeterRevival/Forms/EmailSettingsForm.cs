using System;
using System.Collections;
using System.Windows.Forms;

namespace FreeMeterRevival.Forms
{
    public partial class EmailSettingsForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public MainForm MyParentForm;

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
            InitializeComponent();
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
