namespace FreeMeterRevival.Forms
{
    partial class ErrorForm
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
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.button_copy = new System.Windows.Forms.Button();
            this.button_exit = new System.Windows.Forms.Button();
            this.button_restart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.BackColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(13, 13);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(577, 245);
            this.txtMessage.TabIndex = 0;
            // 
            // button_copy
            // 
            this.button_copy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_copy.Location = new System.Drawing.Point(12, 264);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(107, 21);
            this.button_copy.TabIndex = 1;
            this.button_copy.Text = "Copy to clipboard";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.Click += new System.EventHandler(this.button_copy_Click);
            // 
            // button_exit
            // 
            this.button_exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_exit.Location = new System.Drawing.Point(514, 264);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(75, 21);
            this.button_exit.TabIndex = 2;
            this.button_exit.Text = "exit";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // button_restart
            // 
            this.button_restart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_restart.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_restart.Location = new System.Drawing.Point(423, 264);
            this.button_restart.Name = "button_restart";
            this.button_restart.Size = new System.Drawing.Size(75, 21);
            this.button_restart.TabIndex = 3;
            this.button_restart.Text = "restart";
            this.button_restart.UseVisualStyleBackColor = true;
            this.button_restart.Click += new System.EventHandler(this.button_restart_Click);
            // 
            // ErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 297);
            this.Controls.Add(this.button_restart);
            this.Controls.Add(this.button_exit);
            this.Controls.Add(this.button_copy);
            this.Controls.Add(this.txtMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ErrorForm";
            this.Text = "FreeMeter Revival - Fatal Error";
            this.Load += new System.EventHandler(this.Error_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button button_copy;
        private System.Windows.Forms.Button button_exit;
        private System.Windows.Forms.Button button_restart;
    }
}