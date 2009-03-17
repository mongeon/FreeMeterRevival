using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;

namespace FreeMeterRevival.Forms
{
    public partial class AboutForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public AboutForm()
        {
            InitializeComponent();
        }
        internal static void ShowAboutForm(IWin32Window Owner)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog(Owner);
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            Assembly ThisAssembly = Assembly.GetExecutingAssembly();
            AssemblyName ThisAssemblyName = ThisAssembly.GetName();
            this.Icon = Owner.Icon;
            IconBox1.Image = this.Icon.ToBitmap();
            Stream s = ThisAssembly.GetManifestResourceStream("FreeMeterRevival.lr.ico");
            Icon lr = new Icon(s);
            s = ThisAssembly.GetManifestResourceStream("FreeMeterRevival.ly.ico");
            Icon ly = new Icon(s);
            IconBox2.Image = lr.ToBitmap();
            IconBox3.Image = ly.ToBitmap();
            lr.Dispose();
            ly.Dispose();
            s.Close();
            DateTime lastmodified = File.GetLastWriteTime(Application.ExecutablePath);
            string FriendlyVersion = ThisAssemblyName.Version.Major + "." + ThisAssemblyName.Version.Minor + "." + ThisAssemblyName.Version.Build + "\nBuilt " + lastmodified.ToString();
            Array Attributes = ThisAssembly.GetCustomAttributes(false);
            string Title = "Unknown Application";
            string Copyright = "Unknown Copyright";
            foreach (object o in Attributes)
                if (o is AssemblyTitleAttribute)
                    Title = ((AssemblyTitleAttribute)o).Title;
                else if (o is AssemblyCopyrightAttribute)
                    Copyright = ((AssemblyCopyrightAttribute)o).Copyright;
            this.Text = "About";

            txtArea.Text = Title + " v" + FriendlyVersion + "\n\n" + Copyright;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lnkWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://freemeterrevival.codeplex.com/");
        }

        private void btnLicense_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Copyright © 2009 Gabriel Mongeon \n\nCopyright © 2005-2007, David Schultz, Mieszko Lassota All rights reserved.\n\n" +
             "Check http://freemeterrevival.codeplex.com/ for latest version and contact info.\n\n" +
             "Redistribution and use in source and binary forms, with or without " +
             "modification,\nare permitted provided that the following conditions " +
             "are met:\n\n" +
             "- Redistributions of source code must retain the above copyright " +
             "notice, this list\nof conditions and the following disclaimer.\n\n" +
             "- Neither the name of the owner, nor the names of its " +
             "contributors may be used\nto endorse or promote products " +
             "derived from this software without specific prior\nwritten " +
             "permission.\n\n" +
             "This software is provided by the copyright holders and contributors " +
             "\"as is\" and\nany express or implied warranties, including, but not " +
             "limited to, the implied\nwarranties of merchantability and fitness " +
             "for a particular purpose are disclaimed.\nIn no event shall the " +
             "copyright owner or contributors be liable for any direct,\nindirect, " +
             "incidental, special, exemplary, or consequential damages including, " +
             "but\nnot limited to, procurement of substitute goods or services; " +
             "loss of use, data, or\nprofits; or business interruption) however " +
             "caused and on any theory of liability,\nwhether in contract, strict " +
             "liability, or tort (including negligence or otherwise)\narising in " +
             "any way out of the use of this software, even if advised of the " +
             "possibility\nof such damage.", "FreeMeter Revival GPL License"
           );
        }
    }
}
