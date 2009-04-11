using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FreeMeterRevival.Forms
{
    ///============================================================================================
    /// <summary>
    /// This form manage all the options for this application
    /// </summary>
    ///============================================================================================
    public partial class OptionsForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public OptionsForm()
        {
            InitializeComponent();
        }
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Save the settings and close the form.
        /// </summary>
        ///----------------------------------------------------------------------------------------
        private void btnOK_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.DownloadColor = btnDownloadColor.SelectedColor;
            Properties.Settings.Default.UploadColor = btnUploadColor.SelectedColor;
            Properties.Settings.Default.OverlapColor = btnOverlapColor.SelectedColor;
            Properties.Settings.Default.Save();
            this.Close();
        }
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Roll back the changes and close the window.
        /// </summary>
        ///----------------------------------------------------------------------------------------
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            this.Close();
        }
    }
}
