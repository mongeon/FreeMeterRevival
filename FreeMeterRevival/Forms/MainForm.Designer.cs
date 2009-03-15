using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using FreeMeterRevival.Controls;

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
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exit_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.msMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMenuGraph = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_Opacity = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.avg_checked = new System.Windows.Forms.ToolStripMenuItem();
            this.m_scale_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.autoscale_checked = new System.Windows.Forms.ToolStripMenuItem();
            this.scale_custom = new System.Windows.Forms.ToolStripMenuItem();
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
            this.m_interval_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.interval_tenth = new System.Windows.Forms.ToolStripMenuItem();
            this.interval_fifth = new System.Windows.Forms.ToolStripMenuItem();
            this.interval_half = new System.Windows.Forms.ToolStripMenuItem();
            this.interval_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.m_graphs = new System.Windows.Forms.ToolStripMenuItem();
            this.graph_label_checked = new System.Windows.Forms.ToolStripMenuItem();
            this.graphs_summary = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.graphs_download = new System.Windows.Forms.ToolStripMenuItem();
            this.graphs_upload = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.font_large = new System.Windows.Forms.ToolStripMenuItem();
            this.font_medium = new System.Windows.Forms.ToolStripMenuItem();
            this.font_small = new System.Windows.Forms.ToolStripMenuItem();
            this.m_units = new System.Windows.Forms.ToolStripMenuItem();
            this.units_kbits = new System.Windows.Forms.ToolStripMenuItem();
            this.units_kbytes = new System.Windows.Forms.ToolStripMenuItem();
            this.m_interfaces = new System.Windows.Forms.ToolStripMenuItem();
            this.msMainGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.m_notifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ClipTimer = new System.Windows.Forms.Timer(this.components);
            this.MailTimer = new System.Windows.Forms.Timer(this.components);
            this.ShrinkTimer = new System.Windows.Forms.Timer(this.components);
            this.LogTimer = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.sbMain = new System.Windows.Forms.StatusStrip();
            this.sbMainStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbMainDownload = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbMainUpload = new System.Windows.Forms.ToolStripStatusLabel();
            this.msMain = new System.Windows.Forms.MenuStrip();
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
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.msMainAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem21 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.msMainAboutAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPanel1 = new System.Windows.Forms.ToolStripPanel();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsMainGraph = new System.Windows.Forms.ToolStrip();
            this.FullMeter = new FreeMeterRevival.Controls.NetworkGraph(this.components);
            this.m_menu.SuspendLayout();
            this.cmMenuGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.sbMain.SuspendLayout();
            this.msMain.SuspendLayout();
            this.toolStripPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FullMeter)).BeginInit();
            this.SuspendLayout();
            // 
            // m_menu
            // 
            this.m_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.show_checked,
            this.topmost_checked,
            this.simple_icon_checked,
            this.toolStripMenuItem1,
            this.exit_menu});
            this.m_menu.Name = "m_menu";
            this.m_menu.OwnerItem = this.msMainFile;
            this.m_menu.Size = new System.Drawing.Size(185, 98);
            // 
            // show_checked
            // 
            this.show_checked.Checked = true;
            this.show_checked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.show_checked.Name = "show_checked";
            this.show_checked.Size = new System.Drawing.Size(184, 22);
            this.show_checked.Text = "Show &Desktop Meter";
            this.show_checked.Click += new System.EventHandler(this.Show_Click);
            // 
            // topmost_checked
            // 
            this.topmost_checked.Name = "topmost_checked";
            this.topmost_checked.Size = new System.Drawing.Size(184, 22);
            this.topmost_checked.Text = "&Always on Top";
            this.topmost_checked.Click += new System.EventHandler(this.TopMost_Click);
            // 
            // simple_icon_checked
            // 
            this.simple_icon_checked.Name = "simple_icon_checked";
            this.simple_icon_checked.Size = new System.Drawing.Size(184, 22);
            this.simple_icon_checked.Text = "&Simple Notify Icon";
            this.simple_icon_checked.Click += new System.EventHandler(this.SimpleNotifyIcon_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 6);
            // 
            // exit_menu
            // 
            this.exit_menu.Name = "exit_menu";
            this.exit_menu.Size = new System.Drawing.Size(184, 22);
            this.exit_menu.Text = "&Exit";
            this.exit_menu.Click += new System.EventHandler(this.Exit_Click);
            // 
            // msMainFile
            // 
            this.msMainFile.DropDown = this.m_menu;
            this.msMainFile.Name = "msMainFile";
            this.msMainFile.Size = new System.Drawing.Size(35, 20);
            this.msMainFile.Text = "&File";
            // 
            // cmMenuGraph
            // 
            this.cmMenuGraph.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_Opacity,
            this.toolStripMenuItem2,
            this.avg_checked,
            this.m_scale_menu,
            this.m_interval_menu,
            this.m_graphs,
            this.m_units,
            this.m_interfaces});
            this.cmMenuGraph.Name = "cmMenuGraph";
            this.cmMenuGraph.OwnerItem = this.msMainGraph;
            this.cmMenuGraph.ShowCheckMargin = true;
            this.cmMenuGraph.ShowImageMargin = false;
            this.cmMenuGraph.Size = new System.Drawing.Size(169, 164);
            // 
            // m_Opacity
            // 
            this.m_Opacity.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem10,
            this.menuItem11});
            this.m_Opacity.Name = "m_Opacity";
            this.m_Opacity.Size = new System.Drawing.Size(168, 22);
            this.m_Opacity.Text = "Opacity";
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
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(165, 6);
            // 
            // avg_checked
            // 
            this.avg_checked.CheckOnClick = true;
            this.avg_checked.Name = "avg_checked";
            this.avg_checked.Size = new System.Drawing.Size(168, 22);
            this.avg_checked.Text = "Display Averages";
            this.avg_checked.Click += new System.EventHandler(this.Avg_Click);
            // 
            // m_scale_menu
            // 
            this.m_scale_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoscale_checked,
            this.scale_custom,
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
            this.scale_1000000});
            this.m_scale_menu.Name = "m_scale_menu";
            this.m_scale_menu.Size = new System.Drawing.Size(168, 22);
            this.m_scale_menu.Text = "Graph Scale";
            // 
            // autoscale_checked
            // 
            this.autoscale_checked.Name = "autoscale_checked";
            this.autoscale_checked.Size = new System.Drawing.Size(121, 22);
            this.autoscale_checked.Text = "Auto";
            this.autoscale_checked.Click += new System.EventHandler(this.SetAutoScale);
            // 
            // scale_custom
            // 
            this.scale_custom.Name = "scale_custom";
            this.scale_custom.Size = new System.Drawing.Size(121, 22);
            this.scale_custom.Text = "Custom";
            this.scale_custom.Click += new System.EventHandler(this.SetScale_MenuClick);
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
            // m_interval_menu
            // 
            this.m_interval_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.interval_tenth,
            this.interval_fifth,
            this.interval_half,
            this.interval_1});
            this.m_interval_menu.Name = "m_interval_menu";
            this.m_interval_menu.Size = new System.Drawing.Size(168, 22);
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
            // m_graphs
            // 
            this.m_graphs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graph_label_checked,
            this.graphs_summary,
            this.menuItem12,
            this.graphs_download,
            this.graphs_upload,
            this.menuItem13,
            this.menuItem7,
            this.font_large,
            this.font_medium,
            this.font_small});
            this.m_graphs.Name = "m_graphs";
            this.m_graphs.Size = new System.Drawing.Size(168, 22);
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
            // menuItem7
            // 
            this.menuItem7.Name = "menuItem7";
            this.menuItem7.Size = new System.Drawing.Size(300, 22);
            this.menuItem7.Text = "Text Color";
            this.menuItem7.Click += new System.EventHandler(this.TextColor_Click);
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
            this.m_units.Size = new System.Drawing.Size(168, 22);
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
            this.units_kbytes.Click += new System.EventHandler(this.SetUnits_kbytes);
            // 
            // m_interfaces
            // 
            this.m_interfaces.Name = "m_interfaces";
            this.m_interfaces.Size = new System.Drawing.Size(168, 22);
            this.m_interfaces.Text = "Interfaces";
            // 
            // msMainGraph
            // 
            this.msMainGraph.DropDown = this.cmMenuGraph;
            this.msMainGraph.Name = "msMainGraph";
            this.msMainGraph.Size = new System.Drawing.Size(48, 20);
            this.msMainGraph.Text = "Graph";
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
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
            this.m_notifyicon.Text = "FreeMeter Revival";
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
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // sbMain
            // 
            this.sbMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbMainStatus,
            this.sbMainDownload,
            this.sbMainUpload});
            this.sbMain.Location = new System.Drawing.Point(0, 432);
            this.sbMain.Name = "sbMain";
            this.sbMain.Size = new System.Drawing.Size(632, 22);
            this.sbMain.TabIndex = 4;
            // 
            // sbMainStatus
            // 
            this.sbMainStatus.Name = "sbMainStatus";
            this.sbMainStatus.Size = new System.Drawing.Size(467, 17);
            this.sbMainStatus.Spring = true;
            // 
            // sbMainDownload
            // 
            this.sbMainDownload.AutoSize = false;
            this.sbMainDownload.Image = global::FreeMeterRevival.Properties.Resources.arrow_down;
            this.sbMainDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sbMainDownload.Name = "sbMainDownload";
            this.sbMainDownload.Size = new System.Drawing.Size(75, 17);
            this.sbMainDownload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sbMainUpload
            // 
            this.sbMainUpload.AutoSize = false;
            this.sbMainUpload.Image = global::FreeMeterRevival.Properties.Resources.arrow_up;
            this.sbMainUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sbMainUpload.Name = "sbMainUpload";
            this.sbMainUpload.Size = new System.Drawing.Size(75, 17);
            this.sbMainUpload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msMainFile,
            this.msMainGraph,
            this.m_utils,
            this.msMainAbout});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(632, 24);
            this.msMain.TabIndex = 5;
            this.msMain.Text = "menuStrip1";
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
            this.toolStripMenuItem4,
            this.optionsToolStripMenuItem});
            this.m_utils.Name = "m_utils";
            this.m_utils.Size = new System.Drawing.Size(44, 20);
            this.m_utils.Text = "Tools";
            // 
            // clip_watch
            // 
            this.clip_watch.Name = "clip_watch";
            this.clip_watch.Size = new System.Drawing.Size(187, 22);
            this.clip_watch.Text = "URL Grabber Enabled";
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
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(184, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Enabled = false;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            // 
            // msMainAbout
            // 
            this.msMainAbout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem21,
            this.toolStripMenuItem3,
            this.msMainAboutAbout});
            this.msMainAbout.Name = "msMainAbout";
            this.msMainAbout.Size = new System.Drawing.Size(24, 20);
            this.msMainAbout.Text = "&?";
            // 
            // menuItem21
            // 
            this.menuItem21.Name = "menuItem21";
            this.menuItem21.Size = new System.Drawing.Size(176, 22);
            this.menuItem21.Text = "Check For Updates";
            this.menuItem21.Click += new System.EventHandler(this.Check_Version);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(173, 6);
            // 
            // msMainAboutAbout
            // 
            this.msMainAboutAbout.Name = "msMainAboutAbout";
            this.msMainAboutAbout.Size = new System.Drawing.Size(176, 22);
            this.msMainAboutAbout.Text = "&About...";
            this.msMainAboutAbout.Click += new System.EventHandler(this.About_Click);
            // 
            // toolStripPanel1
            // 
            this.toolStripPanel1.Controls.Add(this.tsMain);
            this.toolStripPanel1.Controls.Add(this.tsMainGraph);
            this.toolStripPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStripPanel1.Location = new System.Drawing.Point(0, 24);
            this.toolStripPanel1.Name = "toolStripPanel1";
            this.toolStripPanel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.toolStripPanel1.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStripPanel1.Size = new System.Drawing.Size(632, 25);
            this.toolStripPanel1.Visible = false;
            // 
            // tsMain
            // 
            this.tsMain.Dock = System.Windows.Forms.DockStyle.None;
            this.tsMain.Location = new System.Drawing.Point(3, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(111, 25);
            this.tsMain.TabIndex = 7;
            this.tsMain.Text = "toolStrip1";
            // 
            // tsMainGraph
            // 
            this.tsMainGraph.Dock = System.Windows.Forms.DockStyle.None;
            this.tsMainGraph.Location = new System.Drawing.Point(126, 0);
            this.tsMainGraph.Name = "tsMainGraph";
            this.tsMainGraph.Size = new System.Drawing.Size(111, 25);
            this.tsMainGraph.TabIndex = 8;
            // 
            // FullMeter
            // 
            this.FullMeter.ContextMenuStrip = this.cmMenuGraph;
            this.FullMeter.DataBindings.Add(new System.Windows.Forms.Binding("DownloadColor", global::FreeMeterRevival.Properties.Settings.Default, "DownloadColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FullMeter.DataBindings.Add(new System.Windows.Forms.Binding("OverlapColor", global::FreeMeterRevival.Properties.Settings.Default, "OverlapColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FullMeter.DataBindings.Add(new System.Windows.Forms.Binding("UploadColor", global::FreeMeterRevival.Properties.Settings.Default, "UploadColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FullMeter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FullMeter.DownloadColor = global::FreeMeterRevival.Properties.Settings.Default.DownloadColor;
            this.FullMeter.Location = new System.Drawing.Point(0, 24);
            this.FullMeter.Name = "FullMeter";
            this.FullMeter.OverlapColor = global::FreeMeterRevival.Properties.Settings.Default.OverlapColor;
            this.FullMeter.ShowSummary = false;
            this.FullMeter.Size = new System.Drawing.Size(632, 408);
            this.FullMeter.TabIndex = 0;
            this.FullMeter.TabStop = false;
            this.FullMeter.UploadColor = global::FreeMeterRevival.Properties.Settings.Default.UploadColor;
            this.FullMeter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.FullMeter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(632, 454);
            this.Controls.Add(this.toolStripPanel1);
            this.Controls.Add(this.FullMeter);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.msMain);
            this.Controls.Add(this.trackBar2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMain;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 480);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(320, 240);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FreeMeter Revival";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.m_menu.ResumeLayout(false);
            this.cmMenuGraph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.sbMain.ResumeLayout(false);
            this.sbMain.PerformLayout();
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.toolStripPanel1.ResumeLayout(false);
            this.toolStripPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FullMeter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private NetworkGraph FullMeter;
        private ContextMenuStrip m_menu;
        private TrackBar trackBar2 ;
        private ColorDialog colorDialog1;
        // Menus and menu click handlers
        private ToolStripMenuItem exit_menu;
        private ToolStripMenuItem show_checked, topmost_checked;
        /* added by miechu */
        private ToolStripMenuItem simple_icon_checked;
        /* end of added by miechu */
        private ToolStripMenuItem m_update;
        private System.ComponentModel.IContainer components;
        private ToolStripSeparator toolStripMenuItem1;
        private NotifyIcon m_notifyicon;
        private Timer ClipTimer;
        protected internal Timer MailTimer;
        private Timer ShrinkTimer;
        private Timer LogTimer;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private StatusStrip sbMain;
        private ToolStripStatusLabel sbMainStatus;
        private ToolStripStatusLabel sbMainDownload;
        private ToolStripStatusLabel sbMainUpload;
        private MenuStrip msMain;
        private ToolStripMenuItem msMainFile;
        private ToolStripPanel toolStripPanel1;
        private ToolStrip tsMain;
        private ToolStripMenuItem msMainGraph;
        private ToolStrip tsMainGraph;
        private ToolStripMenuItem msMainAbout;
        private ToolStripMenuItem msMainAboutAbout;
        private ToolStripMenuItem m_scale_menu;
        private ToolStripMenuItem autoscale_checked;
        private ToolStripMenuItem scale_33;
        private ToolStripMenuItem scale_56;
        private ToolStripMenuItem scale_64;
        private ToolStripMenuItem scale_128;
        private ToolStripMenuItem scale_256;
        private ToolStripMenuItem scale_512;
        private ToolStripMenuItem scale_640;
        private ToolStripMenuItem scale_1000;
        private ToolStripMenuItem scale_1500;
        private ToolStripMenuItem scale_2000;
        private ToolStripMenuItem scale_3000;
        private ToolStripMenuItem scale_5000;
        private ToolStripMenuItem scale_7000;
        private ToolStripMenuItem scale_10000;
        private ToolStripMenuItem scale_11000;
        private ToolStripMenuItem scale_32000;
        private ToolStripMenuItem scale_54000;
        private ToolStripMenuItem scale_100000;
        private ToolStripMenuItem scale_1000000;
        private ToolStripMenuItem scale_custom;
        private ToolStripMenuItem avg_checked;
        private ToolStripMenuItem m_interval_menu;
        private ToolStripMenuItem interval_tenth;
        private ToolStripMenuItem interval_fifth;
        private ToolStripMenuItem interval_half;
        private ToolStripMenuItem interval_1;
        private ToolStripMenuItem m_graphs;
        private ToolStripMenuItem graph_label_checked;
        private ToolStripMenuItem graphs_summary;
        private ToolStripSeparator menuItem12;
        private ToolStripMenuItem graphs_download;
        private ToolStripMenuItem graphs_upload;
        private ToolStripSeparator menuItem13;
        private ToolStripMenuItem menuItem7;
        private ToolStripMenuItem font_large;
        private ToolStripMenuItem font_medium;
        private ToolStripMenuItem font_small;
        private ToolStripMenuItem m_units;
        private ToolStripMenuItem units_kbits;
        private ToolStripMenuItem units_kbytes;
        private ToolStripMenuItem m_interfaces;
        private ToolStripMenuItem m_Opacity;
        private ToolStripMenuItem menuItem10;
        private ToolStripMenuItem menuItem11;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem m_utils;
        private ToolStripMenuItem clip_watch;
        private ToolStripSeparator menuItem1;
        private ToolStripMenuItem mailcheck;
        private ToolStripMenuItem mailchecknow;
        private ToolStripMenuItem menuItem14;
        private ToolStripSeparator menuItem15;
        private ToolStripMenuItem menuItem16;
        private ToolStripMenuItem menuItem17;
        private ToolStripMenuItem menuItem18;
        private ToolStripSeparator menuItem19;
        private ToolStripMenuItem menuItem20;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem menuItem21;
        private ToolStripSeparator toolStripMenuItem3;
        private ContextMenuStrip cmMenuGraph;
    }
}


