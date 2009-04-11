namespace FreeMeterRevival.Forms
{
    partial class OptionsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tbGeneral = new System.Windows.Forms.TabPage();
            this.btnUploadColor = new ComponentFactory.Krypton.Toolkit.KryptonColorButton();
            this.btnOverlapColor = new ComponentFactory.Krypton.Toolkit.KryptonColorButton();
            this.btnDownloadColor = new ComponentFactory.Krypton.Toolkit.KryptonColorButton();
            this.tpGraph = new System.Windows.Forms.TabPage();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPaletteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.kryptonPaletteBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.paletteListStateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabOptions.SuspendLayout();
            this.tbGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPaletteBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPaletteBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteListStateBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabOptions
            // 
            this.tabOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabOptions.Controls.Add(this.tbGeneral);
            this.tabOptions.Controls.Add(this.tpGraph);
            this.tabOptions.Location = new System.Drawing.Point(12, 12);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(331, 248);
            this.tabOptions.TabIndex = 1;
            // 
            // tbGeneral
            // 
            this.tbGeneral.Controls.Add(this.btnUploadColor);
            this.tbGeneral.Controls.Add(this.btnOverlapColor);
            this.tbGeneral.Controls.Add(this.btnDownloadColor);
            this.tbGeneral.Location = new System.Drawing.Point(4, 22);
            this.tbGeneral.Name = "tbGeneral";
            this.tbGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tbGeneral.Size = new System.Drawing.Size(323, 222);
            this.tbGeneral.TabIndex = 1;
            this.tbGeneral.Text = "General";
            this.tbGeneral.UseVisualStyleBackColor = true;
            // 
            // btnUploadColor
            // 
            this.btnUploadColor.DataBindings.Add(new System.Windows.Forms.Binding("SelectedColor", global::FreeMeterRevival.Properties.Settings.Default, "UploadColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnUploadColor.Location = new System.Drawing.Point(102, 6);
            this.btnUploadColor.Name = "btnUploadColor";
            this.btnUploadColor.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnUploadColor.SelectedColor = global::FreeMeterRevival.Properties.Settings.Default.UploadColor;
            this.btnUploadColor.Size = new System.Drawing.Size(90, 25);
            this.btnUploadColor.Splitter = false;
            this.btnUploadColor.TabIndex = 0;
            this.btnUploadColor.Text = "Upload";
            this.btnUploadColor.Values.ExtraText = "";
            this.btnUploadColor.Values.Image = global::FreeMeterRevival.Properties.Resources.chart_bar;
            this.btnUploadColor.Values.ImageStates.ImageCheckedNormal = null;
            this.btnUploadColor.Values.ImageStates.ImageCheckedPressed = null;
            this.btnUploadColor.Values.ImageStates.ImageCheckedTracking = null;
            this.btnUploadColor.Values.Text = "Upload";
            this.btnUploadColor.VisibleNoColor = false;
            // 
            // btnOverlapColor
            // 
            this.btnOverlapColor.DataBindings.Add(new System.Windows.Forms.Binding("SelectedColor", global::FreeMeterRevival.Properties.Settings.Default, "OverlapColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnOverlapColor.Location = new System.Drawing.Point(198, 6);
            this.btnOverlapColor.Name = "btnOverlapColor";
            this.btnOverlapColor.SelectedColor = global::FreeMeterRevival.Properties.Settings.Default.OverlapColor;
            this.btnOverlapColor.Size = new System.Drawing.Size(90, 25);
            this.btnOverlapColor.Splitter = false;
            this.btnOverlapColor.TabIndex = 0;
            this.btnOverlapColor.Text = "Overlap";
            this.btnOverlapColor.Values.ExtraText = "";
            this.btnOverlapColor.Values.Image = global::FreeMeterRevival.Properties.Resources.chart_bar;
            this.btnOverlapColor.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOverlapColor.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOverlapColor.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOverlapColor.Values.Text = "Overlap";
            this.btnOverlapColor.VisibleNoColor = false;
            // 
            // btnDownloadColor
            // 
            this.btnDownloadColor.DataBindings.Add(new System.Windows.Forms.Binding("SelectedColor", global::FreeMeterRevival.Properties.Settings.Default, "DownloadColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnDownloadColor.Location = new System.Drawing.Point(6, 6);
            this.btnDownloadColor.Name = "btnDownloadColor";
            this.btnDownloadColor.SelectedColor = global::FreeMeterRevival.Properties.Settings.Default.DownloadColor;
            this.btnDownloadColor.Size = new System.Drawing.Size(90, 25);
            this.btnDownloadColor.Splitter = false;
            this.btnDownloadColor.TabIndex = 0;
            this.btnDownloadColor.Text = "Download";
            this.btnDownloadColor.Values.ExtraText = "";
            this.btnDownloadColor.Values.Image = global::FreeMeterRevival.Properties.Resources.chart_bar;
            this.btnDownloadColor.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDownloadColor.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDownloadColor.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDownloadColor.Values.Text = "Download";
            this.btnDownloadColor.VisibleNoColor = false;
            // 
            // tpGraph
            // 
            this.tpGraph.Location = new System.Drawing.Point(4, 22);
            this.tpGraph.Name = "tpGraph";
            this.tpGraph.Padding = new System.Windows.Forms.Padding(3);
            this.tpGraph.Size = new System.Drawing.Size(323, 222);
            this.tpGraph.TabIndex = 0;
            this.tpGraph.Text = "Graph";
            this.tpGraph.UseVisualStyleBackColor = true;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOK);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(355, 301);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(253, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(157, 264);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.Values.ExtraText = "";
            this.btnOK.Values.Image = null;
            this.btnOK.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOK.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOK.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOK.Values.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // kryptonManager1
            // 
            this.kryptonManager1.GlobalPaletteMode = global::FreeMeterRevival.Properties.Settings.Default.GlobalPaletteMode;
            // 
            // kryptonPaletteBindingSource
            // 
            this.kryptonPaletteBindingSource.DataSource = typeof(ComponentFactory.Krypton.Toolkit.KryptonPalette);
            // 
            // kryptonPaletteBindingSource1
            // 
            this.kryptonPaletteBindingSource1.DataSource = typeof(ComponentFactory.Krypton.Toolkit.KryptonPalette);
            // 
            // paletteListStateBindingSource
            // 
            this.paletteListStateBindingSource.DataSource = typeof(ComponentFactory.Krypton.Toolkit.PaletteListState);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(355, 301);
            this.Controls.Add(this.tabOptions);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.TextExtra = "FreeMeter Revival";
            this.TopMost = true;
            this.tabOptions.ResumeLayout(false);
            this.tbGeneral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPaletteBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPaletteBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteListStateBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabOptions;
        private System.Windows.Forms.TabPage tpGraph;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
        private System.Windows.Forms.TabPage tbGeneral;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonColorButton btnDownloadColor;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOK;
        private ComponentFactory.Krypton.Toolkit.KryptonColorButton btnUploadColor;
        private ComponentFactory.Krypton.Toolkit.KryptonColorButton btnOverlapColor;
        private System.Windows.Forms.BindingSource kryptonPaletteBindingSource;
        private System.Windows.Forms.BindingSource kryptonPaletteBindingSource1;
        private System.Windows.Forms.BindingSource paletteListStateBindingSource;
    }
}