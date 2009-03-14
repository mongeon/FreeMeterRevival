namespace FreeMeterRevival.Forms
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.IconBox1 = new System.Windows.Forms.PictureBox();
            this.IconBox3 = new System.Windows.Forms.PictureBox();
            this.IconBox2 = new System.Windows.Forms.PictureBox();
            this.txtArea = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.lnkWebsite = new System.Windows.Forms.LinkLabel();
            this.btnLicense = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // IconBox1
            // 
            this.IconBox1.Location = new System.Drawing.Point(12, 25);
            this.IconBox1.Name = "IconBox1";
            this.IconBox1.Size = new System.Drawing.Size(16, 16);
            this.IconBox1.TabIndex = 0;
            this.IconBox1.TabStop = false;
            // 
            // IconBox3
            // 
            this.IconBox3.Location = new System.Drawing.Point(12, 61);
            this.IconBox3.Name = "IconBox3";
            this.IconBox3.Size = new System.Drawing.Size(16, 16);
            this.IconBox3.TabIndex = 1;
            this.IconBox3.TabStop = false;
            // 
            // IconBox2
            // 
            this.IconBox2.Location = new System.Drawing.Point(12, 43);
            this.IconBox2.Name = "IconBox2";
            this.IconBox2.Size = new System.Drawing.Size(16, 16);
            this.IconBox2.TabIndex = 2;
            this.IconBox2.TabStop = false;
            // 
            // txtArea
            // 
            this.txtArea.Location = new System.Drawing.Point(34, 10);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(288, 125);
            this.txtArea.TabIndex = 3;
            this.txtArea.Text = "label1";
            this.txtArea.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(109, 193);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(55, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lnkWebsite
            // 
            this.lnkWebsite.AutoSize = true;
            this.lnkWebsite.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkWebsite.Location = new System.Drawing.Point(74, 135);
            this.lnkWebsite.Name = "lnkWebsite";
            this.lnkWebsite.Size = new System.Drawing.Size(187, 13);
            this.lnkWebsite.TabIndex = 4;
            this.lnkWebsite.TabStop = true;
            this.lnkWebsite.Text = "http://freemeterrevival.codeplex.com/";
            this.lnkWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWebsite_LinkClicked);
            // 
            // btnLicense
            // 
            this.btnLicense.Location = new System.Drawing.Point(170, 193);
            this.btnLicense.Name = "btnLicense";
            this.btnLicense.Size = new System.Drawing.Size(55, 23);
            this.btnLicense.TabIndex = 1;
            this.btnLicense.Text = "License";
            this.btnLicense.UseVisualStyleBackColor = true;
            this.btnLicense.Click += new System.EventHandler(this.btnLicense_Click);
            // 
            // AboutForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(334, 228);
            this.ControlBox = false;
            this.Controls.Add(this.btnLicense);
            this.Controls.Add(this.lnkWebsite);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtArea);
            this.Controls.Add(this.IconBox2);
            this.Controls.Add(this.IconBox3);
            this.Controls.Add(this.IconBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About:";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.IconBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox IconBox1;
        private System.Windows.Forms.PictureBox IconBox3;
        private System.Windows.Forms.PictureBox IconBox2;
        private System.Windows.Forms.Label txtArea;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.LinkLabel lnkWebsite;
        private System.Windows.Forms.Button btnLicense;
    }
}