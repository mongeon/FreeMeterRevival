using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;

namespace FreeMeterRevival.Forms
{
    public partial class UPnPForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        
        private Label label1 ;
        private Label label2;
        private Label label3 ;
        private Label label4 ;
        private TextBox results ;
        private TextBox port ;
        private TextBox address ;
        private ComboBox comboBox1 ;
        private Button refresh ;
        private Button add ;
        private Button delete ;

        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.results = new System.Windows.Forms.TextBox();
            this.port = new System.Windows.Forms.TextBox();
            this.address = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.refresh = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current UPnP NAT Mappings:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label2.Location = new System.Drawing.Point(3, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label3.Location = new System.Drawing.Point(76, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fwd to";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label4.Location = new System.Drawing.Point(214, 235);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Proto";
            // 
            // results
            // 
            this.results.BackColor = System.Drawing.Color.Black;
            this.results.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.results.Font = new System.Drawing.Font("Tahoma", 8F);
            this.results.ForeColor = System.Drawing.Color.Silver;
            this.results.Location = new System.Drawing.Point(3, 24);
            this.results.Multiline = true;
            this.results.Name = "results";
            this.results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.results.Size = new System.Drawing.Size(380, 207);
            this.results.TabIndex = 7;
            this.results.WordWrap = false;
            // 
            // port
            // 
            this.port.Font = new System.Drawing.Font("Tahoma", 8F);
            this.port.Location = new System.Drawing.Point(28, 234);
            this.port.MaximumSize = new System.Drawing.Size(40, 16);
            this.port.MaxLength = 5;
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(40, 16);
            this.port.TabIndex = 1;
            // 
            // address
            // 
            this.address.Font = new System.Drawing.Font("Tahoma", 8F);
            this.address.Location = new System.Drawing.Point(113, 234);
            this.address.MaximumSize = new System.Drawing.Size(95, 16);
            this.address.MaxLength = 15;
            this.address.Name = "address";
            this.address.Size = new System.Drawing.Size(95, 16);
            this.address.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "TCP",
            "UDP"});
            this.comboBox1.Location = new System.Drawing.Point(247, 234);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(40, 17);
            this.comboBox1.TabIndex = 3;
            // 
            // refresh
            // 
            this.refresh.Font = new System.Drawing.Font("Tahoma", 8F);
            this.refresh.Location = new System.Drawing.Point(334, 2);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(50, 20);
            this.refresh.TabIndex = 6;
            this.refresh.Text = "Refresh";
            this.refresh.Click += new System.EventHandler(this.RefreshClick);
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(302, 233);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(40, 20);
            this.add.TabIndex = 4;
            this.add.Text = "Add";
            this.add.Click += new System.EventHandler(this.ButtonAddOnClick);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(344, 233);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(40, 20);
            this.delete.TabIndex = 5;
            this.delete.Text = "Rem";
            this.delete.Click += new System.EventHandler(this.ButtonDeleteOnClick);
            // 
            // UPnPForm
            // 
            this.ClientSize = new System.Drawing.Size(391, 258);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.results);
            this.Controls.Add(this.port);
            this.Controls.Add(this.address);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.add);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "UPnPForm";
            this.ShowIcon = false;
            this.Text = "UPnP NAT Utility";
            this.TextExtra = "FreeMeter Revival";
            this.Load += new System.EventHandler(this.frmUPnP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
            

        
    }
}
