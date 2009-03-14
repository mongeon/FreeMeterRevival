using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FreeMeterRevival.Forms
{
    public partial class MainForm : Form
    {
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.m_menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.show_checked = new System.Windows.Forms.ToolStripMenuItem();
            this.topmost_checked = new System.Windows.Forms.ToolStripMenuItem();
            this.simple_icon_checked = new System.Windows.Forms.ToolStripMenuItem();
            this.m_colors = new System.Windows.Forms.ToolStripMenuItem();
            this.colorcycle = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.hue_slider = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.avg_checked = new System.Windows.Forms.ToolStripMenuItem();
            this.m_interval_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.interval_tenth = new System.Windows.Forms.ToolStripMenuItem();
            this.interval_fifth = new System.Windows.Forms.ToolStripMenuItem();
            this.interval_half = new System.Windows.Forms.ToolStripMenuItem();
            this.interval_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.m_scale_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.autoscale_checked = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_33 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_56 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_64 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_128 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_256 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_512 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_640 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_1000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_1500 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_2000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_3000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_5000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_7000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_10000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_11000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_32000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_54000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_100000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_1000000 = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_custom = new System.Windows.Forms.ToolStripMenuItem();
            this.m_graphs = new System.Windows.Forms.ToolStripMenuItem();
            this.graph_label_checked = new System.Windows.Forms.ToolStripMenuItem();
            this.graphs_summary = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.graphs_download = new System.Windows.Forms.ToolStripMenuItem();
            this.graphs_upload = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.font_large = new System.Windows.Forms.ToolStripMenuItem();
            this.font_medium = new System.Windows.Forms.ToolStripMenuItem();
            this.font_small = new System.Windows.Forms.ToolStripMenuItem();
            this.m_units = new System.Windows.Forms.ToolStripMenuItem();
            this.units_kbits = new System.Windows.Forms.ToolStripMenuItem();
            this.units_kbytes = new System.Windows.Forms.ToolStripMenuItem();
            this.m_interfaces = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_utils = new System.Windows.Forms.ToolStripMenuItem();
            this.clip_watch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mailcheck = new System.Windows.Forms.ToolStripMenuItem();
            this.mailchecknow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem15 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem18 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem19 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem21 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.about_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.exit_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.resizer = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FullMeter = new System.Windows.Forms.PictureBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.m_notifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ClipTimer = new System.Windows.Forms.Timer(this.components);
            this.MailTimer = new System.Windows.Forms.Timer(this.components);
            this.ShrinkTimer = new System.Windows.Forms.Timer(this.components);
            this.LogTimer = new System.Windows.Forms.Timer(this.components);
            this.m_menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resizer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullMeter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // m_menu
            // 
            this.m_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.show_checked,
            this.topmost_checked,
            this.simple_icon_checked,
            this.m_colors,
            this.toolStripMenuItem1,
            this.avg_checked,
            this.m_interval_menu,
            this.m_scale_menu,
            this.m_graphs,
            this.m_units,
            this.m_interfaces,
            this.toolStripSeparator1,
            this.m_utils,
            this.toolStripSeparator2,
            this.about_menu,
            this.exit_menu});
            this.m_menu.Name = "m_menu";
            this.m_menu.ShowImageMargin = false;
            this.m_menu.Size = new System.Drawing.Size(178, 308);
            // 
            // show_checked
            // 
            this.show_checked.Checked = true;
            this.show_checked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.show_checked.Name = "show_checked";
            this.show_checked.Size = new System.Drawing.Size(177, 22);
            this.show_checked.Text = "Show Desktop Meter";
            this.show_checked.Click += new System.EventHandler(this.Show_Click);
            // 
            // topmost_checked
            // 
            this.topmost_checked.Name = "topmost_checked";
            this.topmost_checked.Size = new System.Drawing.Size(177, 22);
            this.topmost_checked.Text = "Always on Top";
            // 
            // simple_icon_checked
            // 
            this.simple_icon_checked.Name = "simple_icon_checked";
            this.simple_icon_checked.Size = new System.Drawing.Size(177, 22);
            this.simple_icon_checked.Text = "Simple Notify Icon";
            this.simple_icon_checked.Click += new System.EventHandler(this.SimpleNotifyIcon_Click);
            // 
            // m_colors
            // 
            this.m_colors.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorcycle,
            this.menuItem5,
            this.hue_slider,
            this.menuItem6,
            this.menuItem7,
            this.menuItem8,
            this.menuItem9,
            this.menuItem10,
            this.menuItem11});
            this.m_colors.Name = "m_colors";
            this.m_colors.Size = new System.Drawing.Size(177, 22);
            this.m_colors.Text = "Colors/Opacity";
            // 
            // colorcycle
            // 
            this.colorcycle.Name = "colorcycle";
            this.colorcycle.Size = new System.Drawing.Size(180, 22);
            this.colorcycle.Text = "Cycle Colors";
            this.colorcycle.Click += new System.EventHandler(this.Cycle_Colors);
            // 
            // menuItem5
            // 
            this.menuItem5.Name = "menuItem5";
            this.menuItem5.Size = new System.Drawing.Size(177, 6);
            // 
            // hue_slider
            // 
            this.hue_slider.Name = "hue_slider";
            this.hue_slider.Size = new System.Drawing.Size(180, 22);
            this.hue_slider.Text = "Hue Slider";
            this.hue_slider.Click += new System.EventHandler(this.Trackbar1_Show);
            // 
            // menuItem6
            // 
            this.menuItem6.Name = "menuItem6";
            this.menuItem6.Size = new System.Drawing.Size(180, 22);
            this.menuItem6.Text = "Color";
            this.menuItem6.Click += new System.EventHandler(this.Color_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Name = "menuItem7";
            this.menuItem7.Size = new System.Drawing.Size(180, 22);
            this.menuItem7.Text = "Text Color";
            this.menuItem7.Click += new System.EventHandler(this.TextColor_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Name = "menuItem8";
            this.menuItem8.Size = new System.Drawing.Size(180, 22);
            this.menuItem8.Text = "Reset To Default";
            this.menuItem8.Click += new System.EventHandler(this.DefaultColor_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Name = "menuItem9";
            this.menuItem9.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItem10
            // 
            this.menuItem10.Name = "menuItem10";
            this.menuItem10.Size = new System.Drawing.Size(180, 22);
            this.menuItem10.Text = "Transparency Slider";
            this.menuItem10.Click += new System.EventHandler(this.Trackbar2_Show);
            // 
            // menuItem11
            // 
            this.menuItem11.Name = "menuItem11";
            this.menuItem11.Size = new System.Drawing.Size(180, 22);
            this.menuItem11.Text = "Opaque";
            this.menuItem11.Click += new System.EventHandler(this.Opaque_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(174, 6);
            // 
            // avg_checked
            // 
            this.avg_checked.Name = "avg_checked";
            this.avg_checked.Size = new System.Drawing.Size(177, 22);
            this.avg_checked.Text = "Display Averages";
            this.avg_checked.Click += new System.EventHandler(this.Avg_Click);
            // 
            // m_interval_menu
            // 
            this.m_interval_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.interval_tenth,
            this.interval_fifth,
            this.interval_half,
            this.interval_1});
            this.m_interval_menu.Name = "m_interval_menu";
            this.m_interval_menu.Size = new System.Drawing.Size(177, 22);
            this.m_interval_menu.Text = "Update Interval";
            // 
            // interval_tenth
            // 
            this.interval_tenth.Name = "interval_tenth";
            this.interval_tenth.Size = new System.Drawing.Size(144, 22);
            this.interval_tenth.Text = "1/10 second";
            this.interval_tenth.Click += new System.EventHandler(this.SetTimerInterval);
            // 
            // interval_fifth
            // 
            this.interval_fifth.Name = "interval_fifth";
            this.interval_fifth.Size = new System.Drawing.Size(144, 22);
            this.interval_fifth.Text = "1/5 second";
            this.interval_fifth.Click += new System.EventHandler(this.SetTimerInterval);
            // 
            // interval_half
            // 
            this.interval_half.Name = "interval_half";
            this.interval_half.Size = new System.Drawing.Size(144, 22);
            this.interval_half.Text = "1/2 second";
            this.interval_half.Click += new System.EventHandler(this.SetTimerInterval);
            // 
            // interval_1
            // 
            this.interval_1.Name = "interval_1";
            this.interval_1.Size = new System.Drawing.Size(144, 22);
            this.interval_1.Text = "1 second";
            this.interval_1.Click += new System.EventHandler(this.SetTimerInterval);
            // 
            // m_scale_menu
            // 
            this.m_scale_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoscale_checked,
            this.scale_33,
            this.scale_56,
            this.scale_64,
            this.scale_128,
            this.scale_256,
            this.scale_512,
            this.scale_640,
            this.scale_1000,
            this.scale_1500,
            this.scale_2000,
            this.scale_3000,
            this.scale_5000,
            this.scale_7000,
            this.scale_10000,
            this.scale_11000,
            this.scale_32000,
            this.scale_54000,
            this.scale_100000,
            this.scale_1000000,
            this.scale_custom});
            this.m_scale_menu.Name = "m_scale_menu";
            this.m_scale_menu.Size = new System.Drawing.Size(177, 22);
            this.m_scale_menu.Text = "Graph Scale";
            // 
            // autoscale_checked
            // 
            this.autoscale_checked.Name = "autoscale_checked";
            this.autoscale_checked.Size = new System.Drawing.Size(121, 22);
            this.autoscale_checked.Text = "Auto";
            this.autoscale_checked.Click += new System.EventHandler(this.SetAutoScale);
            // 
            // scale_33
            // 
            this.scale_33.Name = "scale_33";
            this.scale_33.Size = new System.Drawing.Size(121, 22);
            this.scale_33.Text = "33.6 kb";
            this.scale_33.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_56
            // 
            this.scale_56.Name = "scale_56";
            this.scale_56.Size = new System.Drawing.Size(121, 22);
            this.scale_56.Text = "56 kb";
            this.scale_56.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_64
            // 
            this.scale_64.Name = "scale_64";
            this.scale_64.Size = new System.Drawing.Size(121, 22);
            this.scale_64.Text = "64 kb";
            this.scale_64.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_128
            // 
            this.scale_128.Name = "scale_128";
            this.scale_128.Size = new System.Drawing.Size(121, 22);
            this.scale_128.Text = "128 kb";
            this.scale_128.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_256
            // 
            this.scale_256.Name = "scale_256";
            this.scale_256.Size = new System.Drawing.Size(121, 22);
            this.scale_256.Text = "256 kb";
            this.scale_256.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_512
            // 
            this.scale_512.Name = "scale_512";
            this.scale_512.Size = new System.Drawing.Size(121, 22);
            this.scale_512.Text = "512 kb";
            this.scale_512.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_640
            // 
            this.scale_640.Name = "scale_640";
            this.scale_640.Size = new System.Drawing.Size(121, 22);
            this.scale_640.Text = "640 kb";
            this.scale_640.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_1000
            // 
            this.scale_1000.Name = "scale_1000";
            this.scale_1000.Size = new System.Drawing.Size(121, 22);
            this.scale_1000.Text = "1 mb";
            this.scale_1000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_1500
            // 
            this.scale_1500.Name = "scale_1500";
            this.scale_1500.Size = new System.Drawing.Size(121, 22);
            this.scale_1500.Text = "1.5 mb";
            this.scale_1500.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_2000
            // 
            this.scale_2000.Name = "scale_2000";
            this.scale_2000.Size = new System.Drawing.Size(121, 22);
            this.scale_2000.Text = "2 mb";
            this.scale_2000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_3000
            // 
            this.scale_3000.Name = "scale_3000";
            this.scale_3000.Size = new System.Drawing.Size(121, 22);
            this.scale_3000.Text = "3 mb";
            this.scale_3000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_5000
            // 
            this.scale_5000.Name = "scale_5000";
            this.scale_5000.Size = new System.Drawing.Size(121, 22);
            this.scale_5000.Text = "5 mb";
            this.scale_5000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_7000
            // 
            this.scale_7000.Name = "scale_7000";
            this.scale_7000.Size = new System.Drawing.Size(121, 22);
            this.scale_7000.Text = "7 mb";
            this.scale_7000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_10000
            // 
            this.scale_10000.Name = "scale_10000";
            this.scale_10000.Size = new System.Drawing.Size(121, 22);
            this.scale_10000.Text = "10 mb";
            this.scale_10000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_11000
            // 
            this.scale_11000.Name = "scale_11000";
            this.scale_11000.Size = new System.Drawing.Size(121, 22);
            this.scale_11000.Text = "11 mb";
            this.scale_11000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_32000
            // 
            this.scale_32000.Name = "scale_32000";
            this.scale_32000.Size = new System.Drawing.Size(121, 22);
            this.scale_32000.Text = "32 mb";
            this.scale_32000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_54000
            // 
            this.scale_54000.Name = "scale_54000";
            this.scale_54000.Size = new System.Drawing.Size(121, 22);
            this.scale_54000.Text = "54 mb";
            this.scale_54000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_100000
            // 
            this.scale_100000.Name = "scale_100000";
            this.scale_100000.Size = new System.Drawing.Size(121, 22);
            this.scale_100000.Text = "100 mb";
            this.scale_100000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_1000000
            // 
            this.scale_1000000.Name = "scale_1000000";
            this.scale_1000000.Size = new System.Drawing.Size(121, 22);
            this.scale_1000000.Text = "1 gb";
            this.scale_1000000.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // scale_custom
            // 
            this.scale_custom.Name = "scale_custom";
            this.scale_custom.Size = new System.Drawing.Size(121, 22);
            this.scale_custom.Text = "custom";
            this.scale_custom.Click += new System.EventHandler(this.SetScale_MenuClick);
            // 
            // m_graphs
            // 
            this.m_graphs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graph_label_checked,
            this.graphs_summary,
            this.menuItem12,
            this.graphs_download,
            this.graphs_upload,
            this.menuItem13,
            this.font_large,
            this.font_medium,
            this.font_small});
            this.m_graphs.Name = "m_graphs";
            this.m_graphs.Size = new System.Drawing.Size(177, 22);
            this.m_graphs.Text = "Graphs";
            // 
            // graph_label_checked
            // 
            this.graph_label_checked.Name = "graph_label_checked";
            this.graph_label_checked.Size = new System.Drawing.Size(300, 22);
            this.graph_label_checked.Text = "Show Graph Heading";
            this.graph_label_checked.Click += new System.EventHandler(this.SetGraph_Label);
            // 
            // graphs_summary
            // 
            this.graphs_summary.Name = "graphs_summary";
            this.graphs_summary.Size = new System.Drawing.Size(300, 22);
            this.graphs_summary.Text = "Show Summary On Left(up) and Right(down)";
            this.graphs_summary.Click += new System.EventHandler(this.SetGraph_Summary);
            // 
            // menuItem12
            // 
            this.menuItem12.Name = "menuItem12";
            this.menuItem12.Size = new System.Drawing.Size(297, 6);
            // 
            // graphs_download
            // 
            this.graphs_download.Name = "graphs_download";
            this.graphs_download.Size = new System.Drawing.Size(300, 22);
            this.graphs_download.Text = "Download";
            this.graphs_download.Click += new System.EventHandler(this.SetGraph_Download);
            // 
            // graphs_upload
            // 
            this.graphs_upload.Name = "graphs_upload";
            this.graphs_upload.Size = new System.Drawing.Size(300, 22);
            this.graphs_upload.Text = "Upload";
            this.graphs_upload.Click += new System.EventHandler(this.SetGraph_Upload);
            // 
            // menuItem13
            // 
            this.menuItem13.Name = "menuItem13";
            this.menuItem13.Size = new System.Drawing.Size(297, 6);
            // 
            // font_large
            // 
            this.font_large.Name = "font_large";
            this.font_large.Size = new System.Drawing.Size(300, 22);
            this.font_large.Text = "Large Font";
            this.font_large.Click += new System.EventHandler(this.SetFont_Large);
            // 
            // font_medium
            // 
            this.font_medium.Name = "font_medium";
            this.font_medium.Size = new System.Drawing.Size(300, 22);
            this.font_medium.Text = "Medium Font";
            this.font_medium.Click += new System.EventHandler(this.SetFont_Medium);
            // 
            // font_small
            // 
            this.font_small.Name = "font_small";
            this.font_small.Size = new System.Drawing.Size(300, 22);
            this.font_small.Text = "Small Font";
            this.font_small.Click += new System.EventHandler(this.SetFont_Small);
            // 
            // m_units
            // 
            this.m_units.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.units_kbits,
            this.units_kbytes});
            this.m_units.Name = "m_units";
            this.m_units.Size = new System.Drawing.Size(177, 22);
            this.m_units.Text = "Units";
            // 
            // units_kbits
            // 
            this.units_kbits.Name = "units_kbits";
            this.units_kbits.Size = new System.Drawing.Size(196, 22);
            this.units_kbits.Text = "Bits per sec (eg kbps)";
            this.units_kbits.Click += new System.EventHandler(this.SetUnits_kbits);
            // 
            // units_kbytes
            // 
            this.units_kbytes.Name = "units_kbytes";
            this.units_kbytes.Size = new System.Drawing.Size(196, 22);
            this.units_kbytes.Text = "Bytes per sec (eg kB/s)";
            this.units_kbytes.Click += new System.EventHandler(this.SetUnits_kbits);
            // 
            // m_interfaces
            // 
            this.m_interfaces.Name = "m_interfaces";
            this.m_interfaces.Size = new System.Drawing.Size(177, 22);
            this.m_interfaces.Text = "Interfaces";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(174, 6);
            // 
            // m_utils
            // 
            this.m_utils.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clip_watch,
            this.menuItem1,
            this.mailcheck,
            this.mailchecknow,
            this.menuItem14,
            this.menuItem15,
            this.menuItem16,
            this.menuItem17,
            this.menuItem18,
            this.menuItem19,
            this.menuItem20,
            this.menuItem21});
            this.m_utils.Name = "m_utils";
            this.m_utils.Size = new System.Drawing.Size(177, 22);
            this.m_utils.Text = "Utilities";
            // 
            // clip_watch
            // 
            this.clip_watch.Name = "clip_watch";
            this.clip_watch.Size = new System.Drawing.Size(187, 22);
            this.clip_watch.Text = "URL Grabber Enabled";
            this.clip_watch.Click += new System.EventHandler(this.Clip_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Name = "menuItem1";
            this.menuItem1.Size = new System.Drawing.Size(184, 6);
            // 
            // mailcheck
            // 
            this.mailcheck.Name = "mailcheck";
            this.mailcheck.Size = new System.Drawing.Size(187, 22);
            this.mailcheck.Text = "Email Notify Enabled";
            this.mailcheck.Click += new System.EventHandler(this.CheckMail_Auto);
            // 
            // mailchecknow
            // 
            this.mailchecknow.Name = "mailchecknow";
            this.mailchecknow.Size = new System.Drawing.Size(187, 22);
            this.mailchecknow.Text = "Check Email Now";
            this.mailchecknow.Click += new System.EventHandler(this.CheckMail_Now);
            // 
            // menuItem14
            // 
            this.menuItem14.Name = "menuItem14";
            this.menuItem14.Size = new System.Drawing.Size(187, 22);
            this.menuItem14.Text = "Email Server Settings";
            this.menuItem14.Click += new System.EventHandler(this.CheckMail_Settings);
            // 
            // menuItem15
            // 
            this.menuItem15.Name = "menuItem15";
            this.menuItem15.Size = new System.Drawing.Size(184, 6);
            // 
            // menuItem16
            // 
            this.menuItem16.Name = "menuItem16";
            this.menuItem16.Size = new System.Drawing.Size(187, 22);
            this.menuItem16.Text = "Ping Utility";
            this.menuItem16.Click += new System.EventHandler(this.Ping_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Name = "menuItem17";
            this.menuItem17.Size = new System.Drawing.Size(187, 22);
            this.menuItem17.Text = "Traceroute Utility";
            this.menuItem17.Click += new System.EventHandler(this.Trace_Click);
            // 
            // menuItem18
            // 
            this.menuItem18.Name = "menuItem18";
            this.menuItem18.Size = new System.Drawing.Size(187, 22);
            this.menuItem18.Text = "UPnP NAT Utility";
            this.menuItem18.Click += new System.EventHandler(this.UPnP_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Name = "menuItem19";
            this.menuItem19.Size = new System.Drawing.Size(184, 6);
            // 
            // menuItem20
            // 
            this.menuItem20.Name = "menuItem20";
            this.menuItem20.Size = new System.Drawing.Size(187, 22);
            this.menuItem20.Text = "Totals Log";
            this.menuItem20.Click += new System.EventHandler(this.ShowTotalsLog_Click);
            // 
            // menuItem21
            // 
            this.menuItem21.Name = "menuItem21";
            this.menuItem21.Size = new System.Drawing.Size(187, 22);
            this.menuItem21.Text = "Check For Updates";
            this.menuItem21.Click += new System.EventHandler(this.Check_Version);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(174, 6);
            // 
            // about_menu
            // 
            this.about_menu.Name = "about_menu";
            this.about_menu.Size = new System.Drawing.Size(177, 22);
            this.about_menu.Text = "About FreeMeterRevival";
            this.about_menu.Click += new System.EventHandler(this.About_Click);
            // 
            // exit_menu
            // 
            this.exit_menu.Name = "exit_menu";
            this.exit_menu.Size = new System.Drawing.Size(177, 22);
            this.exit_menu.Text = "Exit FreeMeterRevival";
            this.exit_menu.Click += new System.EventHandler(this.Exit_Click);
            // 
            // resizer
            // 
            this.resizer.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.resizer.Location = new System.Drawing.Point(0, 0);
            this.resizer.Name = "resizer";
            this.resizer.Size = new System.Drawing.Size(11, 11);
            this.resizer.TabIndex = 0;
            this.resizer.TabStop = false;
            this.resizer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Resize_MouseMove);
            this.resizer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Resize_MouseDown);
            this.resizer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Resize_MouseUp);
            // 
            // label1
            // 
            this.label1.ContextMenuStrip = this.m_menu;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            // 
            // label2
            // 
            this.label2.ContextMenuStrip = this.m_menu;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 1;
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            // 
            // label3
            // 
            this.label3.ContextMenuStrip = this.m_menu;
            this.label3.Font = new System.Drawing.Font("Wingdings", 7F);
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "ê";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            // 
            // label4
            // 
            this.label4.ContextMenuStrip = this.m_menu;
            this.label4.Font = new System.Drawing.Font("Wingdings", 7F);
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(9, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "é";
            this.label4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            // 
            // FullMeter
            // 
            this.FullMeter.BackColor = System.Drawing.Color.Black;
            this.FullMeter.ContextMenuStrip = this.m_menu;
            this.FullMeter.Location = new System.Drawing.Point(3, 3);
            this.FullMeter.Name = "FullMeter";
            this.FullMeter.Size = new System.Drawing.Size(314, 222);
            this.FullMeter.TabIndex = 0;
            this.FullMeter.TabStop = false;
            this.FullMeter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.FullMeter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.Location = new System.Drawing.Point(0, 0);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(0);
            this.trackBar1.Maximum = 360;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickFrequency = 45;
            this.trackBar1.Visible = false;
            this.trackBar1.ValueChanged += new System.EventHandler(this.Trackbar1_Show);
            this.trackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Trackbar1_Hide);
            // 
            // trackBar2
            // 
            this.trackBar2.AutoSize = false;
            this.trackBar2.LargeChange = 10;
            this.trackBar2.Location = new System.Drawing.Point(0, 0);
            this.trackBar2.Margin = new System.Windows.Forms.Padding(0);
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(104, 45);
            this.trackBar2.TabIndex = 1;
            this.trackBar2.TickFrequency = 10;
            this.trackBar2.Visible = false;
            this.trackBar2.ValueChanged += new System.EventHandler(this.Trackbar2_Update);
            this.trackBar2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Trackbar2_Hide);
            // 
            // m_notifyicon
            // 
            this.m_notifyicon.ContextMenuStrip = this.m_menu;
            this.m_notifyicon.Icon = ((System.Drawing.Icon)(resources.GetObject("m_notifyicon.Icon")));
            this.m_notifyicon.Text = "Free Meter Revival";
            this.m_notifyicon.Visible = true;
            this.m_notifyicon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Icon_MouseDown);
            // 
            // ClipTimer
            // 
            this.ClipTimer.Interval = 1000;
            this.ClipTimer.Tick += new System.EventHandler(this.ClipTimer_Tick);
            // 
            // MailTimer
            // 
            this.MailTimer.Tick += new System.EventHandler(this.MailTimer_Tick);
            // 
            // ShrinkTimer
            // 
            this.ShrinkTimer.Interval = 60000;
            this.ShrinkTimer.Tick += new System.EventHandler(this.ShrinkTimer_Tick);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(320, 240);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.FullMeter);
            this.Controls.Add(this.trackBar2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 480);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(320, 240);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.m_menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resizer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullMeter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.ResumeLayout(false);

        }
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private PictureBox FullMeter;
        private PictureBox resizer;
        private ContextMenuStrip m_menu;
        private TrackBar trackBar1 ;
        private TrackBar trackBar2 ;
        private ColorDialog colorDialog1;
        // Menus and menu click handlers
        private ToolStripMenuItem m_interval_menu;
        private ToolStripMenuItem m_scale_menu;
        private ToolStripMenuItem m_units;
        private ToolStripMenuItem m_interfaces;
        private ToolStripMenuItem m_graphs;
        private ToolStripMenuItem m_colors, about_menu, exit_menu;
        private ToolStripMenuItem m_utils;
        private ToolStripMenuItem interval_tenth, interval_fifth, interval_half, interval_1;
        private ToolStripMenuItem scale_33, scale_56, scale_64, scale_128, scale_256, scale_512, scale_640, scale_1000, scale_1500, scale_2000, scale_3000, scale_5000, scale_7000, scale_10000, scale_11000, scale_32000, scale_54000, scale_100000, scale_1000000, scale_custom;
        private ToolStripMenuItem avg_checked, clip_watch, show_checked, topmost_checked, autoscale_checked, graph_label_checked, mailcheck, mailchecknow;
        /* added by miechu */
        private ToolStripMenuItem simple_icon_checked;
        /* end of added by miechu */
        private ToolStripMenuItem units_kbits, units_kbytes, graphs_download, graphs_upload, graphs_summary, colorcycle, m_update;
        private ToolStripMenuItem font_large, font_medium, font_small, hue_slider;
        private ToolStripMenuItem menuItem6;
        private ToolStripMenuItem menuItem7;
        private ToolStripMenuItem menuItem8;
        private ToolStripMenuItem menuItem10;
        private ToolStripMenuItem menuItem11;
        private ToolStripMenuItem menuItem14;
        private ToolStripMenuItem menuItem16;
        private ToolStripMenuItem menuItem17;
        private ToolStripMenuItem menuItem18;
        private ToolStripMenuItem menuItem20;
        private ToolStripMenuItem menuItem21;
        private System.ComponentModel.IContainer components;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator menuItem5;
        private ToolStripSeparator menuItem9;
        private ToolStripSeparator menuItem12;
        private ToolStripSeparator menuItem13;
        private ToolStripSeparator menuItem1;
        private ToolStripSeparator menuItem15;
        private ToolStripSeparator menuItem19;
        private NotifyIcon m_notifyicon;
        private Timer ClipTimer;
        protected internal Timer MailTimer;
        private Timer ShrinkTimer;
        private Timer LogTimer;
    }
}


