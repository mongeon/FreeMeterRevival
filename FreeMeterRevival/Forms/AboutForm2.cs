using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace FreeMeterRevival.Forms
{
    //Just a simple about form to be called like AboutForm.ShowAboutForm(this);
    public class AboutForm2 : Form
    {

      
        private Label Legend;
        private Label dl;
        private Label ul;
        private Label du;
        private AboutForm2()
        {

            Legend = new Label();
            dl = new Label();
            ul = new Label();
            du = new Label();
            // IconBoxes
            
            // Legend
            Legend.Location = new Point(4, 10);
            Legend.Name = "Legend";
            Legend.Size = new Size(50, 13);
            Legend.Text = "Legend:";
            Legend.Font = new Font("Tahoma", 8);
            dl.Location = new Point(32, 25);
            dl.Name = "dl";
            dl.Size = new Size(60, 13);
            dl.Text = "Download";
            dl.Font = new Font("Tahoma", 8);
            ul.Location = new Point(32, 43);
            ul.Name = "ul";
            ul.Size = new Size(50, 13);
            ul.Text = "Upload";
            ul.Font = new Font("Tahoma", 8);
            du.Location = new Point(32, 61);
            du.Name = "du";
            du.Size = new Size(50, 13);
            du.Text = "Both";
            du.Font = new Font("Tahoma", 8);
            //link

            

            
        }

    }
}
