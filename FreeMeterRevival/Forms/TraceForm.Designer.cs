using System.Windows.Forms;
using System;
using System.Drawing;

namespace FreeMeterRevival.Forms
{
    public partial class TraceForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.hostbox = new System.Windows.Forms.TextBox();
            this.results = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.databox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.databox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.databox3 = new System.Windows.Forms.CheckBox();
            this.sendit = new System.Windows.Forms.Button();
            this.stopit = new System.Windows.Forms.Button();
            this.clearit = new System.Windows.Forms.Button();
            this.closeit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host:";
            // 
            // hostbox
            // 
            this.hostbox.Location = new System.Drawing.Point(35, 3);
            this.hostbox.Name = "hostbox";
            this.hostbox.Size = new System.Drawing.Size(227, 20);
            this.hostbox.TabIndex = 0;
            // 
            // results
            // 
            this.results.BackColor = System.Drawing.Color.Black;
            this.results.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.results.Font = new System.Drawing.Font("Tahoma", 8F);
            this.results.ForeColor = System.Drawing.Color.Silver;
            this.results.Location = new System.Drawing.Point(3, 25);
            this.results.Multiline = true;
            this.results.Name = "results";
            this.results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.results.Size = new System.Drawing.Size(388, 213);
            this.results.TabIndex = 7;
            this.results.WordWrap = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label2.Location = new System.Drawing.Point(3, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Max hops";
            // 
            // databox
            // 
            this.databox.Location = new System.Drawing.Point(56, 241);
            this.databox.MaximumSize = new System.Drawing.Size(30, 16);
            this.databox.Name = "databox";
            this.databox.Size = new System.Drawing.Size(30, 16);
            this.databox.TabIndex = 1;
            this.databox.Text = "30";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label3.Location = new System.Drawing.Point(92, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Timeout(ms)";
            // 
            // databox2
            // 
            this.databox2.Location = new System.Drawing.Point(158, 241);
            this.databox2.MaximumSize = new System.Drawing.Size(38, 16);
            this.databox2.Name = "databox2";
            this.databox2.Size = new System.Drawing.Size(38, 16);
            this.databox2.TabIndex = 2;
            this.databox2.Text = "3000";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label4.Location = new System.Drawing.Point(202, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Resolve names";
            // 
            // databox3
            // 
            this.databox3.Location = new System.Drawing.Point(280, 242);
            this.databox3.Name = "databox3";
            this.databox3.Size = new System.Drawing.Size(16, 16);
            this.databox3.TabIndex = 3;
            // 
            // sendit
            // 
            this.sendit.Location = new System.Drawing.Point(265, 3);
            this.sendit.Name = "sendit";
            this.sendit.Size = new System.Drawing.Size(40, 20);
            this.sendit.TabIndex = 4;
            this.sendit.Text = "Start";
            this.sendit.Click += new System.EventHandler(this.ButtonSendOnClick);
            // 
            // stopit
            // 
            this.stopit.Enabled = false;
            this.stopit.Location = new System.Drawing.Point(308, 3);
            this.stopit.Name = "stopit";
            this.stopit.Size = new System.Drawing.Size(40, 20);
            this.stopit.TabIndex = 5;
            this.stopit.Text = "Stop";
            this.stopit.Click += new System.EventHandler(this.ButtonStopOnClick);
            // 
            // clearit
            // 
            this.clearit.Location = new System.Drawing.Point(351, 3);
            this.clearit.Name = "clearit";
            this.clearit.Size = new System.Drawing.Size(40, 20);
            this.clearit.TabIndex = 6;
            this.clearit.Text = "Clear";
            this.clearit.Click += new System.EventHandler(this.ButtonClearOnClick);
            // 
            // closeit
            // 
            this.closeit.Location = new System.Drawing.Point(351, 239);
            this.closeit.Name = "closeit";
            this.closeit.Size = new System.Drawing.Size(42, 20);
            this.closeit.TabIndex = 8;
            this.closeit.Text = "Close";
            this.closeit.Click += new System.EventHandler(this.ButtonCloseOnClick);
            // 
            // TraceForm
            // 
            this.ClientSize = new System.Drawing.Size(398, 266);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hostbox);
            this.Controls.Add(this.results);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.databox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.databox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.databox3);
            this.Controls.Add(this.sendit);
            this.Controls.Add(this.stopit);
            this.Controls.Add(this.clearit);
            this.Controls.Add(this.closeit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "TraceForm";
            this.ShowIcon = false;
            this.Text = "Traceroute Utility";
            this.TextExtra = "FreeMeter Revival";
            this.Load += new System.EventHandler(this.AdvTrace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Button sendit ;
        private Button stopit ;
        private Button clearit;
        private Button closeit;
        private TextBox hostbox, databox, databox2, results;
        private CheckBox databox3;
        Label label1;
            Label label2;
                Label label3;
                Label label4;
    }
}
