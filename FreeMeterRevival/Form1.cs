/*
Copyright © 2005-2007, David Schultz, Mieszko Lassota
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

- Redistributions of source code must retain the above copyright
notice, this list of conditions and the following disclaimer.

- Neither the name of the owner, nor the names of its
contributors may be used to endorse or promote products
derived from this software without specific prior written
permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using NatTraversal.Interop;
using System.Drawing.Drawing2D;

namespace FreeMeter
{

	public class Form1 : System.Windows.Forms.Form
	{

		public string ClipData = "";
		public ArrayList MailServers = new ArrayList();
		public static NotifyIcon m_notifyicon = new NotifyIcon();
		public System.Windows.Forms.Timer MailTimer = new System.Windows.Forms.Timer();

		#region <Name>

		public bool will_reboot = false;

		#endregion

		private System.Windows.Forms.Timer ClipTimer = new System.Windows.Forms.Timer();
		private Assembly myAssembly = Assembly.GetExecutingAssembly();
		private NetworkMonitor monitor = new NetworkMonitor();
		private Thread backgroundWorker1;

		private int timerInterval = 1000;
		private int WLength, WHeight, scale;
		private bool m_closing = false;
		private string display_xscale, display_yscale;
		private int[] downlines = new int[16];
		private int[] uplines = new int[16];
		private int[] full_downlines, full_uplines;
		private double[] full_downspeeds, full_upspeeds;
		public double downspeed = 0.0; //modified to from private -> public by miechu
		public double upspeed = 0.0; //modified to from private -> public by miechu
		private bool respond_to_latest = false;

		private Label label1 = new Label();
		private Label label2 = new Label();
		private Label label3 = new Label();
		private Label label4 = new Label();
		private PictureBox pictureBox1 = new PictureBox();
		private PictureBox resizer = new PictureBox();
		private ContextMenu m_menu = new ContextMenu();
		private TrackBar trackBar1 = new TrackBar();
		private TrackBar trackBar2 = new TrackBar();

		private ColorDialog colorDialog1 = new ColorDialog();
		private Color DOWNLOAD_COLOR = Color.FromArgb(255, 0, 255, 0);
		private Color UPLOAD_COLOR = Color.FromArgb(255, 255, 0, 0);
		private Color OVERLAP_COLOR = Color.FromArgb(255, 255, 255, 0);
		private Color FORGROUND_COLOR = Color.White;
		private Color BACKGROUND_COLOR;
		private Color HIGHLIGHT_COLOR;
		private Color SHADOW_COLOR;
		private Pen downloadPen = new Pen(Color.FromArgb(255, 0, 255, 0), 1);
		private Pen uploadPen = new Pen(Color.FromArgb(255, 255, 0, 0), 1);
		private Pen overlapPen = new Pen(Color.FromArgb(255, 255, 255, 0), 1);

		//Cool icon representation
		private bool icon_representation = false;
		private bool icons_loaded = false;
		private Image upload_icon_green;
		private Image upload_icon_red;
		private Image download_icon_green;
		private Image download_icon_red;

		//##TotalsLog##
		Totals_Log logs_form;
		public System.Windows.Forms.Timer LogTimer = new System.Windows.Forms.Timer();
		public bool LogEnabled;
		public int LogInterval;

		//for counting time in ms
		[DllImport("Kernel32.dll", EntryPoint = "QueryPerformanceCounter")]
		static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
		[DllImport("Kernel32.dll", EntryPoint = "QueryPerformanceFrequency")]
		static extern bool QueryPerformanceFrequency(out long lpFrequency);
		//For destroying the leaky GDI Object handles
		[DllImport("user32.dll", EntryPoint = "DestroyIcon")]
		static extern bool DestroyIcon(IntPtr oIcon);
		[DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
		static extern bool DeleteObject(IntPtr oBm);

		public Form1()
		{
			if (monitor.Adapters.Length == 0)
			{
				MessageBox.Show("I can't find any network adapters on this computer.", "FreeMeter Failed.");
				return;
			}

			logs_form = new Totals_Log();
			MailTimer.Tick += new EventHandler(MailTimer_Tick);

			ClipTimer.Interval = 1000;
			ClipTimer.Tick += new EventHandler(ClipTimer_Tick);

			Visible = false;
			StartPosition = FormStartPosition.Manual;
			Name = "Form1";
			FormBorderStyle = FormBorderStyle.None;
			MaximizeBox = false;
			MinimizeBox = false;
			ControlBox = false;
			ShowInTaskbar = false;
			BackColor = BACKGROUND_COLOR;
			ForeColor = FORGROUND_COLOR;
			HIGHLIGHT_COLOR = HSL_to_RGB(BACKGROUND_COLOR.GetHue(), BACKGROUND_COLOR.GetSaturation(), BACKGROUND_COLOR.GetBrightness() + .3);
			SHADOW_COLOR = HSL_to_RGB(BACKGROUND_COLOR.GetHue(), BACKGROUND_COLOR.GetSaturation(), BACKGROUND_COLOR.GetBrightness() - .1);
			AutoScaleMode = AutoScaleMode.None;
			MinimumSize = new Size(66, 45);
			MaximumSize = new Size(606, 455);
			Resize += new EventHandler(Form1_Resize);
			DoubleBuffered = true;

			MakeMenus();
			RestoreRegistry();

			try
			{
				SetDefaults();
				LoadConfiguration();
			}
			catch
			{}

			Check_Menus();
			Form1_Borders();

			//control for resizing 
			resizer.Location = new Point(ClientSize.Width - 13, ClientSize.Height - 13);
			resizer.Size = new Size(11, 11);
			resizer.Name = "resizer";
			Make_Grabhandle();
			Controls.Add(this.resizer);
			resizer.MouseMove += new MouseEventHandler(Resize_MouseMove);
			resizer.MouseDown += new MouseEventHandler(Resize_MouseDown);
			resizer.MouseUp += new MouseEventHandler(Resize_MouseUp);
			resizer.Cursor = Cursors.SizeNWSE;

			//SysTray Icon which is animated to show smaller graph
			m_notifyicon.ContextMenu = m_menu;
			Stream s = myAssembly.GetManifestResourceStream("FreeMeter.FreeMeter.ico");
			Icon = new Icon(s);
			m_notifyicon.Icon = this.Icon;
			s.Close();
			m_notifyicon.Visible = true;
			m_notifyicon.MouseDown += new MouseEventHandler(Icon_MouseDown);

			//full graph
			pictureBox1.Location = new Point(3, 3);
			pictureBox1.Size = new Size(ClientSize.Width - 6, ClientSize.Height - 18);
			pictureBox1.Name = "FullMeter";
			pictureBox1.BackColor = Color.Black;
			Controls.Add(this.pictureBox1);
			pictureBox1.ContextMenu = m_menu;
			pictureBox1.MouseDown += new MouseEventHandler(Main_MouseDown);
			pictureBox1.MouseMove += new MouseEventHandler(Form1_MouseMove);

			//download text display
			label1.Location = new Point(10, WHeight - 14);
			label1.Size = new Size(WLength / 2 - 10 - 5, 13);
			label1.BackColor = BACKGROUND_COLOR;
			label1.ForeColor = FORGROUND_COLOR;
			label1.Font = new Font("MS Serif", 7);
			label1.TextAlign = ContentAlignment.TopLeft;
			Controls.Add(this.label1);
			label1.ContextMenu = m_menu;
			label1.MouseDown += new MouseEventHandler(Main_MouseDown);

			//upload text display
			label2.Location = new Point(WLength / 2 + 9 - 5, WHeight - 14);
			label2.Size = new Size(WLength / 2 - 9 - 13 + 5, 13);
			label2.BackColor = BACKGROUND_COLOR;
			label2.ForeColor = FORGROUND_COLOR;
			label2.Font = new Font("MS Serif", 7);
			label2.TextAlign = ContentAlignment.TopLeft;
			Controls.Add(this.label2);
			label2.ContextMenu = m_menu;
			label2.MouseDown += new MouseEventHandler(Main_MouseDown);

			//down arrow
			label3.Location = new Point(1, WHeight - 14);
			label3.Size = new Size(9, 13);
			label3.BackColor = BACKGROUND_COLOR;
			label3.ForeColor = DOWNLOAD_COLOR;
			label3.Font = new Font("Wingdings", 7);
			label3.TextAlign = ContentAlignment.BottomLeft;
			label3.Text = "ê";
			Controls.Add(this.label3);
			label3.ContextMenu = m_menu;
			label3.MouseDown += new MouseEventHandler(Main_MouseDown);

			//up arrow
			label4.Location = new Point(WLength / 2 - 5, WHeight - 14);
			label4.Size = new Size(9, 13);
			label4.BackColor = BACKGROUND_COLOR;
			label4.ForeColor = UPLOAD_COLOR;
			label4.Font = new Font("Wingdings", 7);
			label4.TextAlign = ContentAlignment.TopLeft;
			label4.Text = "é";
			Controls.Add(this.label4);
			label4.ContextMenu = m_menu;
			label4.MouseDown += new MouseEventHandler(Main_MouseDown);

			// Color Dialog
			colorDialog1.FullOpen = true;
			colorDialog1.AnyColor = true;
			colorDialog1.AllowFullOpen = true;

			// Hue Trackbar
			trackBar1.AutoSize = false;
			trackBar1.Location = new Point(2, WHeight - 14);
			trackBar1.Margin = new Padding(0);
			trackBar1.Size = new Size(WLength - 15, 15);
			trackBar1.Maximum = 360;
			trackBar1.Name = "trackBar1";
			trackBar1.TickFrequency = 45;
			trackBar1.ValueChanged += new EventHandler(Trackbar1_Update);
			trackBar1.MouseUp += new MouseEventHandler(Trackbar1_Hide);
			Controls.Add(this.trackBar1);
			trackBar1.SendToBack();
			trackBar1.Hide();
			trackBar1.Value = (int)BACKGROUND_COLOR.GetHue();

			// Transparency Trackbar
			trackBar2.AutoSize = false;
			trackBar2.Location = new Point(2, WHeight - 14);
			trackBar2.Margin = new Padding(0);
			trackBar2.Size = new Size(WLength - 15, 15);
			trackBar2.LargeChange = 10;
			trackBar2.SmallChange = 1;
			trackBar2.Name = "trackBar2";
			trackBar2.TickFrequency = 10;
			trackBar2.ValueChanged += new EventHandler(Trackbar2_Update);
			trackBar2.MouseUp += new MouseEventHandler(Trackbar2_Hide);
			Controls.Add(this.trackBar2);
			trackBar2.SendToBack();
			trackBar2.Hide();

			backgroundWorker1 = new Thread(new ThreadStart(backgroundWorker1_DoWork));
			backgroundWorker1.IsBackground = false;
			backgroundWorker1.Priority = ThreadPriority.AboveNormal;
			backgroundWorker1.Start();

			Check_Version(this, new EventArgs());

			//hack to initially try to reduce the memory footprint of the app (admin only)
			try
			{
				Process loProcess = Process.GetCurrentProcess();
				loProcess.MaxWorkingSet = loProcess.MaxWorkingSet;
				loProcess.Dispose();
			}
			catch { }
			System.Windows.Forms.Timer ShrinkTimer = new System.Windows.Forms.Timer();
			ShrinkTimer.Interval = 60000;
			ShrinkTimer.Tick += new EventHandler(ShrinkTimer_Tick);
			ShrinkTimer.Start();

			if (File.Exists("upload.bmp") && File.Exists("upload2.bmp") && File.Exists("download.bmp") && File.Exists("download2.bmp"))
			{
				upload_icon_red = Bitmap.FromFile("upload.bmp");
				upload_icon_green = Bitmap.FromFile("upload2.bmp");
				download_icon_red = Bitmap.FromFile("download.bmp");
				download_icon_green = Bitmap.FromFile("download2.bmp");

				((Bitmap)upload_icon_red).MakeTransparent(Color.FromArgb(255, 0, 255));
				((Bitmap)upload_icon_green).MakeTransparent(Color.FromArgb(255, 0, 255));
				((Bitmap)download_icon_red).MakeTransparent(Color.FromArgb(255, 0, 255));
				((Bitmap)download_icon_green).MakeTransparent(Color.FromArgb(255, 0, 255));

				icons_loaded = true;
			}
		}

		void UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			(new Error(e.ExceptionObject as Exception)).ShowDialog();
		}

		//the timer stuff
		private void backgroundWorker1_DoWork()
		{
			long freq = 0;
			if (QueryPerformanceFrequency(out freq))
			{
				if (freq != 0)
				{
					if (freq == 1000)
						MessageBox.Show("Uses GetTickCount", "?");
					long count1 = 0;
					long count2 = 0;
					while (!m_closing)
					{
						QueryPerformanceCounter(out count1);
						ElapsedTimer();
						QueryPerformanceCounter(out count2);
						long time_ms = (count2 - count1) * 1000 / freq;
						if (time_ms > (long)timerInterval)
							time_ms = (long)timerInterval;
						Thread.Sleep((int)((long)timerInterval - time_ms));
					}
				}
				else
					MessageBox.Show("I can't find QueryPerformanceFrequency()", "FreeMeter Failed.");
			}
			else
				MessageBox.Show("I failed to use QueryPerformanceFrequency()", "FreeMeter Failed.");
		}

		private void ElapsedTimer()
		{
			foreach (NetworkAdapter adapter in monitor.Adapters)
			{
				if (adapter.Enabled)
					adapter.refresh();
			}

			RefreshSpeeds();
			DrawIconRepresentation();
			DrawFullMeter();

			if (colorcycle.Checked && this.Visible)
			{
				double h = 0, sa = 0, l = 0;
				RGB_to_HSL(BACKGROUND_COLOR, ref h, ref sa, ref l);
				h = h + 1;
				if (h > 360.0) h = 0;
				BACKGROUND_COLOR = label1.BackColor = label2.BackColor = label3.BackColor = label4.BackColor = HSL_to_RGB(h, sa, l);
				SetColor(this, BACKGROUND_COLOR);
				HIGHLIGHT_COLOR = HSL_to_RGB(h, sa, l + .3);
				SHADOW_COLOR = HSL_to_RGB(h, sa, l - .1);
				Make_Grabhandle();
				Form1_Borders();
			}
		}
		private bool checkingmail = false;
		private void MailTimer_Tick(Object sender, EventArgs e)
		{
			if (mailcheck.Checked || sender.Equals(mailchecknow))
			{
				if (sender.Equals(mailchecknow))
					checkingmail = true;
				BackgroundWorker checkmailWorker = new BackgroundWorker();
				checkmailWorker.WorkerReportsProgress = false;
				checkmailWorker.DoWork += new DoWorkEventHandler(CheckMailWorker_DoWork);
				checkmailWorker.RunWorkerAsync();
				checkmailWorker.Dispose();
			}
		}
		private void CheckMailWorker_DoWork(Object sender, DoWorkEventArgs e)
		{
			StringBuilder balloon_text = new StringBuilder();
			bool ErrorOccured = false, NewMailOccurred = false;
			ToolTipIcon TTI = ToolTipIcon.Info;
			foreach (MailServer server in MailServers)
			{
				if (server.Enabled)
				{
					int newmsgcount = 0;
					string errmsg = null;
					if (server.Type == "1")
					{ //imap
						IMAP pop = new IMAP(server.Host, server.User, server.Pass);
						newmsgcount = pop.GetNumberOfMessages();
						errmsg = pop.ErrMsg;
					}
					else if (server.Type == "0")
					{ //pop
						POP3 pop = new POP3(server.Host, server.User, server.Pass);
						newmsgcount = pop.GetNumberOfMessages();
						errmsg = pop.ErrMsg;
					}

					if (newmsgcount == -1)
					{ //error
						balloon_text.Append(errmsg);
						balloon_text.Append(" (");
						balloon_text.Append(server.Host);
						balloon_text.Append(")\n");
						ErrorOccured = true;
						//server.Enabled = false;
					}
					else if (newmsgcount == 0)
					{
						server.OldMsgCount = 0;
						if (checkingmail)
						{
							balloon_text.Append("No New Messages on ");
							balloon_text.Append(server.Host);
							balloon_text.Append("\n");
						}
					}
					else if (newmsgcount == server.OldMsgCount && checkingmail)
					{
						balloon_text.Append("No New Messages on ");
						balloon_text.Append(server.Host);
						balloon_text.Append("\n");
					}
					else if (newmsgcount > server.OldMsgCount)
					{
						balloon_text.Append(newmsgcount - server.OldMsgCount);
						if (newmsgcount - server.OldMsgCount == 1)
							balloon_text.Append(" new message on ");
						else
							balloon_text.Append(" new messages on ");
						balloon_text.Append(server.Host);
						balloon_text.Append("\n");
						server.OldMsgCount = newmsgcount;
						NewMailOccurred = true;
					}
				}
			}
			if (balloon_text.Length > 0)
			{
				string balloon_title = "";
				if (ErrorOccured)
				{
					balloon_title = "There were errors checking your email.";
					TTI = ToolTipIcon.Error;
				}
				else if (NewMailOccurred)
					balloon_title = "You have new email!";
				else
					balloon_title = "No New Messages.";
				m_notifyicon.ShowBalloonTip(1, balloon_title, balloon_text.ToString(), TTI);
			}
			checkingmail = false;
		}
		private void ClipTimer_Tick(object sender, EventArgs e)
		{
			if (clip_watch.Checked)
			{
				IDataObject o = Clipboard.GetDataObject();
				if (o != null)
				{
					if (o.GetDataPresent(DataFormats.Text, false))
					{
						string s = (string)o.GetData(DataFormats.Text);
						if (s != null && s != String.Empty)
						{
							s = s.Trim();
							s = s.Replace("\r", "").Replace("\n", ""); //newlines
							s = s.Replace(": ", "").Replace("³ ", "").Replace("| ", "").Replace(" ", ""); //weird chars that i find in my URLs copied from my different appplications like email and IRC
							s = s.Replace(">", ""); //commonly in email RE:'s
							if (ClipData == "")
								ClipData = s;
							if (s != ClipData)
							{
								ClipData = s;
								if (s.StartsWith("http://") || s.StartsWith("https://") || s.StartsWith("ftp://") || s.StartsWith("www."))
									Process.Start(s);
							}
						}
						else
							ClipData = null;
					}
					else
						ClipData = null;
				}
				else
					ClipData = null;
			}
		}
		private void ShrinkTimer_Tick(object sender, EventArgs e)
		{
			//hack to initially try to reduce the memory footprint of the app (admin only)
			try
			{
				Process loProcess = Process.GetCurrentProcess();
				loProcess.MaxWorkingSet = loProcess.MaxWorkingSet;
				loProcess.Dispose();
			}
			catch { }
		}

		//cycle colors and color trackbar
		private void Trackbar1_Update(Object sender, EventArgs e)
		{
			double h = 0, sa = 0, l = 0;
			TrackBar tb = (TrackBar)sender;
			RGB_to_HSL(BACKGROUND_COLOR, ref h, ref sa, ref l);
			h = tb.Value;
			BackColor = BACKGROUND_COLOR = label1.BackColor = label2.BackColor = label3.BackColor = label4.BackColor = HSL_to_RGB(h, sa, l);
			HIGHLIGHT_COLOR = HSL_to_RGB(h, sa, l + .3);
			SHADOW_COLOR = HSL_to_RGB(h, sa, l - .1);
			Make_Grabhandle();
			Form1_Borders();
		}
		private void Trackbar1_Hide(Object sender, MouseEventArgs e)
		{
			trackBar1.SendToBack();
			trackBar1.Enabled = false;
			trackBar1.Hide();
		}
		private void Trackbar1_Show(Object sender, EventArgs e)
		{
			colorcycle.Checked = false;
			trackBar1.BringToFront();
			trackBar1.Enabled = true;
			trackBar1.Show();
		}
		private void Cycle_Colors(Object sender, EventArgs e)
		{
			if (colorcycle.Checked)
				colorcycle.Checked = false;
			else
				colorcycle.Checked = true;
		}
		private void Color_Click(Object sender, EventArgs e)
		{
			double h = 0, sa = 0, l = 0;
			colorcycle.Checked = false;
			colorDialog1.Color = BACKGROUND_COLOR;
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				BackColor = BACKGROUND_COLOR = label1.BackColor = label2.BackColor = label3.BackColor = label4.BackColor = colorDialog1.Color;
				RGB_to_HSL(BACKGROUND_COLOR, ref h, ref sa, ref l);
				HIGHLIGHT_COLOR = HSL_to_RGB(h, sa, l + .3);
				SHADOW_COLOR = HSL_to_RGB(h, sa, l - .1);
				Make_Grabhandle();
				Form1_Borders();
			}
		}
		private void TextColor_Click(Object sender, EventArgs e)
		{
			colorDialog1.Color = FORGROUND_COLOR;
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				ForeColor = FORGROUND_COLOR = label1.ForeColor = label2.ForeColor = colorDialog1.Color;
			}
		}
		private void DefaultColor_Click(Object sender, EventArgs e)
		{
			double h = 0, sa = 0, l = 0;
			colorcycle.Checked = false;
			ForeColor = FORGROUND_COLOR = label1.ForeColor = label2.ForeColor = Color.White;
			BACKGROUND_COLOR = Color.FromArgb(255, 44, 81, 138);
			RGB_to_HSL(BACKGROUND_COLOR, ref h, ref sa, ref l);
			BackColor = BACKGROUND_COLOR = label1.BackColor = label2.BackColor = label3.BackColor = label4.BackColor = HSL_to_RGB(h, sa, l);
			HIGHLIGHT_COLOR = HSL_to_RGB(h, sa, l + .3);
			SHADOW_COLOR = HSL_to_RGB(h, sa, l - .1);
			Make_Grabhandle();
			Form1_Borders();
		}


		//transparency and opacity
		private void Trackbar2_Update(Object sender, EventArgs e)
		{
			TrackBar tb = (TrackBar)sender;
			this.Opacity = ((float)tb.Value) / 100;
		}
		private void Trackbar2_Hide(Object sender, EventArgs e)
		{
			trackBar2.SendToBack();
			trackBar2.Enabled = false;
			trackBar2.Hide();
		}
		private void Trackbar2_Show(Object sender, EventArgs e)
		{
			trackBar2.BringToFront();
			trackBar2.Enabled = true;
			trackBar2.Show();
		}
		private void Opaque_Click(Object sender, EventArgs e)
		{
			this.Opacity = 1.00;
			trackBar2.Value = 100;
		}

		//draw the UI
		private void Form1_Borders()

		{
            if (ClientSize.Width != 0 && ClientSize.Height != 0)
            {
                //do 3D borders
                Bitmap borders = new Bitmap(ClientSize.Width, ClientSize.Height, PixelFormat.Format24bppRgb);
                Graphics formGraphics = Graphics.FromImage((Image)borders);
                formGraphics.Clear(BACKGROUND_COLOR);
                formGraphics.DrawRectangle(new Pen(HIGHLIGHT_COLOR), 2, 2, WLength - 5, WHeight - 17);
                formGraphics.DrawLine(new Pen(HIGHLIGHT_COLOR), 0, 0, WLength - 1, 0);
                formGraphics.DrawLine(new Pen(HIGHLIGHT_COLOR), 0, 0, 0, WHeight - 1);
                formGraphics.DrawLine(new Pen(SHADOW_COLOR), 0, WHeight - 1, WLength - 1, WHeight - 1);
                formGraphics.DrawLine(new Pen(SHADOW_COLOR), WLength - 1, WHeight - 1, WLength - 1, 0);
                formGraphics.DrawRectangle(new Pen(BACKGROUND_COLOR), WLength - 1, 0, 1, 1);
                formGraphics.DrawRectangle(new Pen(BACKGROUND_COLOR), 0, WHeight - 1, 1, 1);
                IntPtr oFB = borders.GetHbitmap();
                this.BackgroundImage = Image.FromHbitmap(oFB);
                DeleteObject(oFB);
                formGraphics.Dispose();
                borders.Dispose();
            }

		}
		private void Make_Grabhandle()
		{
			Bitmap grabhandle = new Bitmap(11, 11, PixelFormat.Format24bppRgb);
			Graphics g = Graphics.FromImage((Image)grabhandle);
			g.FillRectangle(new SolidBrush(BACKGROUND_COLOR), new Rectangle(0, 0, 11, 11));
			Rectangle[] r = new Rectangle[] { new Rectangle(9, 1, 2, 2), new Rectangle(5, 5, 2, 2), new Rectangle(9, 5, 2, 2), new Rectangle(1, 9, 2, 2), new Rectangle(5, 9, 2, 2), new Rectangle(9, 9, 2, 2) };
			g.FillRectangles(new SolidBrush(HIGHLIGHT_COLOR), r);
			r = new Rectangle[] { new Rectangle(8, 0, 2, 2), new Rectangle(4, 4, 2, 2), new Rectangle(8, 4, 2, 2), new Rectangle(0, 8, 2, 2), new Rectangle(4, 8, 2, 2), new Rectangle(8, 8, 2, 2) };
			g.FillRectangles(new SolidBrush(SHADOW_COLOR), r);

			try
			{
				IntPtr oBm1 = grabhandle.GetHbitmap();
				resizer.Image = Image.FromHbitmap(oBm1);
				DeleteObject(oBm1);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			g.Dispose();
			grabhandle.Dispose();
		}

		private void RefreshSpeeds()
		{
			//scroll values in icaon arrays.
			for (int i = 0; i < downlines.Length - 1; i++)
			{
				downlines[i] = downlines[i + 1];
				uplines[i] = uplines[i + 1];
			}
			//scroll values in large arrays.
			for (int i = 0; i < full_downspeeds.Length - 1; i++)
			{
				if (i < full_downspeeds.Length - 1)
				{
					full_downspeeds[i] = full_downspeeds[i + 1];
					full_upspeeds[i] = full_upspeeds[i + 1];
					full_downlines[i] = full_downlines[i + 1];
					full_uplines[i] = full_uplines[i + 1];
				}
			}

			//calculate latest icon values
			downspeed = upspeed = 0.0;
			foreach (NetworkAdapter adapter in monitor.Adapters)
			{
				if (adapter.Enabled)
				{
					downspeed += adapter.DownloadSpeed(timerInterval);
					upspeed += adapter.UploadSpeed(timerInterval);
					
					if (downspeed < 0)
						downspeed = 0;
					
					if (upspeed < 0)
						upspeed = 0;
				}
			}

			logs_form.UpdateData(upspeed / (1024 / timerInterval), downspeed / (1024 / timerInterval));

			if (downspeed < 0 || upspeed < 0)
				MessageBox.Show("something is wrong! downspeed=" + downspeed + " upspeed=" + upspeed);

			downlines[15] = (int)(16 * downspeed / scale);
			uplines[15] = (int)(16 * upspeed / scale);
			if (downlines[15] > 16) downlines[15] = 16;
			if (uplines[15] > 16) uplines[15] = 16;
			//calculate latest large values
			full_downspeeds[full_downspeeds.Length - 1] = downspeed;
			full_upspeeds[full_downspeeds.Length - 1] = upspeed;
			full_downlines[full_downspeeds.Length - 1] = (int)(pictureBox1.Height * downspeed / scale);
			full_uplines[full_downspeeds.Length - 1] = (int)(pictureBox1.Height * upspeed / scale);
			if (full_downlines[full_downspeeds.Length - 1] > pictureBox1.Height) full_downlines[full_downspeeds.Length - 1] = pictureBox1.Height;
			if (full_uplines[full_downspeeds.Length - 1] > pictureBox1.Height) full_uplines[full_downspeeds.Length - 1] = pictureBox1.Height;

			if (autoscale_checked.Checked)
				Auto_Scale();
		}

		private void DrawIconRepresentation()
		{
			Bitmap b = new Bitmap(16, 16, PixelFormat.Format16bppRgb555);
			Graphics g = Graphics.FromImage((Image)b);
            g.FillRegion(new LinearGradientBrush(new PointF(b.Width / 2, 0), new PointF(b.Width / 2, b.Width ), Color.Gray, Color.Black), new Region(new Rectangle(0, 0, 16, 16)));

			if (!icon_representation)
			{
				//draw each line in the graph
				DrawGraph(g, 16, downlines, uplines, true);
			}
			else
			{
				//draw cool icon
				DrawCoolIcon(g);
				b.MakeTransparent(Color.FromArgb(255, 0, 255));

			}

			IntPtr oIcon = b.GetHicon();
			m_notifyicon.Icon = Icon.FromHandle(oIcon);
			g.Dispose();
			b.Dispose();
			DestroyIcon(oIcon);
		}

		private void DrawCoolIcon(Graphics graph)
		{
			graph.Clear(Color.FromArgb(255, 0, 255));

			if (icons_loaded)
			{
				if (upspeed > 0)
					graph.DrawImage(upload_icon_green, 0, 0, upload_icon_green.Width, upload_icon_green.Height);
				else
					graph.DrawImage(upload_icon_red, 0, 0, upload_icon_red.Width, upload_icon_red.Height);

				if (downspeed > 0)
					graph.DrawImage(download_icon_green, 0, 0, download_icon_green.Width, download_icon_green.Height);
				else
					graph.DrawImage(download_icon_red, 0, 0, download_icon_red.Width, download_icon_red.Height);
			}
			else
			{
				Pen color;

				if (upspeed > 0)
					color = Pens.Green;
				else
					color = Pens.Red;

				//upload
				for (int i = 6; i >= 0; i--)
					graph.DrawLine(color, 2 + i, 6 - i, 14 - i, 6 - i);

				if (downspeed > 0)
					color = Pens.Green;
				else
					color = Pens.Red;

				//download
				for (int i = 0; i < 6; i++)
					graph.DrawLine(color, 2 + i, 9 + i, 14 - i, 9 + i);
			}
		}

		private void DrawFullMeter()
		{
            if (this.Visible && pictureBox1.Width != 0 && pictureBox1.Height != 0)
			{
				int full_time_visible = timerInterval * pictureBox1.Width / 1000;
				Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format16bppRgb555);
				Graphics g = Graphics.FromImage((Image)bm);
                g.FillRegion(new LinearGradientBrush(new PointF(bm.Width / 2, 0), new PointF(bm.Width / 2, bm.Width), Color.Gray, Color.Black), new Region(new Rectangle(0, 0, bm.Width, bm.Height)));

				//draw each line in the graph
				DrawGraph(g, pictureBox1.Height, full_downlines, full_uplines, false);

				IntPtr oBm = bm.GetHbitmap();
				pictureBox1.Image = Image.FromHbitmap(oBm);


				DeleteObject(oBm);
				bm.Dispose();
				g.Dispose();
			}
			DoStringOutput();
		}
		private void Auto_Scale()
		{
			if (pictureBox1.Height * Max(full_downspeeds) / scale > pictureBox1.Height || pictureBox1.Height * Max(full_upspeeds) / scale > pictureBox1.Height)
			{
				switch (scale)
				{
					case 4200: scale = 7000; break; //33.6k
					case 7000: scale = 8000; break; //56k
					case 8000: scale = 16000; break; //64k
					case 16000: scale = 32000; break; //128k
					case 32000: scale = 80000; break; //256k
					case 64000: scale = 80000; break; //512k
					case 80000: scale = 128000; break; //640k
					case 128000: scale = 192000; break; //1m
					case 192000: scale = 256000; break; //1.5m
					case 256000: scale = 384000; break; //2m
					case 384000: scale = 640000; break; //3m
					case 640000: scale = 896000; break; //5m
					case 896000: scale = 1280000; break; //7m
					case 1280000: scale = 1408000; break; //10m
					case 1408000: scale = 4096000; break; //11m
					case 4096000: scale = 6912000; break; //32m
					case 6912000: scale = 12800000; break; //54m
					case 12800000: scale = 128000000; break; //100m
				}
				ResizeScale();
				Check_Menus();
			}
			else if (pictureBox1.Height * Max(full_downspeeds) / scale < pictureBox1.Height / 3 && pictureBox1.Height * Max(full_upspeeds) / scale < pictureBox1.Height / 3)
			{
				switch (scale)
				{
					case 7000: scale = 4200; break; //56k
					case 8000: scale = 7000; break; //64k
					case 16000: scale = 8000; break; //128k
					case 32000: scale = 16000; break; //256k
					case 64000: scale = 32000; break; //512k
					case 80000: scale = 64000; break; //640k
					case 128000: scale = 80000; break; //1m
					case 192000: scale = 128000; break; //1.5m
					case 256000: scale = 192000; break; //2m
					case 384000: scale = 256000; break; //3m
					case 640000: scale = 384000; break; //5m
					case 896000: scale = 640000; break; //7m
					case 1280000: scale = 896000; break; //10m
					case 1408000: scale = 1280000; break; //11m
					case 4096000: scale = 1408000; break; //32m
					case 6912000: scale = 4096000; break; //54m
					case 12800000: scale = 6912000; break; //100m
					case 128000000: scale = 12800000; break; //1g
				}
				ResizeScale();
				Check_Menus();
			}
		}
		private void DrawGraph(Graphics graph, int height, int[] dlines, int[] ulines, bool drawingIcon)
		{
			for (int i = 0; i < dlines.Length; i++)
			{
				if (dlines[i] > 0 || ulines[i] > 0)
				{
					if (dlines[i] > ulines[i])
					{
						if (graphs_download.Checked && graphs_upload.Checked)
						{
							graph.DrawLine(downloadPen, i, height, i, height - dlines[i]);
							graph.DrawLine(overlapPen, i, height, i, height - ulines[i]);
						}
						else if (graphs_download.Checked && !graphs_upload.Checked)
							graph.DrawLine(downloadPen, i, height, i, height - dlines[i]);
						else if (!graphs_download.Checked && graphs_upload.Checked)
							graph.DrawLine(uploadPen, i, height, i, height - ulines[i]);
					}
					else if (dlines[i] < ulines[i])
					{
						if (graphs_download.Checked && graphs_upload.Checked)
						{
							graph.DrawLine(uploadPen, i, height, i, height - ulines[i]);
							graph.DrawLine(overlapPen, i, height, i, height - dlines[i]);
						}
						else if (!graphs_download.Checked && graphs_upload.Checked)
							graph.DrawLine(uploadPen, i, height, i, height - ulines[i]);
						else if (graphs_download.Checked && !graphs_upload.Checked)
							graph.DrawLine(downloadPen, i, height, i, height - dlines[i]);
					}
					else if (dlines[i] == ulines[i])
					{
						if (graphs_upload.Checked && graphs_download.Checked)
							graph.DrawLine(overlapPen, i, height, i, height - ulines[i]);
						else if (!graphs_upload.Checked && graphs_download.Checked)
							graph.DrawLine(downloadPen, i, height, i, height - dlines[i]);
						else if (graphs_upload.Checked && !graphs_download.Checked)
							graph.DrawLine(uploadPen, i, height, i, height - ulines[i]);
					}
				}
			}


			int down = dlines[dlines.Length - 1];
			int up = ulines[ulines.Length - 1];

			if (graphs_download.Checked && graphs_summary.Checked)
			{
				graph.DrawLine(Pens.Black, dlines.Length - 2, 0, dlines.Length - 2, height);
				graph.DrawLine(Pens.Black, dlines.Length - 1, 0, dlines.Length - 1, height - down);
				graph.DrawLine(Pens.White, dlines.Length - 1, height, dlines.Length - 1, height - down);
			}

			if (graphs_upload.Checked && graphs_summary.Checked)
			{
				graph.DrawLine(Pens.Black, 1, 0, 1, height);
				graph.DrawLine(Pens.Black, 0, 0, 0, height - up);
				graph.DrawLine(Pens.White, 0, height, 0, height - up);
			}


			if (graph_label_checked.Checked && !drawingIcon)
			{
				Font f;
				string fontName = "Verdana";
				int fontSize = 6;

				if (font_large.Checked)
					f = new Font(fontName, fontSize + 2, FontStyle.Regular);
				else if (font_medium.Checked)
					f = new Font(fontName, fontSize + 1, FontStyle.Regular);
				else
					f = new Font(fontName, fontSize, FontStyle.Regular);

				string text = display_xscale + " " + display_yscale;
				SizeF size = graph.MeasureString(text, f);
				RectangleF rect = new RectangleF(new PointF(2, 2), size);

				//graph.FillRectangle(new SolidBrush(Color.Black), rect);
				graph.DrawString(text, f, new SolidBrush(Color.White), rect);
			}
		}
		private void DoStringOutput()
		{
			string downunits = "";
			string upunits = "";
			string downformat = "F1";
			string upformat = "F1";
			string label1text = "";
			string label2text = "";

			if (avg_checked.Checked)
			{
				double averageDown = Average(full_downspeeds);
				double averageUp = Average(full_upspeeds);
				if (units_kbits.Checked)
				{
					double downbps = averageDown * 8;
					double upbps = averageUp * 8;
					if (downbps < 1024)
					{
						downunits = "";
						downformat = "F0";
					}
					else if (downbps < 1024 * 1024)
					{ 
						downbps = downbps / 1024;
						downunits = "k";
					}
					else
					{
						downunits = "m";
						downbps = downbps / 1024 / 1024;
					}
					if (upbps < 1024)
					{
						upunits = "";
						upformat = "F0";
					}
					else if (upbps < 1024 * 1024)
					{
						upbps = upbps / 1024;
						upunits = "k";
					}
					else
					{
						upunits = "m";
						upbps = upbps / 1024 / 1024;
					}
					label1text += downbps.ToString(downformat) + " " + downunits + "bps";
					label2text += upbps.ToString(upformat) + " " + upunits + "bps";
				}
				if (units_kbytes.Checked)
				{
					if (averageDown < 1024)
					{
						downunits = "";
						downformat = "F0";
					}
					else if (averageDown < 1024 * 1024)
					{
						averageDown = averageDown / 1024;
						downunits = "k";
					}
					else
					{
						downunits = "m";
						averageDown = averageDown / 1024 / 1024;
					}
					if (averageUp < 1024)
					{
						upunits = "";
						upformat = "F0";
					}
					else if (averageUp < 1024 * 1024)
					{
						averageUp = averageUp / 1024;
						upunits = "k";
					}
					else
					{
						upunits = "m";
						averageUp = averageUp / 1024 / 1024;
					}
					label1text += " " + averageDown.ToString(downformat) + " " + downunits + "B/s";
					label2text += " " + averageUp.ToString(upformat) + " " + upunits + "B/s";
				}
				string nText = timerInterval * full_downspeeds.Length / 1000 + " sec Avg:\n" + label1text + " Down\n" + label2text + " Up";
				if (nText.Length >= 64)
					m_notifyicon.Text = nText.Substring(0, 64);
				else
					m_notifyicon.Text = nText;

			}
			else
			{
				if (units_kbits.Checked)
				{
					double downbps = downspeed * 8;
					double upbps = upspeed * 8;
					if (downbps < 1024)
					{
						downunits = "";
						downformat = "F0";
					}
					else if (downbps < 1024 * 1024)
					{
						downbps = downbps / 1024;
						downunits = "k";
					}
					else
					{
						downunits = "m";
						downbps = downbps / 1024 / 1024;
					}
					if (upbps < 1024)
					{
						upunits = "";
						upformat = "F0";
					}
					else if (upbps < 1024 * 1024)
					{
						upbps = upbps / 1024;
						upunits = "k";
					}
					else
					{
						upunits = "m";
						upbps = upbps / 1024 / 1024;
					}
					label1text += downbps.ToString(downformat) + " " + downunits + "bps";
					label2text += upbps.ToString(upformat) + " " + upunits + "bps";
				}
				if (units_kbytes.Checked)
				{
					if (downspeed < 1024)
					{
						downunits = "";
						downformat = "F0";
					}
					else if (downspeed < 1024 * 1024)
					{
						downspeed = downspeed / 1024;
						downunits = "k";
					}
					else
					{
						downunits = "m";
						downspeed = downspeed / 1024 / 1024;
					}
					if (upspeed < 1024)
					{
						upunits = "";
						upformat = "F0";
					}
					else if (upspeed < 1024 * 1024)
					{
						upspeed = upspeed / 1024;
						upunits = "k";
					}
					else
					{
						upunits = "m";
						upspeed = upspeed / 1024 / 1024;
					}
					label1text += " " + downspeed.ToString(downformat) + " " + downunits + "B/s";
					label2text += " " + upspeed.ToString(upformat) + " " + upunits + "B/s";
				}
				string nText = label1text + " Down\n" + label2text + " Up";
				if (nText.Length >= 64)
					m_notifyicon.Text = nText.Substring(0, 64);
				else
					m_notifyicon.Text = nText;
			}
			SetText(label1, label1text, 0);
			SetText(label2, label2text, WLength / 2 - 5);
		}

		//Average and max functions
		private static double Average(int[] num)
		{
			double sum = 0.0;
			double avg = 0.0;
			for (int i = 0; i < num.Length; i++)
				sum += num[i];
			if (num.Length > 0)
				avg = sum / System.Convert.ToDouble(num.Length);
			return avg;
		}
		private static double Average(double[] num)
		{
			double sum = 0.0;
			double avg = 0.0;
			for (int i = 0; i < num.Length; i++)
				sum += num[i];
			if (num.Length > 0)
				avg = sum / System.Convert.ToDouble(num.Length);
			return avg;
		}
		private static double Max(double[] A)
		{
			double maxVal = A[0];
			for (int i = 1; i < A.Length; i++)
				if (A[i] > maxVal)
					maxVal = A[i];
			return maxVal;
		}

		//handle form mouse click and drag events
		private Point ptOffset;
		private void Icon_MouseDown(Object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (WindowState == FormWindowState.Normal)
				{
					WindowState = FormWindowState.Minimized;
					show_checked.Checked = false;
					Hide();
				}
				else
				{
					this.Show();
					show_checked.Checked = true;
					WindowState = FormWindowState.Normal;
				}
			}
			else Check_Menus();
		}
		private void Main_MouseDown(Object sender, MouseEventArgs e)
		{
			ptOffset = new Point(-e.X - pictureBox1.Left, -e.Y - pictureBox1.Top);
			Check_Menus();
		}
		private void Form1_MouseMove(Object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Point mousePos = Control.MousePosition;
				mousePos.Offset(ptOffset.X, ptOffset.Y);
				this.Location = mousePos;
			}
		}

		//for resizing
		static int frmLastWidth = 0, frmLastHeight = 0, frmWidth, frmHeight;
		static bool frmIsResizing = false;
		Rectangle frmRectangle = new Rectangle();
		private void Resize_MouseDown(object sender, MouseEventArgs e)
		{
			frmWidth = WLength;
			frmHeight = WHeight;
			frmRectangle.Location = new Point(this.Left, this.Top);
			frmRectangle.Size = new Size(frmWidth, frmHeight);
		}
		private void Resize_MouseUp(object sender, MouseEventArgs e)
		{
			if (frmIsResizing)
			{
				frmRectangle.Location = new Point(this.Left, this.Top);
				frmRectangle.Size = new Size(frmWidth, frmHeight);
				this.Size = frmRectangle.Size;
				this.Width = frmWidth;
				this.Height = frmHeight;
				frmIsResizing = false;
			}
			Form1_Borders();
		}
		private void Resize_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int sizeageX = (MousePosition.X - this.Location.X);
				int sizeageY = (MousePosition.Y - this.Location.Y);
				if (sizeageX < 66) sizeageX = 66;
				if (sizeageY < 45) sizeageY = 45;
				frmWidth = sizeageX;
				frmHeight = sizeageY;
				if (frmLastWidth == 0)
					frmLastWidth = frmWidth;
				if (frmLastHeight == 0)
					frmLastHeight = frmHeight;
				if (frmIsResizing)
				{
					frmRectangle.Location = new Point(this.Left, this.Top);
					frmRectangle.Size = new Size(frmLastWidth, frmLastHeight);
				}
				frmIsResizing = true;
				frmLastWidth = frmWidth;
				frmLastHeight = frmHeight;
				frmRectangle.Location = new Point(this.Left, this.Top);
				frmRectangle.Size = new Size(frmWidth, frmHeight);
				this.Size = frmRectangle.Size;
			}
		}
		private void Form1_Resize(Object sender, EventArgs e)
		{
			if (ClientSize.Width > 40 && ClientSize.Height > 40)
			{
				WLength = ClientSize.Width;
				WHeight = ClientSize.Height;
			}

			label1.Location = new Point(10, WHeight - 14);
			label1.Size = new Size(WLength / 2 - 9 - 5, 13);
			label2.Location = new Point(WLength / 2 + 10 - 5, WHeight - 14);
			label2.Size = new Size(WLength / 2 - 9 - 13 + 5, 13);
			label3.Location = new Point(1, WHeight - 14);
			label4.Location = new Point(WLength / 2 - 4, WHeight - 14);
			pictureBox1.Size = new Size(WLength - 6, WHeight - 18);
			resizer.Location = new Point(WLength - 13, WHeight - 13);
			trackBar1.Location = new Point(2, WHeight - 14);
			trackBar1.Size = new Size(WLength - 15, 15);
			trackBar2.Location = new Point(2, WHeight - 14);
			trackBar2.Size = new Size(WLength - 15, 15);

			display_xscale = "time: " + (timerInterval * pictureBox1.Width / 1000).ToString() + "s ";

			//resize arrays to match new window size
			int[] temp = new int[pictureBox1.Width];
			double[] temp2 = new double[pictureBox1.Width];
			if (full_downlines.Length <= temp.Length)
				Array.Copy(full_downlines, 0, temp, temp.Length - full_downlines.Length, full_downlines.Length);
			else
				Array.Copy(full_downlines, full_downlines.Length - temp.Length, temp, 0, temp.Length);
			full_downlines = temp;

			temp = new int[pictureBox1.Width];
			if (full_uplines.Length <= temp.Length)
				Array.Copy(full_uplines, 0, temp, temp.Length - full_uplines.Length, full_uplines.Length);
			else
				Array.Copy(full_uplines, full_uplines.Length - temp.Length, temp, 0, temp.Length);
			full_uplines = temp;

			temp2 = new double[pictureBox1.Width];
			if (full_downspeeds.Length <= temp2.Length)
				Array.Copy(full_downspeeds, 0, temp2, temp2.Length - full_downspeeds.Length, full_downspeeds.Length);
			else
				Array.Copy(full_downspeeds, full_downspeeds.Length - temp2.Length, temp2, 0, temp2.Length);
			full_downspeeds = temp2;

			temp2 = new double[pictureBox1.Width];
			if (full_upspeeds.Length <= temp2.Length)
				Array.Copy(full_upspeeds, 0, temp2, temp2.Length - full_upspeeds.Length, full_upspeeds.Length);
			else
				Array.Copy(full_upspeeds, full_upspeeds.Length - temp2.Length, temp2, 0, temp2.Length);
			full_upspeeds = temp2;

			int font_adjust = 0;
			if (!font_small.Checked) font_adjust = 1;
			if (WLength > 125)
			{
				if (font_large.Checked) label1.Font = label2.Font = new Font("MS Serif", 7 + font_adjust, FontStyle.Bold);
				else label1.Font = label2.Font = new Font("MS Serif", 7 + font_adjust, FontStyle.Regular);
				label1.TextAlign = label2.TextAlign = ContentAlignment.TopLeft;
			}
			else if (WLength > 95)
			{
				if (font_large.Checked) label1.Font = label2.Font = new Font("MS Serif", 6 + font_adjust, FontStyle.Bold);
				else label1.Font = label2.Font = new Font("MS Serif", 6 + font_adjust, FontStyle.Regular);
				label1.TextAlign = label2.TextAlign = ContentAlignment.MiddleLeft;
			}
			else
			{
				if (font_large.Checked) label1.Font = label2.Font = new Font("MS Serif", 5 + font_adjust, FontStyle.Bold);
				else label1.Font = label2.Font = new Font("MS Serif", 5 + font_adjust, FontStyle.Regular);
				label1.TextAlign = label2.TextAlign = ContentAlignment.MiddleLeft;
			}
			ResizeScale();
		}

		//HSL <-> RGB (Hue/Saturation/Luminosity to/from Red/Green/Blue color format)
		public Color HSL_to_RGB(double h, double s, double l)
		{
			double r = 0, g = 0, b = 0;
			double temp1, temp2;
			h = h / 360.0;
			if (l == 0)
				r = g = b = 0;
			else
			{
				if (s == 0)
					r = g = b = l;
				else
				{
					temp2 = ((l <= 0.5) ? l * (1.0 + s) : l + s - (l * s));
					temp1 = 2.0 * l - temp2;
					double[] t3 = new double[] { h + 1.0 / 3.0, h, h - 1.0 / 3.0 };
					double[] clr = new double[] { 0, 0, 0 };
					for (int i = 0; i < 3; i++)
					{
						if (t3[i] < 0)
							t3[i] += 1.0;
						if (t3[i] > 1)
							t3[i] -= 1.0;
						if (6.0 * t3[i] < 1.0)
							clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
						else if (2.0 * t3[i] < 1.0)
							clr[i] = temp2;
						else if (3.0 * t3[i] < 2.0)
							clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
						else
							clr[i] = temp1;
					}
					r = clr[0];
					g = clr[1];
					b = clr[2];
				}
			}
			try
			{
				return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
			}
			catch (ArgumentException)
			{
				return BACKGROUND_COLOR;
			}
		}
		public void RGB_to_HSL(Color c, ref double h, ref double s, ref double l)
		{
			h = c.GetHue();
			s = c.GetSaturation();
			l = c.GetBrightness();
		}

		// Menus and menu click handlers
		private MenuItem m_interval_menu = new MenuItem(), m_scale_menu = new MenuItem(), m_units = new MenuItem(), m_interfaces = new MenuItem(), m_graphs = new MenuItem(), m_colors = new MenuItem(), m_utils = new MenuItem();
		private MenuItem interval_tenth, interval_fifth, interval_half, interval_1;
		private MenuItem scale_33, scale_56, scale_64, scale_128, scale_256, scale_512, scale_640, scale_1000, scale_1500, scale_2000, scale_3000, scale_5000, scale_7000, scale_10000, scale_11000, scale_32000, scale_54000, scale_100000, scale_1000000, scale_custom;
		private MenuItem avg_checked, clip_watch, show_checked, topmost_checked, autoscale_checked, graph_label_checked, mailcheck, mailchecknow;
		/* added by miechu */
		private MenuItem simple_icon_checked;
		/* end of added by miechu */
		private MenuItem units_kbits, units_kbytes, graphs_download, graphs_upload, graphs_summary, colorcycle, m_update;
		private MenuItem font_large, font_medium, font_small;
		private void MakeMenus()
		{
			m_menu.MenuItems.Add(0, show_checked = new MenuItem("Show Desktop Meter", new EventHandler(Show_Click)));
			show_checked.Checked = true;
			m_menu.MenuItems.Add(topmost_checked = new MenuItem("Always On Top", new EventHandler(TopMost_Click)));
			/* added by miechu */
			m_menu.MenuItems.Add(simple_icon_checked = new MenuItem("Simple Notify Icon", new EventHandler(SimpleNotifyIcon_Click)));
			simple_icon_checked.Checked = false;
			/* end of added by miechu */
			m_menu.MenuItems.Add(m_colors);
			m_menu.MenuItems.Add(new MenuItem("-"));
			m_menu.MenuItems.Add(avg_checked = new MenuItem("Display Averages", new EventHandler(Avg_Click)));
			m_menu.MenuItems.Add(m_interval_menu);
			m_menu.MenuItems.Add(m_scale_menu);
			m_menu.MenuItems.Add(m_graphs);
			m_menu.MenuItems.Add(m_units);
			m_menu.MenuItems.Add(m_interfaces);
			m_menu.MenuItems.Add(new MenuItem("-"));
			m_menu.MenuItems.Add(m_utils);
			m_menu.MenuItems.Add(new MenuItem("-"));
			m_menu.MenuItems.Add(new MenuItem("About FreeMeter", new EventHandler(About_Click)));
			m_menu.MenuItems.Add(new MenuItem("Exit FreeMeter", new EventHandler(Exit_Click)));

			m_colors.Text = "Colors/Opacity";
			m_colors.MenuItems.Add(colorcycle = new MenuItem("Cycle Colors", new EventHandler(Cycle_Colors)));
			m_colors.MenuItems.Add(new MenuItem("-"));
			m_colors.MenuItems.Add(new MenuItem("Hue Slider", new EventHandler(Trackbar1_Show)));
			m_colors.MenuItems.Add(new MenuItem("Color", new EventHandler(Color_Click)));
			m_colors.MenuItems.Add(new MenuItem("Text Color", new EventHandler(TextColor_Click)));
			m_colors.MenuItems.Add(new MenuItem("Reset To Default", new EventHandler(DefaultColor_Click)));
			m_colors.MenuItems.Add(new MenuItem("-"));
			m_colors.MenuItems.Add(new MenuItem("Transparency Slider", new EventHandler(Trackbar2_Show)));
			m_colors.MenuItems.Add(new MenuItem("Opaque", new EventHandler(Opaque_Click)));

			m_interval_menu.Text = "Update Interval";
			m_interval_menu.MenuItems.Add(interval_tenth = new MenuItem("1/10 second", new EventHandler(SetTimerInterval)));
			m_interval_menu.MenuItems.Add(interval_fifth = new MenuItem("1/5 second", new EventHandler(SetTimerInterval)));
			m_interval_menu.MenuItems.Add(interval_half = new MenuItem("1/2 second", new EventHandler(SetTimerInterval)));
			m_interval_menu.MenuItems.Add(interval_1 = new MenuItem("1 second", new EventHandler(SetTimerInterval)));

			m_scale_menu.Text = "Graph Scale";
			m_scale_menu.MenuItems.Add(autoscale_checked = new MenuItem("Auto", new EventHandler(SetAutoScale)));
			m_scale_menu.MenuItems.Add(new MenuItem("-"));
			m_scale_menu.MenuItems.Add(scale_33 = new MenuItem("33.6 kb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_56 = new MenuItem("56 kb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_64 = new MenuItem("64 kb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_128 = new MenuItem("128 kb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_256 = new MenuItem("256 kb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_512 = new MenuItem("512 kb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_640 = new MenuItem("640 kb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_1000 = new MenuItem("1 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_1500 = new MenuItem("1.5 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_2000 = new MenuItem("2 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_3000 = new MenuItem("3 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_5000 = new MenuItem("5 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_7000 = new MenuItem("7 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_10000 = new MenuItem("10 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_11000 = new MenuItem("11 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_32000 = new MenuItem("32 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_54000 = new MenuItem("54 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_100000 = new MenuItem("100 mb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_1000000 = new MenuItem("1 gb", new EventHandler(SetScale_MenuClick)));
			m_scale_menu.MenuItems.Add(scale_custom = new MenuItem("custom", new EventHandler(SetScale_MenuClick)));

			m_units.Text = "Units";
			m_units.MenuItems.Add(units_kbits = new MenuItem("Bits per sec (eg kbps)", new EventHandler(SetUnits_kbits)));
			m_units.MenuItems.Add(units_kbytes = new MenuItem("Bytes per sec (eg kB/s)", new EventHandler(SetUnits_kbytes)));

			m_interfaces.Text = "Interfaces";
			foreach (NetworkAdapter adapter in monitor.Adapters)
			{
				MenuItem tmp = new MenuItem(adapter.name, new EventHandler(SetAdapter));
				m_interfaces.MenuItems.Add(tmp);
				tmp.Checked = adapter.Enabled;
			}

			m_utils.Text = "Utilities";
			m_utils.MenuItems.Add(clip_watch = new MenuItem("URL Grabber Enabled", new EventHandler(Clip_Click)));
			m_utils.MenuItems.Add(new MenuItem("-"));
			m_utils.MenuItems.Add(mailcheck = new MenuItem("Email Notify Enabled", new EventHandler(CheckMail_Auto)));
			m_utils.MenuItems.Add(mailchecknow = new MenuItem("Check Email Now", new EventHandler(CheckMail_Now)));
			m_utils.MenuItems.Add(new MenuItem("Email Server Settings", new EventHandler(CheckMail_Settings)));
			m_utils.MenuItems.Add(new MenuItem("-"));
			m_utils.MenuItems.Add(new MenuItem("Ping Utility", new EventHandler(Ping_Click)));
			m_utils.MenuItems.Add(new MenuItem("Traceroute Utility", new EventHandler(Trace_Click)));
			m_utils.MenuItems.Add(new MenuItem("UPnP NAT Utility", new EventHandler(UPnP_Click)));
			m_utils.MenuItems.Add(new MenuItem("-"));
			m_utils.MenuItems.Add(new MenuItem("Totals Log", new EventHandler(ShowTotalsLog_Click)));
			m_utils.MenuItems.Add(m_update = new MenuItem("Check For Updates", new EventHandler(Check_Version)));

			m_graphs.Text = "Graphs";
			m_graphs.MenuItems.Add(graph_label_checked = new MenuItem("Show Graph Heading", new EventHandler(SetGraph_Label)));
			m_graphs.MenuItems.Add(graphs_summary = new MenuItem("Show Summary On Left(up) and Right(down)", new EventHandler(SetGraph_Summary)));
			
			m_graphs.MenuItems.Add(new MenuItem("-"));
			m_graphs.MenuItems.Add(graphs_download = new MenuItem("Download", new EventHandler(SetGraph_Download)));
			m_graphs.MenuItems.Add(graphs_upload = new MenuItem("Upload", new EventHandler(SetGraph_Upload)));
			m_graphs.MenuItems.Add(new MenuItem("-"));
			m_graphs.MenuItems.Add(font_large = new MenuItem("Large Font", new EventHandler(SetFont_Large)));
			m_graphs.MenuItems.Add(font_medium = new MenuItem("Medium Font", new EventHandler(SetFont_Medium)));
			m_graphs.MenuItems.Add(font_small = new MenuItem("Small Font", new EventHandler(SetFont_Small)));
		}

		public void Check_Menus()
		{
			interval_tenth.Checked = interval_fifth.Checked = interval_half.Checked = interval_1.Checked = false;
			scale_33.Checked = scale_56.Checked = scale_64.Checked = scale_128.Checked = scale_256.Checked = scale_512.Checked = scale_640.Checked = scale_1000.Checked = scale_1500.Checked = scale_2000.Checked = scale_3000.Checked = scale_5000.Checked = scale_7000.Checked = scale_10000.Checked = scale_11000.Checked = scale_32000.Checked = scale_54000.Checked = scale_100000.Checked = scale_1000000.Checked = scale_custom.Checked = false;

			if (timerInterval == 100) 
				interval_tenth.Checked = true;
			else if (timerInterval == 200) 
				interval_fifth.Checked = true;
			else if (timerInterval == 500) 
				interval_half.Checked = true;
			else if (timerInterval == 1000) 
				interval_1.Checked = true;

			switch (scale)
			{
				case 4200: scale_33.Checked = true; display_yscale = "scale: 33.6 kb"; break;
				case 7000: scale_56.Checked = true; display_yscale = "scale: 56 kb"; break;
				case 8000: scale_64.Checked = true; display_yscale = "scale: 64 kb"; break;
				case 16000: scale_128.Checked = true; display_yscale = "scale: 128 kb"; break;
				case 32000: scale_256.Checked = true; display_yscale = "scale: 256 kb"; break;
				case 64000: scale_512.Checked = true; display_yscale = "scale: 512 kb"; break;
				case 80000: scale_640.Checked = true; display_yscale = "scale: 640 kb"; break;
				case 128000: scale_1000.Checked = true; display_yscale = "scale: 1 mb"; break;
				case 192000: scale_1500.Checked = true; display_yscale = "scale: 1.5 mb"; break;
				case 256000: scale_2000.Checked = true; display_yscale = "scale: 2 mb"; break;
				case 384000: scale_3000.Checked = true; display_yscale = "scale: 3 mb"; break;
				case 640000: scale_5000.Checked = true; display_yscale = "scale: 5 mb"; break;
				case 896000: scale_7000.Checked = true; display_yscale = "scale: 7 mb"; break;
				case 1280000: scale_10000.Checked = true; display_yscale = "scale: 10 mb"; break;
				case 1408000: scale_11000.Checked = true; display_yscale = "scale: 11 mb"; break;
				case 4096000: scale_32000.Checked = true; display_yscale = "scale: 32 mb"; break;
				case 6912000: scale_54000.Checked = true; display_yscale = "scale: 54 mb"; break;
				case 12800000: scale_100000.Checked = true; display_yscale = "scale: 100 mb"; break;
				case 128000000: scale_1000000.Checked = true; display_yscale = "scale: 1 gb"; break;
				default: scale_custom.Checked = true; display_yscale = "scale: custom (" + Totals_Log.Value(scale, null) + ")"; break;
			}
		}

		// handlers for menu clicks
		private void SetTimerInterval(Object sender, EventArgs e)
		{
			foreach (MenuItem m in m_interval_menu.MenuItems)
			{
				if (m.Equals(sender))
				{
					if (!m.Checked)
						m.Checked = true;
					
					switch (m.Text)
					{
						case "1/10 second": timerInterval = 100; break;
						case "1/5 second": timerInterval = 200; break;
						case "1/2 second": timerInterval = 500; break;
						case "1 second": timerInterval = 1000; break;
					}
					display_xscale = "time: " + timerInterval * pictureBox1.Width / 1000 + "s ";
				}
				else
				{
					m.Checked = false;
				}
			}
		}

		private void SetAutoScale(Object sender, EventArgs e)
		{
			autoscale_checked.Checked = !autoscale_checked.Checked;
		}

		private void ResizeScale()//resize line values in array to match new scale.
		{
			for (int i = 0; i < full_downlines.Length; i++)
			{
				full_downlines[i] = (int)(pictureBox1.Height * full_downspeeds[i] / scale);
				full_uplines[i] = (int)(pictureBox1.Height * full_upspeeds[i] / scale);
			}
			for (int i = 0; i < downlines.Length; i++)
			{
				downlines[i] = 16 * (int)full_downspeeds[full_downlines.Length - 16 + i] / scale;
				uplines[i] = 16 * (int)full_upspeeds[full_downlines.Length - 16 + i] / scale;
			}
		}

		private void SetScale_MenuClick(Object sender, EventArgs e)//from a menu click to change graph scale
		{
			foreach (MenuItem m in m_scale_menu.MenuItems)
			{
				if (m != autoscale_checked)
				{
					if (m.Equals(sender))
					{
						if (!m.Checked)
							m.Checked = true;
						switch (m.Text)
						{
							case "33.6 kb":	scale = 4200; break;
							case "56 kb":	scale = 7000; break;
							case "64 kb":	scale = 8000; break;
							case "128 kb":	scale = 16000; break;
							case "256 kb":	scale = 32000; break;
							case "512 kb":	scale = 64000; break;
							case "640 kb":	scale = 80000; break;
							case "1 mb":	scale = 128000; break;
							case "1.5 mb":	scale = 192000; break;
							case "2 mb":	scale = 256000; break;
							case "3 mb":	scale = 384000; break;
							case "5 mb":	scale = 640000; break;
							case "7 mb":	scale = 896000; break;
							case "10 mb":	scale = 1280000; break;
							case "11 mb":	scale = 1408000; break;
							case "32 mb":	scale = 4096000; break;
							case "54 mb":	scale = 6912000; break;
							case "100 mb":	scale = 12800000; break;
							case "1 gb":	scale = 128000000; break;
							
							//custom
							default: 
									{
										GetValue g = new GetValue("Provide custom scale in bytes (1024B = 1KB)");

										if (g.ShowDialog() == DialogResult.OK)
										{
											try
											{
												scale = int.Parse(g.Value);
												m.Text = "custom (" + Totals_Log.Value(scale, null) + ")";
											}
											catch
											{ 
												m.Text = "custom";
											}
										}

										break;
									}
						}

						Check_Menus();
						ResizeScale();
					}
					else
					{
						m.Checked = false;
					}
				}
			}
		}

		private void SetUnits_kbits(Object sender, EventArgs e)
		{
			units_kbits.Checked = !units_kbits.Checked;
		}

		private void SetUnits_kbytes(Object sender, EventArgs e)
		{
			units_kbytes.Checked = !units_kbytes.Checked;
		}

		private void SetAdapter(Object sender, EventArgs e)
		{
			foreach (NetworkAdapter adapter in monitor.Adapters)
			{
				foreach (MenuItem m in m_interfaces.MenuItems)
				{
					if (m == sender && adapter.name == m.Text)
					{
						if (m.Checked)
							m.Checked = adapter.Enabled = false;
						else
						{
							m.Checked = adapter.Enabled = true;
							adapter.init();
						}
					}
				}
			}
		}
		

		private void SetGraph_Summary(Object sender, EventArgs e)
		{
			graphs_summary.Checked = !graphs_summary.Checked;
		}

		private void SetGraph_Label(Object sender, EventArgs e)
		{
			graph_label_checked.Checked = !graph_label_checked.Checked;
		}

		private void SetGraph_Download(Object sender, EventArgs e)
		{
			graphs_download.Checked = !graphs_download.Checked;
		}
		
		private void SetGraph_Upload(Object sender, EventArgs e)
		{
			graphs_upload.Checked = !graphs_upload.Checked;
		}
		
		private void SetFont_Large(Object sender, EventArgs e)
		{
			font_large.Checked = true;
			font_medium.Checked = font_small.Checked = false;
		}
		
		private void SetFont_Medium(Object sender, EventArgs e)
		{
			font_medium.Checked = true;
			font_large.Checked = font_small.Checked = false;
		}
		
		private void SetFont_Small(Object sender, EventArgs e)
		{
			font_small.Checked = true;
			font_medium.Checked = font_large.Checked = false;
		}
		
		private void Avg_Click(Object sender, EventArgs e)
		{
			avg_checked.Checked = !avg_checked.Checked;
		}

		private void Clip_Click(Object sender, EventArgs e)
		{
			if (clip_watch.Checked)
			{
				clip_watch.Checked = false;
				ClipData = null;
				ClipTimer.Stop();
			}
			else
			{
				clip_watch.Checked = true;
				ClipData = "";
				ClipTimer.Start();
			}
		}

		private void Show_Click(Object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Normal)
			{
				WindowState = FormWindowState.Minimized;
				show_checked.Checked = false;
				Hide();
			}
			else
			{
				Show();
				show_checked.Checked = true;
				WindowState = FormWindowState.Normal;

				Rectangle rect = Screen.PrimaryScreen.WorkingArea;

				if (this.Location.X > rect.Width || this.Location.Y > rect.Height || this.Location.Y < 0 || this.Location.X < 0)
					this.CenterToScreen();
			}
		}

		private void TopMost_Click(Object sender, EventArgs e)
		{
			topmost_checked.Checked = !topmost_checked.Checked;
			TopMost = topmost_checked.Checked;
		}

		private void SimpleNotifyIcon_Click(Object sender, EventArgs e)
		{
			simple_icon_checked.Checked = !simple_icon_checked.Checked;
			icon_representation = simple_icon_checked.Checked;
		}

		private void Exit_Click(Object sender, EventArgs e)
		{
			m_closing = true;
			Application.Exit();
		}

		private void About_Click(Object sender, EventArgs e)
		{
			AboutForm.ShowAboutForm(this);
		}
		//handlers for mail menu clicks
		private void CheckMail_Auto(Object sender, EventArgs e)
		{
			if (mailcheck.Checked)
			{
				mailcheck.Checked = false;
				MailTimer.Stop();
			}
			else
			{
				mailcheck.Checked = true;
				MailTimer.Start();
			}
		}
		private void CheckMail_Now(Object sender, EventArgs e)
		{
			MailTimer_Tick(sender, e);
		}

		private void CheckMail_Settings(Object sender, EventArgs e)
		{
			EmailSettings_Form frm = new EmailSettings_Form();
			frm.MyParentForm = this;
			if (frm.ShowDialog() == DialogResult.OK)
			{
				MailServers.Clear();
				for (int i = 0; i < frm.har.Count; i++)
				{
					MailServer server = new MailServer();
					server.Host = frm.har[i].ToString(); server.User = frm.uar[i].ToString(); server.Pass = frm.par[i].ToString(); server.Enabled = (bool)frm.ear[i]; server.Type = frm.tar[i].ToString();
					MailServers.Add(server);
				}
				MailTimer.Interval = frm.Time * 1000 * 60;
			}
			frm.Dispose();
		}

		private void Ping_Click(Object sender, EventArgs e)
		{
			AdvPing frm = new AdvPing();
			frm.MyParentForm = this;
			frm.Show(this);
		}
		private void Trace_Click(Object sender, EventArgs e)
		{
			AdvTrace frm = new AdvTrace();
			frm.MyParentForm = this;
			frm.Show(this);
		}
		private void UPnP_Click(Object sender, EventArgs e)
		{
			frmUPnP frm = new frmUPnP();
			frm.MyParentForm = this;
			frm.Show(this);
		}

		//check version
		private void Check_Version(object sender, EventArgs e)
		{
			if (sender.Equals(m_update))
				respond_to_latest = true;
			BackgroundWorker checkversionWorker = new BackgroundWorker();
			checkversionWorker.WorkerReportsProgress = false;
			checkversionWorker.DoWork += new DoWorkEventHandler(CheckVersionWorker_DoWork);
			checkversionWorker.RunWorkerAsync();
			checkversionWorker.Dispose();
		}
		private void CheckVersionWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			AssemblyName ThisAssemblyName = myAssembly.GetName();
			string FriendlyVersion = "v" + ThisAssemblyName.Version.Major + "." + ThisAssemblyName.Version.Minor + "." + ThisAssemblyName.Version.Build;
			if (!respond_to_latest)
				Thread.Sleep(30000);
			try
			{
				WebRequest w = WebRequest.Create("http://freemeter.cvs.sourceforge.net/*checkout*/freemeter/FM_CVS/changelog.txt?revision=HEAD");
				Stream sw = w.GetResponse().GetResponseStream();
				StreamReader sr = new StreamReader(sw);
				string line = sr.ReadLine();

				int result = String.Compare(line, 1, FriendlyVersion, 1, 8, true, CultureInfo.InvariantCulture);

				if (result < 0)
					m_notifyicon.ShowBalloonTip(1, "Your version is newer.", line + " is online version. You have " + FriendlyVersion + ".", ToolTipIcon.Info);
				else if (result > 0)
					m_notifyicon.ShowBalloonTip(1, "New Update Is Available", line + " is available. You have " + FriendlyVersion + ".\nCheck About dialog for download site.", ToolTipIcon.Info);
				else if (respond_to_latest)
				{
					m_notifyicon.ShowBalloonTip(1, "No New Updates", "You have the latest version (" + line + ").", ToolTipIcon.Info);
					respond_to_latest = false;
				}

				sr.Close();
				sr.Dispose();
				sw.Close();
				sw.Dispose();
			}
			catch (Exception ex)
			{
				m_notifyicon.ShowBalloonTip(1, "Check For Update", ex.Message, ToolTipIcon.Error);
			}
		}

		// Registry reading/writing, and form Dispose override
		private void RestoreRegistry()
		{
			try
			{
				Registry.CurrentUser.DeleteSubKey("Software\\FreeMeter");
			}
			catch
			{ }
		}

		protected override void Dispose(bool disposing)
		{
			if (backgroundWorker1 != null)
				backgroundWorker1.Abort();

			SaveConfiguration();
			
			if (disposing)
				m_notifyicon.Dispose();

			base.Dispose(disposing);
		}

		private void LoadConfiguration()
		{
			string app_dir = Application.ExecutablePath;
			app_dir = app_dir.Remove(app_dir.LastIndexOf('\\'));

			XmlDocument xml_doc = new XmlDocument();
			xml_doc.Load(app_dir + "\\config.xml");

			Hashtable xml = new Hashtable();

			foreach (XmlNode node in xml_doc.DocumentElement.ChildNodes)
				xml[node.Name] = node.InnerText;

			avg_checked.Checked			= bool.Parse(xml["DispAvg"].ToString());
			units_kbytes.Checked		= bool.Parse(xml["UnitsKbytes"].ToString());
			units_kbits.Checked			= bool.Parse(xml["UnitsKbits"].ToString());
			graphs_download.Checked		= bool.Parse(xml["GraphDownload"].ToString());
			graphs_upload.Checked		= bool.Parse(xml["GraphUpload"].ToString());
			
			autoscale_checked.Checked	= bool.Parse(xml["AutoScale"].ToString());
			topmost_checked.Checked		= bool.Parse(xml["TopMost"].ToString());
			simple_icon_checked.Checked = bool.Parse(xml["SimpleNotifyIcon"].ToString());
			graph_label_checked.Checked = bool.Parse(xml["ShowGraphLabel"].ToString());
			colorcycle.Checked			= bool.Parse(xml["ColorCycle"].ToString());
			mailcheck.Checked			= bool.Parse(xml["MailCheck"].ToString());
			clip_watch.Checked			= bool.Parse(xml["ClipWatch"].ToString());
			LogEnabled					= bool.Parse(xml["LogEnabled"].ToString());

			timerInterval = int.Parse(xml["TimerInterval"].ToString());
			LogInterval = int.Parse(xml["LogInterval"].ToString());
			scale = int.Parse(xml["GraphScale"].ToString());

			WLength = int.Parse(xml["DispWidth"].ToString());
			WHeight = int.Parse(xml["DispHeight"].ToString());
			full_downlines = new int[WLength];
			full_uplines = new int[WLength];
			full_downspeeds = new double[WLength];
			full_upspeeds = new double[WLength];
			ClientSize = new Size(WLength, WHeight);

			if (xml["WindowIsVisible"].ToString().ToLower() == "true")
			{
				Show();
				Location = new Point(int.Parse(xml["WindowX"].ToString()), int.Parse(xml["WindowY"].ToString()));
				show_checked.Checked = true;
				WindowState = FormWindowState.Normal;
			}
			else
			{
				Location = new Point(int.Parse(xml["WindowX"].ToString()), int.Parse(xml["WindowY"].ToString()));
				WindowState = FormWindowState.Minimized;
				show_checked.Checked = false;
				Hide();
               
			}


			if (int.Parse(xml["FontSize"].ToString()) == 2)
				font_large.Checked = true;
			else if (int.Parse(xml["FontSize"].ToString()) == 1)
				font_medium.Checked = true;
			else
				font_small.Checked = true;

			string host = xml["PopServer"].ToString();
			string user = xml["PopUser"].ToString();
			string pass = xml["PopPass"].ToString();
			string enab = xml["PopEnabled"].ToString();
			string type = xml["PopType"].ToString();

			string[] htokens = host.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			string[] utokens = user.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			string[] ptokens = pass.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			string[] etokens = enab.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			string[] ttokens = type.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			
			for (int i = 0; i < htokens.Length; i++)
			{
				MailServer server = new MailServer();
				server.Host = htokens[i]; server.User = utokens[i]; server.Pass = Decrypt(ptokens[i], utokens[i]); server.Type = ttokens[i];
				if (etokens[i] == "True")
					server.Enabled = true;
				else
					server.Enabled = false;
				server.OldMsgCount = 0;
				MailServers.Add(server);
			}

			MailServers.RemoveAt(0); //remove the default value
			MailTimer.Interval = int.Parse(xml["MailCheckInterval"].ToString());

			if (mailcheck.Checked)
				MailTimer.Start();

			if (clip_watch.Checked)
				ClipTimer.Start();

			LogTimer.Interval = LogInterval * 60000;

			if (LogEnabled)
				LogTimer.Start();

			trackBar2.Maximum = 100;
			trackBar2.Minimum = 30;
			trackBar2.Value = int.Parse(xml["Trans"].ToString());
			Opacity = ((float)trackBar2.Value) / 100;

			BACKGROUND_COLOR = Color.FromArgb(255, int.Parse(xml["BackgroundRed"].ToString()), int.Parse(xml["BackgroundGreen"].ToString()), int.Parse(xml["BackgroundBlue"].ToString()));
			FORGROUND_COLOR = Color.FromArgb(255, int.Parse(xml["ForegroundRed"].ToString()), int.Parse(xml["ForegroundGreen"].ToString()), int.Parse(xml["ForegroundBlue"].ToString()));

			TopMost = topmost_checked.Checked;
			icon_representation = simple_icon_checked.Checked;

			logs_form.LoadConfiguration(xml);

			graphs_summary.Checked = bool.Parse(xml["GraphSummary"].ToString());
		}

		private void SetDefaults()
		{
			timerInterval = 1000;
			scale = 7000;
			avg_checked.Checked = false;

			WLength = 126;
			WHeight = 64;
			full_downlines = new int[WLength];
			full_uplines = new int[WLength];
			full_downspeeds = new double[WLength];
			full_upspeeds = new double[WLength];
			ClientSize = new Size(WLength, WHeight);

			units_kbytes.Checked = true;
			units_kbits.Checked = false;
			graphs_download.Checked = true;
			graphs_upload.Checked = true;

			autoscale_checked.Checked = true;
			this.CenterToScreen(); // Location
			topmost_checked.Checked = true;

			show_checked.Checked = true;
			WindowState = FormWindowState.Normal;
			Show();

			graph_label_checked.Checked = true;
			colorcycle.Checked = false;

			BACKGROUND_COLOR = Color.FromArgb(255, 44, 81, 138);
			FORGROUND_COLOR = Color.FromArgb(255, 255, 255, 255);

			mailcheck.Checked = false;

			MailServer server = new MailServer();
			server.Host = "mail.exampleserver.com";
			server.User = "username";
			server.Pass = Decrypt(Encrypt("password", "username"), "username");
			server.Type = "0";
			server.Enabled = false;
			server.OldMsgCount = 0;
			MailServers.Add(server);

			MailTimer.Interval = 600000;

			trackBar2.Maximum = 100;
			trackBar2.Minimum = 30;
			trackBar2.Value = 100;
			Opacity = ((float)trackBar2.Value) / 100;

			clip_watch.Checked = false;
			LogInterval = 5;
			LogEnabled = false;

			font_large.Checked = false;
			font_medium.Checked = false;
			font_small.Checked = true;
		}

		private void SaveConfiguration()
		{
			string app_dir = Application.ExecutablePath;
			app_dir = app_dir.Remove(app_dir.LastIndexOf('\\'));

			XmlTextWriter writer = new XmlTextWriter(app_dir + "\\config.xml", Encoding.UTF8);
			writer.Formatting = Formatting.Indented;

			writer.WriteStartDocument();
			writer.WriteStartElement("settings");

			writer.WriteElementString("DispAvg", avg_checked.Checked.ToString());
			writer.WriteElementString("UnitsKbytes", units_kbytes.Checked.ToString());
			writer.WriteElementString("UnitsKbits", units_kbits.Checked.ToString());
			writer.WriteElementString("GraphDownload", graphs_download.Checked.ToString());
			writer.WriteElementString("GraphUpload", graphs_upload.Checked.ToString());
			writer.WriteElementString("GraphSummary", graphs_summary.Checked.ToString());
			writer.WriteElementString("AutoScale", autoscale_checked.Checked.ToString());
			writer.WriteElementString("TopMost", topmost_checked.Checked.ToString());
			writer.WriteElementString("SimpleNotifyIcon", simple_icon_checked.Checked.ToString());
			writer.WriteElementString("ShowGraphLabel", graph_label_checked.Checked.ToString());
			writer.WriteElementString("ColorCycle", colorcycle.Checked.ToString());
			writer.WriteElementString("MailCheck", mailcheck.Checked.ToString());
			writer.WriteElementString("ClipWatch", clip_watch.Checked.ToString());
			writer.WriteElementString("LogEnabled", LogEnabled.ToString());

			writer.WriteElementString("TimerInterval", timerInterval.ToString());
			writer.WriteElementString("LogInterval", LogInterval.ToString());

			// TODO: save data regarding mail

			string hstring = null;
			string ustring = null;
			string pstring = null;
			string estring = null;
			string tstring = null;
			foreach (MailServer server in MailServers)
			{
				hstring += server.Host + ",";
				ustring += server.User + ",";
				pstring += Encrypt(server.Pass, server.User) + ",";
				estring += server.Enabled.ToString() + ",";
				tstring += server.Type.ToString() + ",";
			}
			hstring = hstring.Substring(0, hstring.Length - 1);
			ustring = ustring.Substring(0, ustring.Length - 1);
			pstring = pstring.Substring(0, pstring.Length - 1);
			estring = estring.Substring(0, estring.Length - 1);
			tstring = tstring.Substring(0, tstring.Length - 1);

			writer.WriteElementString("PopServer", hstring);
			writer.WriteElementString("PopUser", ustring);
			writer.WriteElementString("PopPass", pstring);
			writer.WriteElementString("PopEnabled", estring);
			writer.WriteElementString("PopType", tstring);

			// TODO: save data regarding mail
			writer.WriteElementString("MailCheckInterval", MailTimer.Interval.ToString());
			writer.WriteElementString("BackgroundRed", ((int)BACKGROUND_COLOR.R).ToString());
			writer.WriteElementString("BackgroundGreen", ((int)BACKGROUND_COLOR.G).ToString());
			writer.WriteElementString("BackgroundBlue", ((int)BACKGROUND_COLOR.B).ToString());
			writer.WriteElementString("ForegroundRed", ((int)FORGROUND_COLOR.R).ToString());
			writer.WriteElementString("ForegroundGreen", ((int)FORGROUND_COLOR.G).ToString());
			writer.WriteElementString("ForegroundBlue", ((int)FORGROUND_COLOR.B).ToString());

            if (this.Visible)
            {

                writer.WriteElementString("WindowX", this.Location.X.ToString());
                writer.WriteElementString("WindowY", this.Location.Y.ToString());
                writer.WriteElementString("DispWidth", ClientSize.Width.ToString());
                writer.WriteElementString("DispHeight", ClientSize.Height.ToString());
            }
            else
            {
                writer.WriteElementString("WindowX", Convert.ToInt32( Screen.PrimaryScreen.WorkingArea.Width/2).ToString());
                writer.WriteElementString("WindowY", Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height / 2).ToString());
                writer.WriteElementString("DispWidth","640"/*ClientSize.Width.ToString()*/);
                writer.WriteElementString("DispHeight","480" /*ClientSize.Height.ToString()*/);
            }

			if (WindowState == FormWindowState.Normal)
				writer.WriteElementString("WindowIsVisible", "True");
			else
				writer.WriteElementString("WindowIsVisible", "False");

			writer.WriteElementString("GraphScale", scale.ToString());
			writer.WriteElementString("Trans", trackBar2.Value.ToString());

			if (font_large.Checked)
				writer.WriteElementString("FontSize", "2");
			else if (font_medium.Checked)
				writer.WriteElementString("FontSize", "1");
			else
				writer.WriteElementString("FontSize", "0");

			logs_form.SaveConfiguration(writer);

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Close();
		}

		//enc denc
		private string Encrypt(string plainMessage, string password)
		{
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.IV = new byte[8];
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[0]);
			des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);
			MemoryStream ms = new MemoryStream(plainMessage.Length * 2);
			CryptoStream encStream = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
			byte[] plainBytes = Encoding.UTF8.GetBytes(plainMessage);
			encStream.Write(plainBytes, 0, plainBytes.Length);
			encStream.FlushFinalBlock();
			byte[] encryptedBytes = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(encryptedBytes, 0, (int)ms.Length);
			encStream.Close();
			ms.Close();
			return Convert.ToBase64String(encryptedBytes);
		}
		private string Decrypt(string encryptedBase64, string password)
		{
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.IV = new byte[8];
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[0]);
			des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);
			byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);
			MemoryStream ms = new MemoryStream(encryptedBase64.Length);
			CryptoStream decStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
			decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
			decStream.FlushFinalBlock();
			byte[] plainBytes = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(plainBytes, 0, (int)ms.Length);
			decStream.Close();
			ms.Close();
			return Encoding.UTF8.GetString(plainBytes);
		}

		//set the text of a control in a thread safe manner
		delegate void SetTextCallback(Label l, string t, int offset);
		delegate void SetColorCallback(Control l, Color c);
		private void SetText(Label l, string t, int offset)
		{
			if (l.InvokeRequired)
			{
				SetTextCallback d = new SetTextCallback(SetText);
				try
				{
					this.Invoke(d, new Object[] { l, t, offset });
				}
				catch (ObjectDisposedException e)
				{
					Console.WriteLine(e.ToString());
				}
			}
			else
			{
				if (ClientSize.Width > 40 && ClientSize.Height > 40)
				{
					WLength = ClientSize.Width;
					WHeight = ClientSize.Height;
				}

				int font_adjust = 0;
				if (!font_small.Checked) font_adjust = 1;
				if (WLength > 125)
				{
					if (font_large.Checked) l.Font = new Font("MS Serif", 7 + font_adjust, FontStyle.Bold);
					else l.Font = new Font("MS Serif", 7 + font_adjust, FontStyle.Regular);
					l.TextAlign = ContentAlignment.TopLeft;
					l.Location = new Point(10 + offset, WHeight - 14);
				}
				else if (WLength > 95)
				{
					if (font_large.Checked) l.Font = new Font("MS Serif", 6 + font_adjust, FontStyle.Bold);
					else l.Font = new Font("MS Serif", 6 + font_adjust, FontStyle.Regular);
					l.TextAlign = ContentAlignment.MiddleLeft;
					l.Location = new Point(10 + offset, WHeight - 14);
				}
				else
				{
					if (font_large.Checked) l.Font = new Font("MS Serif", 5 + font_adjust, FontStyle.Bold);
					else l.Font = new Font("MS Serif", 5 + font_adjust, FontStyle.Regular);
					l.TextAlign = ContentAlignment.MiddleLeft;
					l.Location = new Point(10 + offset, WHeight - 14);
				}
				l.Text = t;
			}
		}
		private void SetColor(Control l, Color c)
		{
			if (l.InvokeRequired)
			{
				SetColorCallback d = new SetColorCallback(SetColor);
				this.Invoke(d, new Object[] { l, c });
			}
			else
				l.BackColor = c;
		}

		[STAThread]
		// MAIN - if already running notify, otherwise run the main form
		static void Main()
		{
			//try
			{
				Process[] RunningProcesses = Process.GetProcessesByName("FreeMeter");
				if (RunningProcesses.Length == 1)
				{
					Application.EnableVisualStyles();
					Application.Run(new Form1());
				}
				else if (RunningProcesses.Length == 2)
				{
					if (RunningProcesses[0].StartTime > RunningProcesses[1].StartTime)
						RunningProcesses[1].Kill();
					else
						RunningProcesses[0].Kill();

					Application.EnableVisualStyles();
					Application.Run(new Form1());
				}
				else
					MessageBox.Show("I'm Already Running!", "!");
				
			}
			/*catch (Exception ex)
			{
				if (!Debugger.IsAttached)
					(new Error(ex)).ShowDialog();
				else
					throw ex;
			}*/		
		}

		private void ShowTotalsLog_Click(Object sender, EventArgs e)
		{
			logs_form.Show(this);
		}
	}

	public class MailServer
	{
		internal bool Enabled;
		internal string Host;
		internal string User;
		internal string Pass;
		internal string Type;
		internal int OldMsgCount;
	}

	// The NetworkMonitor class monitors network speed for each network adapter on the computer,
	// using classes for Performance counter in .NET library.
	public class NetworkMonitor
	{
		private ArrayList adapters;
		public ArrayList monitoredAdapters;

		public NetworkMonitor()
		{
			this.adapters = new ArrayList();
			this.monitoredAdapters = new ArrayList();
			EnumerateNetworkAdapters();
		}

		private void EnumerateNetworkAdapters()
		{
			PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");

			foreach (string name in category.GetInstanceNames())
			{
				if (name == "MS TCP Loopback interface")
					continue;
				NetworkAdapter adapter = new NetworkAdapter(name);
				adapter.dlCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
				adapter.ulCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name);
				this.adapters.Add(adapter);
				adapter.init();
			}
		}

		public NetworkAdapter[] Adapters
		{
			get
			{
				return (NetworkAdapter[])this.adapters.ToArray(typeof(NetworkAdapter));
			}
		}
	}

	// Represents a network adapter installed on the machine.
	// Properties of this class can be used to obtain current network speed.
	public class NetworkAdapter
	{
		// Instances of this class are supposed to be created only in an NetworkMonitor.
		internal NetworkAdapter(string name)
		{
			this.name = name;
		}

		private long dlSpeed, ulSpeed;
		private long dlValue, ulValue;
		private long dlValueOld, ulValueOld;

		internal string name;
		internal PerformanceCounter dlCounter, ulCounter;
		internal bool Enabled;

		internal void init()
		{
			this.dlValueOld = this.dlCounter.NextSample().RawValue;
			this.ulValueOld = this.ulCounter.NextSample().RawValue;
			this.Enabled = true;
		}

		// Obtain new sample from performance counters, and refresh the values saved in dlSpeed, ulSpeed, etc.
		// This method is supposed to be called only in NetworkMonitor, one time every second.
		internal void refresh()
		{
			this.dlValue = this.dlCounter.NextSample().RawValue;
			this.ulValue = this.ulCounter.NextSample().RawValue;

			// Calculates download and upload speed.
			this.dlSpeed = this.dlValue - this.dlValueOld;
			this.ulSpeed = this.ulValue - this.ulValueOld;
			this.dlValueOld = this.dlValue;
			this.ulValueOld = this.ulValue;
		}

		// Overrides method to return the name of the adapter.
		public override string ToString()
		{
			return this.name;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}
		// Current download speed in bytes per second.
		public long DownloadSpeed(int Interval)
		{
			return this.dlSpeed * 1000 / Interval;
		}
		// Current upload speed in bytes per second.
		public long UploadSpeed(int Interval)
		{
			return this.ulSpeed * 1000 / Interval;
		}

	}

	//classes for dealing with pop and imap email
	public class POP3
	{
		string POPServer;
		string user;
		string pwd;
		public string ErrMsg = "";
		NetworkStream ns;
		StreamReader sr;
		TcpClient sender;
		public POP3()
		{
		}
		public POP3(string server, string _user, string _pwd)
		{
			POPServer = server;
			user = _user;
			pwd = _pwd;
		}
		private string Connect()
		{
			try
			{
				sender = new TcpClient(POPServer, 110);
			}
			catch (SocketException e)
			{
				return e.Message;
			}
			Byte[] outbytes;
			string input;
			try
			{
				ns = sender.GetStream();
				sr = new StreamReader(ns);
				sr.ReadLine();

				input = "user " + user + "\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				ns.Write(outbytes, 0, outbytes.Length);
				sr.ReadLine();

				input = "pass " + pwd + "\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				ns.Write(outbytes, 0, outbytes.Length);
				string resp = sr.ReadLine();

				string[] tokens = resp.Split(new Char[] { ' ' });
				if (tokens[0].ToLower() == "-err")
					return "Login Failed";

			}
			catch (InvalidOperationException e)
			{
				return e.Message;
			}
			return null;
		}
		private void Disconnect()
		{
			string input = "quit" + "\r\n";
			Byte[] outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
			ns.Write(outbytes, 0, outbytes.Length);
			sr.Dispose();
			ns.Close();
			sender.Close();
		}
		public int GetNumberOfMessages()
		{
			Byte[] outbytes;
			string input;
			try
			{
				string msg = Connect();
				if (msg != null)
				{
					this.ErrMsg = msg;
					return -1;
				}
				input = "stat" + "\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				if (ns == null)
				{
					this.ErrMsg = "NetworkStream not connected";
					return -1;
				}
				ns.Write(outbytes, 0, outbytes.Length);
				string resp = sr.ReadLine();
				string[] tokens = resp.Split(new Char[] { ' ' });
				Disconnect();
				if (tokens[0].ToLower() == "-err")
				{
					this.ErrMsg = "Invalid command";
					return -1;
				}
				else
					return Convert.ToInt32(tokens[1]);
			}
			catch (InvalidOperationException e)
			{
				this.ErrMsg = e.Message;
				return -1;
			}
		}
	}

	public class IMAP
	{
		string IMAPServer;
		string user;
		string pwd;
		public string ErrMsg = "";
		NetworkStream ns;
		StreamReader sr;
		TcpClient sender;
		public IMAP()
		{
		}
		public IMAP(string server, string _user, string _pwd)
		{
			IMAPServer = server;
			user = _user;
			pwd = _pwd;
		}
		private string Connect()
		{
			try
			{
				sender = new TcpClient(IMAPServer, 143);
			}
			catch (SocketException e)
			{
				return e.Message;
			}
			Byte[] outbytes;
			string input;
			try
			{
				ns = sender.GetStream();
				sr = new StreamReader(ns);
				sr.ReadLine();

				input = "a001 login " + user + " " + pwd + "\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				ns.Write(outbytes, 0, outbytes.Length);
				string resp = sr.ReadLine();
				string[] tokens = resp.Split(new Char[] { ' ' });
				if (tokens[1].ToLower() == "no")
					return "Login Failed";
			}
			catch (InvalidOperationException e)
			{
				return e.Message;
			}
			catch (IOException eio)
			{
				return eio.Message;
			}
			return null;
		}
		private void Disconnect()
		{
			string input = "a003 logout" + "\r\n";
			Byte[] outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
			ns.Write(outbytes, 0, outbytes.Length);
			sr.Dispose();
			ns.Close();
			sender.Close();
		}
		public int GetNumberOfMessages()
		{
			Byte[] outbytes;
			string input;
			try
			{
				string msg = Connect();
				if (msg != null)
				{
					this.ErrMsg = msg;
					return -1;
				}
				input = "a002 select inbox" + "\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				if (ns == null)
				{
					this.ErrMsg = "NetworkStream not connected";
					return -1;
				}
				ns.Write(outbytes, 0, outbytes.Length);
				bool found = false;
				string[] tokens = null;
				while (!found)
				{
					string resp = sr.ReadLine();
					tokens = resp.Split(new Char[] { ' ' });
					if (tokens[1].ToLower() == "no")
					{
						this.ErrMsg = "Invalid command";
						return -1;
					}
					else if (tokens[2].ToLower() == "exists")
						found = true;
				}
				Disconnect();
				return Convert.ToInt32(tokens[1]);
			}
			catch (InvalidOperationException e)
			{
				this.ErrMsg = e.Message;
				return -1;
			}
			catch (IOException e)
			{
				this.ErrMsg = e.Message;
				return -1;
			}
		}
	}

	public class EmailSettings_Form : System.Windows.Forms.Form
	{
		public Form1 MyParentForm;
		private ComboBox comboBox1;
		private ComboBox comboBox2;
		private TextBox textBox1;
		private TextBox textBox2;
		private TextBox textBox3;
		private NumericUpDown numericUpDown1;
		private CheckBox checkBox1;
		private Label label1;
		private Label labelt;
		private Label label2;
		private Label label3;
		private Label label4;
		private Label label5;
		private Label label6;
		private Button button1;
		private Button button2;
		private Button button3;
		private Button button4;
		private GroupBox groupBox1;
		public string Server
		{
			get { return comboBox1.SelectedItem.ToString(); }
		}
		public string User
		{
			get { return textBox2.Text; }
		}
		public string Pass
		{
			get { return textBox3.Text; }
		}
		public int Time
		{
			get { return (int)numericUpDown1.Value; }
		}

		public EmailSettings_Form()
		{
			comboBox1 = new ComboBox();
			comboBox2 = new ComboBox();
			textBox1 = new TextBox();
			textBox2 = new TextBox();
			textBox3 = new TextBox();
			numericUpDown1 = new NumericUpDown();
			checkBox1 = new CheckBox();
			label1 = new Label();
			labelt = new Label();
			label2 = new Label();
			label3 = new Label();
			label4 = new Label();
			label5 = new Label();
			label6 = new Label();
			button1 = new Button();
			button2 = new Button();
			button3 = new Button();
			button4 = new Button();
			groupBox1 = new GroupBox();

			// comboBox1
			comboBox1.FormattingEnabled = true;
			comboBox1.Location = new Point(103, 9);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new Size(153, 20);
			comboBox1.TabIndex = 0;
			comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
			comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

			// comboBox2
			comboBox2.FormattingEnabled = true;
			comboBox2.Location = new Point(103, 33);
			comboBox2.Name = "comboBox2";
			comboBox2.Size = new Size(60, 20);
			comboBox2.TabIndex = 1;
			comboBox2.SelectedIndexChanged += new EventHandler(comboBox2_SelectedIndexChanged);
			comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBox2.Items.Add("POP3");
			comboBox2.Items.Add("IMAP");

			// textBox1
			textBox1.Location = new Point(103, 57);
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(133, 20);
			textBox1.TabIndex = 2;
			textBox1.TextChanged += new EventHandler(textBox1_TextChanged);

			// textBox2
			textBox2.Location = new Point(103, 81);
			textBox2.Name = "textBox2";
			textBox2.Size = new Size(100, 20);
			textBox2.TabIndex = 3;
			textBox2.TextChanged += new EventHandler(textBox2_TextChanged);

			// textBox3
			textBox3.Location = new Point(103, 105);
			textBox3.Name = "textBox3";
			textBox3.Size = new Size(100, 20);
			textBox3.TabIndex = 4;
			textBox3.UseSystemPasswordChar = true;
			textBox3.TextChanged += new EventHandler(textBox3_TextChanged);

			// checkbox1
			checkBox1.Location = new Point(103, 127);
			checkBox1.Name = "checkbox1";
			checkBox1.TabIndex = 5;
			checkBox1.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);

			// numericUpDown1
			numericUpDown1.Location = new Point(145, 168);
			numericUpDown1.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
			numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDown1.Name = "numericUpDown1";
			numericUpDown1.Size = new Size(40, 20);
			numericUpDown1.TabIndex = 6;
			numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });

			// label1
			label1.Location = new Point(3, 12);
			label1.Name = "label1";
			label1.Font = new Font("Tahoma", 8);
			label1.Size = new Size(95, 13);
			label1.Text = "Display Name";
			label1.TextAlign = ContentAlignment.MiddleRight;
			label1.BackColor = Color.White;

			// labelt
			labelt.Location = new Point(3, 36);
			labelt.Name = "labelt";
			labelt.Font = new Font("Tahoma", 8);
			labelt.Size = new Size(95, 13);
			labelt.Text = "Server Type";
			labelt.TextAlign = ContentAlignment.MiddleRight;
			labelt.BackColor = Color.White;

			// label2
			label2.Location = new Point(3, 60);
			label2.Name = "label2";
			label2.Font = new Font("Tahoma", 8);
			label2.Size = new Size(95, 13);
			label2.Text = "Server Host";
			label2.TextAlign = ContentAlignment.MiddleRight;
			label2.BackColor = Color.White;

			// label3
			label3.Location = new Point(3, 84);
			label3.Name = "label3";
			label3.Font = new Font("Tahoma", 8);
			label3.Size = new Size(95, 13);
			label3.Text = "Username";
			label3.TextAlign = ContentAlignment.MiddleRight;
			label3.BackColor = Color.White;

			// label4
			label4.Location = new Point(3, 108);
			label4.Name = "label4";
			label4.Font = new Font("Tahoma", 8);
			label4.Size = new Size(95, 13);
			label4.Text = "Password";
			label4.TextAlign = ContentAlignment.MiddleRight;
			label4.BackColor = Color.White;

			// label5
			label5.Location = new Point(3, 132);
			label5.Name = "label5";
			label5.Font = new Font("Tahoma", 8);
			label5.Size = new Size(95, 13);
			label5.Text = "Enabled";
			label5.TextAlign = ContentAlignment.MiddleRight;
			label5.BackColor = Color.White;

			// label6
			label6.Location = new Point(17, 171);
			label6.Name = "label6";
			label6.Font = new Font("Tahoma", 8);
			label6.Size = new Size(125, 13);
			label6.Text = "Check Email Every (min)";
			label6.TextAlign = ContentAlignment.MiddleRight;

			// add button (button4)
			button4.Location = new Point(20, 196);
			button4.Name = "button3";
			button4.Size = new Size(60, 17);
			button4.TabIndex = 7;
			button4.Text = "Add New";
			button4.Click += new EventHandler(button4_Click);

			// button1
			button1.DialogResult = DialogResult.OK;
			button1.Location = new Point(85, 196);
			button1.Name = "button1";
			button1.Size = new Size(55, 17);
			button1.TabIndex = 8;
			button1.Text = "Save";

			// button2
			button2.DialogResult = DialogResult.Cancel;
			button2.Location = new Point(145, 196);
			button2.Name = "button2";
			button2.Size = new Size(55, 17);
			button2.TabIndex = 9;
			button2.Text = "Cancel";

			// button3
			button3.Location = new Point(205, 196);
			button3.Name = "button3";
			button3.Size = new Size(55, 17);
			button3.TabIndex = 10;
			button3.Text = "Delete";
			button3.Click += new EventHandler(button3_Click);

			// groupBox1
			groupBox1.Location = new Point(10, 6);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(260, 154);
			groupBox1.TabStop = false;
			groupBox1.Text = "";
			groupBox1.SendToBack();

			// EmailSettings_Form
			CancelButton = button2;
			ClientSize = new Size(280, 224);
			ControlBox = false;
			groupBox1.Controls.AddRange(new Control[] { label1, labelt, label2, label3, label4, label5, comboBox1, comboBox2, textBox1, textBox2, textBox3, checkBox1 });
			Controls.AddRange(new Control[] { groupBox1, label6, numericUpDown1, button4, button1, button2, button3 });
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "EmailSettings_Form";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Email Server Settings";
			Load += new System.EventHandler(EmailSettings_Form_Load);
		}

		//comboBox fuctionality and events to synch data to main form
		public ArrayList har = new ArrayList();
		public ArrayList uar = new ArrayList();
		public ArrayList par = new ArrayList();
		public ArrayList ear = new ArrayList();
		public ArrayList tar = new ArrayList();
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex > -1)
			{
				textBox1.Text = har[comboBox1.SelectedIndex].ToString();
				textBox2.Text = uar[comboBox1.SelectedIndex].ToString();
				textBox3.Text = par[comboBox1.SelectedIndex].ToString();
				if (tar[comboBox1.SelectedIndex].ToString() == "1")
					comboBox2.SelectedIndex = 1;
				else
					comboBox2.SelectedIndex = 0;
				checkBox1.Checked = (bool)ear[comboBox1.SelectedIndex];
			}
			else
			{
				textBox1.Clear();
				textBox2.Clear();
				textBox3.Clear();
			}
		}
		void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			tar[comboBox1.SelectedIndex] = comboBox2.SelectedIndex;
		}
		void textBox1_TextChanged(object sender, EventArgs e)
		{
			har[comboBox1.SelectedIndex] = textBox1.Text;
			comboBox1.Items[comboBox1.SelectedIndex] = textBox1.Text;
		}
		void textBox2_TextChanged(object sender, EventArgs e)
		{
			uar[comboBox1.SelectedIndex] = textBox2.Text;
		}
		void textBox3_TextChanged(object sender, EventArgs e)
		{
			par[comboBox1.SelectedIndex] = textBox3.Text;
		}
		void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			ear[comboBox1.SelectedIndex] = checkBox1.Checked;
		}
		//delete mail server
		void button3_Click(object sender, EventArgs e)
		{
			if (comboBox1.Items.Count > 1)
			{
				har.RemoveAt(comboBox1.SelectedIndex);
				uar.RemoveAt(comboBox1.SelectedIndex);
				par.RemoveAt(comboBox1.SelectedIndex);
				ear.RemoveAt(comboBox1.SelectedIndex);
				tar.RemoveAt(comboBox1.SelectedIndex);
				comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
				comboBox1.SelectedIndex = 0;
			}
			else
			{
				MessageBox.Show("Zero is not enough for me!\n\nIf you don't want to use me, then just\nuncheck Email Notify in the main menu.", "Cannot remove only entry.");
			}
		}
		//add mail server
		void button4_Click(object sender, EventArgs e)
		{
			if (comboBox1.Items.Count < 5)
			{
				har.Add("mail.exampleserver.com");
				uar.Add("username");
				par.Add("password");
				ear.Add(true);
				tar.Add(0);
				this.comboBox1.Items.Add("mail.exampleserver.com");
				this.comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
				this.comboBox2.SelectedIndex = 0;
			}
			else
			{
				MessageBox.Show("Five is enough for me!\n\nI can't display more in a balloon tip.", "Reached Maximum Entries.");
			}
		}

		private void EmailSettings_Form_Load(object sender, System.EventArgs e)
		{
			foreach (MailServer s in ((Form1)MyParentForm).MailServers)
			{
				har.Add(s.Host);
				uar.Add(s.User);
				par.Add(s.Pass);
				ear.Add(s.Enabled);
				tar.Add(s.Type);
				this.comboBox1.Items.Add(s.Host);
			}
			this.comboBox1.SelectedIndex = 0;
			this.numericUpDown1.Value = new decimal(new int[] { (((Form1)MyParentForm).MailTimer.Interval / 60 / 1000), 0, 0, 0 });
		}
	}

	//Just a simple about form to be called like AboutForm.ShowAboutForm(this);
	public class AboutForm : System.Windows.Forms.Form
	{
		private PictureBox IconBox1;
		private PictureBox IconBox2;
		private PictureBox IconBox3;
		private Label TextArea;
		private Label Legend;
		private Label dl;
		private Label ul;
		private Label du;
		private LinkLabel link;
		private Button OKButton;
		private Button LButton;
		private AboutForm()
		{
			IconBox1 = new PictureBox();
			IconBox2 = new PictureBox();
			IconBox3 = new PictureBox();
			TextArea = new Label();
			Legend = new Label();
			dl = new Label();
			ul = new Label();
			du = new Label();
			link = new LinkLabel();
			OKButton = new Button();
			LButton = new Button();
			// IconBoxes
			IconBox1.Location = new Point(12, 25);
			IconBox1.Name = "IconBox1";
			IconBox1.Size = new Size(16, 16);
			IconBox2.Location = new Point(12, 43);
			IconBox2.Name = "IconBox2";
			IconBox2.Size = new Size(16, 16);
			IconBox3.Location = new Point(12, 61);
			IconBox3.Name = "IconBox3";
			IconBox3.Size = new Size(16, 16);
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
			link.Location = new Point(50, 102);
			link.Name = "link";
			link.Size = new Size(210, 13);
			link.LinkBehavior = LinkBehavior.HoverUnderline;
			link.LinkColor = Color.Navy;
			link.Font = new Font("Tahoma", 8);
			link.Text = "http://freemeter.sourceforge.net/";
			link.LinkClicked += new LinkLabelLinkClickedEventHandler(link_Clicked);
			// TextArea
			TextArea.Location = new Point(96, 10);
			TextArea.Name = "TextArea";
			TextArea.Size = new Size(208, 86);
			TextArea.Text = "label1";
			TextArea.TextAlign = ContentAlignment.MiddleLeft;
			TextArea.Font = new Font("Tahoma", 8);
			// OKButton
			OKButton.Location = new Point(96, 120);
			OKButton.Size = new Size(55, 17);
			OKButton.Name = "OKButton";
			OKButton.TabIndex = 0;
			OKButton.Text = "OK";
			OKButton.Click += new EventHandler(OKButton_Click);
			//License Button
			LButton.Location = new Point(155, 120);
			LButton.Size = new Size(55, 17);
			LButton.Name = "LButton";
			LButton.TabIndex = 1;
			LButton.Text = "License";
			LButton.Click += new EventHandler(LButton_Click);
			// AboutForm
			StartPosition = FormStartPosition.CenterScreen;
			AcceptButton = OKButton;
			ClientSize = new Size(280, 150);
			Controls.AddRange(new Control[] { OKButton, LButton, TextArea, Legend, dl, ul, du, link, IconBox1, IconBox2, IconBox3 });
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			ShowInTaskbar = false;
			ShowIcon = false;
			ControlBox = false;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "AboutForm";
			Text = "About: ";
			Load += new EventHandler(AboutForm_Load);
		}
		private void AboutForm_Load(object sender, System.EventArgs e)
		{
			Assembly ThisAssembly = Assembly.GetExecutingAssembly();
			AssemblyName ThisAssemblyName = ThisAssembly.GetName();
			this.Icon = Owner.Icon;
			IconBox1.Image = Owner.Icon.ToBitmap();
			Stream s = ThisAssembly.GetManifestResourceStream("FreeMeter.lr.ico");
			Icon lr = new Icon(s);
			s = ThisAssembly.GetManifestResourceStream("FreeMeter.ly.ico");
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
			this.Text = "About " + Title;
			TextArea.Text = Title + " v" + FriendlyVersion + "\n\n" + Copyright;
		}
		private void OKButton_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void LButton_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Copyright © 2005-2007, David Schultz, Mieszko Lassota All rights reserved.\n\n" +
			  "Check http://freemeter.sourceforge.net/ for latest version and contact info.\n\n" +
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
			  "possibility\nof such damage.", "FreeMeter GPL License"
			);
		}
		private void link_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://freemeter.sourceforge.net/");
		}
		internal static void ShowAboutForm(IWin32Window Owner)
		{
			AboutForm form = new AboutForm();
			form.ShowDialog(Owner);
		}
	}

	public class AdvPing : Form
	{
		public Form1 MyParentForm;
		private static TextBox hostbox, databox, databox2, databox3, results;
		private static CheckBox df;
		private static Thread pinger;
		private Button sendit = new Button();
		private Button stopit = new Button();
		private int sentcount = 0, recvcount = 0;
		private ArrayList times = new ArrayList();
		private string PingTarget;
		private bool pinging = false;

		public AdvPing()
		{
			Text = "FreeMeter Ping Utility";
			Size = new Size(400, 284);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			MinimizeBox = true;
			MaximizeBox = false;
			ControlBox = true;
			ShowIcon = false;
			Load += new EventHandler(AdvPing_Load);

			Label label1 = new Label();
			label1.Parent = this;
			label1.Text = "Host:";
			label1.Font = new Font("Tahoma", 8);
			label1.Size = new Size(30, 13);
			label1.Location = new Point(3, 5);

			hostbox = new TextBox();
			hostbox.Parent = this;
			hostbox.Size = new Size(227, 13);
			hostbox.Location = new Point(35, 3);
			hostbox.TabIndex = 0;

			results = new TextBox();
			results.Parent = this;
			results.Multiline = true;
			results.TabIndex = 8;
			results.Size = new Size(388, 213);
			results.Location = new Point(3, 25);
			results.BorderStyle = BorderStyle.FixedSingle;
			results.BackColor = Color.Black;
			results.ForeColor = Color.Silver;
			results.Font = new Font("Tahoma", 8);
			results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			results.WordWrap = false;

			Label label2 = new Label();
			label2.Parent = this;
			label2.Text = "Size(bytes)";
			label2.Font = new Font("Tahoma", 8);
			label2.Size = new Size(58, 13);
			label2.Location = new Point(3, 242);

			databox = new TextBox();
			databox.Parent = this;
			databox.Text = "32";
			databox.TabIndex = 1;
			databox.Location = new Point(64, 241);
			databox.MaximumSize = new Size(38, 16);

			Label label3 = new Label();
			label3.Parent = this;
			label3.Text = "Interval(ms)";
			label3.Font = new Font("Tahoma", 8);
			label3.Size = new Size(64, 13);
			label3.Location = new Point(105, 242);

			databox2 = new TextBox();
			databox2.Parent = this;
			databox2.Text = "1000";
			databox2.TabIndex = 2;
			databox2.Location = new Point(171, 241);
			databox2.MaximumSize = new Size(38, 16);

			Label label4 = new Label();
			label4.Parent = this;
			label4.Text = "Timeout(ms)";
			label4.Font = new Font("Tahoma", 8);
			label4.Size = new Size(66, 13);
			label4.Location = new Point(211, 242);

			databox3 = new TextBox();
			databox3.Parent = this;
			databox3.Text = "3000";
			databox3.TabIndex = 3;
			databox3.Location = new Point(277, 241);
			databox3.MaximumSize = new Size(38, 16);

			Label label5 = new Label();
			label5.Parent = this;
			label5.Text = "DF";
			label5.Font = new Font("Tahoma", 8);
			label5.Size = new Size(17, 13);
			label5.Location = new Point(318, 242);

			df = new CheckBox();
			df.Parent = this;
			df.TabIndex = 4;
			df.Location = new Point(335, 242);
			df.Size = new Size(16, 16);

			sendit.Parent = this;
			sendit.Size = new Size(40, 20);
			sendit.Text = "Start";
			sendit.TabIndex = 5;
			sendit.Location = new Point(265, 3);
			sendit.Click += new EventHandler(ButtonSendOnClick);

			stopit.Parent = this;
			stopit.Size = new Size(40, 20);
			stopit.Text = "Stop";
			stopit.TabIndex = 6;
			stopit.Location = new Point(308, 3);
			stopit.Click += new EventHandler(ButtonStopOnClick);
			stopit.Enabled = false;

			Button clearit = new Button();
			clearit.Parent = this;
			clearit.Size = new Size(40, 20);
			clearit.Text = "Clear";
			clearit.TabIndex = 7;
			clearit.Location = new Point(351, 3);
			clearit.Click += new EventHandler(ButtonClearOnClick);

			Button closeit = new Button();
			closeit.Parent = this;
			closeit.Size = new Size(42, 20);
			closeit.TabIndex = 9;
			closeit.Text = "Close";
			closeit.Location = new Point(351, 239);
			closeit.Click += new EventHandler(ButtonCloseOnClick);
		}

		void AdvPing_Load(object sender, EventArgs e)
		{
			try
			{
				if (MyParentForm.ClipData.Length < 64)
					hostbox.Text = MyParentForm.ClipData;
			}
			catch { }
		}

		void ButtonSendOnClick(object obj, EventArgs ea)
		{
			sendit.Enabled = false;
			stopit.Enabled = true;
			pinging = true;
			if (hostbox.Text.Trim() == "")
				hostbox.Text = Dns.GetHostName();
			pinger = new Thread(new ThreadStart(sendPing));
			pinger.IsBackground = false;
			pinger.Start();
		}
		void ButtonStopOnClick(object obj, EventArgs ea)
		{
			sendit.Enabled = true;
			stopit.Enabled = false;
			pinging = false;
			try
			{
				pinger.Abort();
			}
			catch (NullReferenceException e)
			{
				results.AppendText("\r\n" + e.Message);
			}
			results.AppendText("\r\n");
			if (sentcount > 0)
			{
				results.AppendText("\r\nPing statistics for " + PingTarget + ":\r\n");
				results.AppendText("   Sent: " + sentcount + "    Received: " + recvcount + "    Lost: " + (sentcount - recvcount) + " (" + (100 - (double)recvcount / (double)sentcount * 100).ToString("F1") + "% loss)\r\n");
				results.AppendText("Approximate round trip times in milli-seconds:\r\n");
				results.AppendText("   Minimum = " + Min(times) + "ms    Maximum = " + Max(times) + "ms    Average = " + Avg(times).ToString("F1") + "ms");
			}
			sentcount = recvcount = 0;
			times = new ArrayList();
		}
		void ButtonClearOnClick(object obj, EventArgs ea)
		{
			results.Text = "";
		}
		void ButtonCloseOnClick(object obj, EventArgs ea)
		{
			try
			{
				pinger.Abort();
			}
			catch (NullReferenceException e)
			{
				results.AppendText("\r\n" + e.Message);
			}
			Close();
		}

		void sendPing()
		{
			Ping pingSender = new Ping();
			PingOptions options = new PingOptions();
			if (df.Checked)
				options.DontFragment = true;
			IPAddress[] IPs = null;
			try
			{
				IPs = Dns.GetHostEntry(hostbox.Text.Trim()).AddressList;
			}
			catch (SocketException e)
			{
				Update_Button(sendit, true);
				Update_Button(stopit, false);
				SetText(results, e.Message);
				return;
			}

			int timeout = 0, interval = 0, datasize = 0;
			try
			{
				timeout = Convert.ToInt32(databox3.Text.Trim());
				interval = Convert.ToInt32(databox2.Text.Trim());
				datasize = Convert.ToInt32(databox.Text.Trim());
			}
			catch (FormatException e)
			{
				Update_Button(sendit, true);
				Update_Button(stopit, false);
				SetText(results, e.Message);
				return;
			}

			PingTarget = null;
			for (int j = 0; j < IPs.Length; j++)
				PingTarget += IPs[j].ToString() + " ";
			int i = 1;
			StringBuilder data = new StringBuilder();
			for (int j = 0; j < datasize; j++)
				data.Append("#");
			byte[] buffer = Encoding.ASCII.GetBytes(data.ToString());

			SetText(results, "-----------------------------------\r\nPinging " + hostbox.Text.Trim() + " [" + IPs[0] + "] with " + data.Length + " bytes of data");

			while (pinging)
			{
				PingReply reply = null;

				try
				{
					reply = pingSender.Send(hostbox.Text.Trim(), timeout, buffer, options);
					sentcount++;
				}
				catch (PingException e)
				{
					Update_Button(sendit, true);
					Update_Button(stopit, false);
					Console.WriteLine(e.ToString());
					return;
				}
				catch (ArgumentException e)
				{
					Update_Button(sendit, true);
					Update_Button(stopit, false);
					SetText(results, e.Message);
					Console.WriteLine(e.ToString());
					return;
				}
				if (reply.Status == IPStatus.Success)
				{
					recvcount++;
					times.Add(reply.RoundtripTime);
					SetText(results, "  " + reply.Buffer.Length + " bytes from: " + reply.Address.ToString() + ", seq: " + i + ", time = " + reply.RoundtripTime + "ms, ttl: " + reply.Options.Ttl + ((reply.Options.DontFragment) ? " DF" : ""));
				}
				else
					SetText(results, "  " + reply.Status.ToString());
				i++;

				int time_ms = (int)(interval - reply.RoundtripTime);
				if (time_ms > 0)
					Thread.Sleep(time_ms);
			}
			Update_Button(sendit, true);
			Update_Button(stopit, false);
			pingSender.Dispose();
		}

		//set the shit in a control in a thread safe manner
		delegate void SetTextCallback(TextBox l, string t);
		private void SetText(TextBox l, string t)
		{
			if (!l.IsDisposed)
			{
				if (l.InvokeRequired)
				{
					SetTextCallback d = new SetTextCallback(SetText);
					this.Invoke(d, new Object[] { l, t });
				}
				else
				{
					l.AppendText("\r\n" + t);
				}
			}
		}
		delegate void SetButtonCallback(Button l, bool t);
		private void Update_Button(Button l, bool t)
		{
			if (l.InvokeRequired)
			{
				SetButtonCallback d = new SetButtonCallback(Update_Button);
				this.Invoke(d, new Object[] { l, t });
			}
			else
			{
				l.Enabled = t;
			}
		}
		private static double Avg(ArrayList num)
		{
			double sum = 0.0;
			for (int i = 0; i < num.Count; i++)
				sum = sum + Convert.ToDouble(num[i]);
			return sum / Convert.ToDouble(num.Count);
		}
		private static double Max(ArrayList A)
		{
			if (A.Count > 0)
			{
				double maxVal = Convert.ToDouble(A[0]);
				for (int i = 0; i < A.Count; i++)
					if (Convert.ToDouble(A[i]) > maxVal)
						maxVal = Convert.ToDouble(A[i]);
				return maxVal;
			}
			else return 0;
		}
		private static double Min(ArrayList A)
		{
			if (A.Count > 0)
			{
				double minVal = Convert.ToDouble(A[0]);
				for (int i = 0; i < A.Count; i++)
					if (Convert.ToDouble(A[i]) < minVal)
						minVal = Convert.ToDouble(A[i]);
				return minVal;
			}
			else return 0;
		}

	}

	public class AdvTrace : Form
	{
		public Form1 MyParentForm;
		private static TextBox hostbox, databox, databox2, results;
		private static CheckBox databox3;
		private static Thread tracer;
		private Button sendit = new Button();
		private Button stopit = new Button();
		//private string TraceTarget;
		private bool traceing = false;

		public AdvTrace()
		{
			Text = "FreeMeter Traceroute Utility";
			Size = new Size(400, 284);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			MinimizeBox = true;
			MaximizeBox = false;
			ControlBox = true;
			ShowIcon = false;
			Load += new EventHandler(AdvTrace_Load);

			Label label1 = new Label();
			label1.Parent = this;
			label1.Text = "Host:";
			label1.Font = new Font("Tahoma", 8);
			label1.Size = new Size(30, 13);
			label1.Location = new Point(3, 5);

			hostbox = new TextBox();
			hostbox.Parent = this;
			hostbox.Size = new Size(227, 13);
			hostbox.Location = new Point(35, 3);
			hostbox.TabIndex = 0;

			results = new TextBox();
			results.Parent = this;
			results.Multiline = true;
			results.TabIndex = 7;
			results.Size = new Size(388, 213);
			results.Location = new Point(3, 25);
			results.BorderStyle = BorderStyle.FixedSingle;
			results.BackColor = Color.Black;
			results.ForeColor = Color.Silver;
			results.Font = new Font("Tahoma", 8);
			results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			results.WordWrap = false;

			Label label2 = new Label();
			label2.Parent = this;
			label2.Text = "Max hops";
			label2.Font = new Font("Tahoma", 8);
			label2.Size = new Size(50, 13);
			label2.Location = new Point(3, 242);

			databox = new TextBox();
			databox.Parent = this;
			databox.Text = "30";
			databox.TabIndex = 1;
			databox.Location = new Point(56, 241);
			databox.MaximumSize = new Size(30, 16);

			Label label3 = new Label();
			label3.Parent = this;
			label3.Text = "Timeout(ms)";
			label3.Font = new Font("Tahoma", 8);
			label3.Size = new Size(66, 13);
			label3.Location = new Point(92, 242);

			databox2 = new TextBox();
			databox2.Parent = this;
			databox2.Text = "3000";
			databox2.TabIndex = 2;
			databox2.Location = new Point(158, 241);
			databox2.MaximumSize = new Size(38, 16);

			Label label4 = new Label();
			label4.Parent = this;
			label4.Text = "Resolve names";
			label4.Font = new Font("Tahoma", 8);
			label4.Size = new Size(77, 13);
			label4.Location = new Point(202, 242);

			databox3 = new CheckBox();
			databox3.Parent = this;
			databox3.TabIndex = 3;
			databox3.Location = new Point(280, 242);
			databox3.Size = new Size(16, 16);

			sendit.Parent = this;
			sendit.Size = new Size(40, 20);
			sendit.Text = "Start";
			sendit.TabIndex = 4;
			sendit.Location = new Point(265, 3);
			sendit.Click += new EventHandler(ButtonSendOnClick);

			stopit.Parent = this;
			stopit.Size = new Size(40, 20);
			stopit.Text = "Stop";
			stopit.TabIndex = 5;
			stopit.Location = new Point(308, 3);
			stopit.Click += new EventHandler(ButtonStopOnClick);
			stopit.Enabled = false;

			Button clearit = new Button();
			clearit.Parent = this;
			clearit.Size = new Size(40, 20);
			clearit.Text = "Clear";
			clearit.TabIndex = 6;
			clearit.Location = new Point(351, 3);
			clearit.Click += new EventHandler(ButtonClearOnClick);

			Button closeit = new Button();
			closeit.Parent = this;
			closeit.Size = new Size(42, 20);
			closeit.TabIndex = 8;
			closeit.Text = "Close";
			closeit.Location = new Point(351, 239);
			closeit.Click += new EventHandler(ButtonCloseOnClick);
		}

		void AdvTrace_Load(object sender, EventArgs e)
		{
			try
			{
				if (MyParentForm.ClipData.Length < 64)
					hostbox.Text = MyParentForm.ClipData;
			}
			catch { }
		}

		void ButtonSendOnClick(object obj, EventArgs ea)
		{
			sendit.Enabled = false;
			stopit.Enabled = true;
			traceing = true;
			if (hostbox.Text.Trim() == "")
				hostbox.Text = Dns.GetHostName();
			tracer = new Thread(new ThreadStart(sendTrace));
			tracer.IsBackground = false;
			tracer.Start();
		}
		void ButtonStopOnClick(object obj, EventArgs ea)
		{
			sendit.Enabled = true;
			stopit.Enabled = false;
			traceing = false;
			try
			{
				tracer.Abort();
			}
			catch (NullReferenceException e)
			{
				results.AppendText("\r\n" + e.Message);
			}
		}
		void ButtonClearOnClick(object obj, EventArgs ea)
		{
			results.Text = "";
		}
		void ButtonCloseOnClick(object obj, EventArgs ea)
		{
			try
			{
				tracer.Abort();
			}
			catch (NullReferenceException e)
			{
				results.AppendText("\r\n" + e.Message);
			}
			Close();
		}
		//set the shit in a control in a thread safe manner
		delegate void SetTextCallback(TextBox l, string t);
		private void SetText(TextBox l, string t)
		{
			if (!l.IsDisposed)
			{
				if (l.InvokeRequired)
				{
					SetTextCallback d = new SetTextCallback(SetText);
					this.Invoke(d, new Object[] { l, t });
				}
				else
				{
					l.AppendText(t);
				}
			}
		}
		delegate void SetButtonCallback(Button l, bool t);
		private void Update_Button(Button l, bool t)
		{
			if (l.InvokeRequired)
			{
				SetButtonCallback d = new SetButtonCallback(Update_Button);
				this.Invoke(d, new Object[] { l, t });
			}
			else
			{
				l.Enabled = t;
			}
		}

		void sendTrace()
		{
			Ping pingSender = new Ping();
			PingOptions options = new PingOptions();
			int ttl = 1;
			options.DontFragment = true;
			IPAddress[] IPs = null;
			try
			{
				IPs = Dns.GetHostEntry(hostbox.Text.Trim()).AddressList;
			}
			catch (SocketException e)
			{
				Update_Button(sendit, true);
				Update_Button(stopit, false);
				SetText(results, e.Message + "\r\n");
				return;
			}

			int hops = 0, timeout = 0;
			try
			{
				hops = Convert.ToInt32(databox.Text.Trim());
				timeout = Convert.ToInt32(databox2.Text.Trim());
			}
			catch (FormatException e)
			{
				Update_Button(sendit, true);
				Update_Button(stopit, false);
				SetText(results, e.Message + "\r\n");
				return;
			}

			SetText(results, "-----------------------------------\r\nTracing route to " + hostbox.Text + " [" + IPs[0] + "]\r\n");

			while (traceing && ttl <= hops)
			{
				PingReply reply = null;
				options.Ttl = ttl;
				try
				{
					reply = pingSender.Send(hostbox.Text.Trim(), timeout, new byte[0], options);
				}
				catch (PingException e)
				{
					Update_Button(sendit, true);
					Update_Button(stopit, false);
					//SetText(results, "  Interrupted\r\n");
					Console.WriteLine(e.Message);
					return;
				}

				if (reply.Status == IPStatus.Success)
					traceing = false;

				SetText(results, "  " + ttl.ToString());

				try
				{
					PingReply timing = pingSender.Send(reply.Address, timeout, new byte[32], new PingOptions(128, true));
					SetText(results, (timing.Status == IPStatus.Success) ? "\t" + timing.RoundtripTime.ToString() + "ms" : "\t*");
					timing = pingSender.Send(reply.Address, timeout, new byte[32], new PingOptions(128, true));
					SetText(results, (timing.Status == IPStatus.Success) ? "\t" + timing.RoundtripTime.ToString() + "ms" : "\t*");
					timing = pingSender.Send(reply.Address, timeout, new byte[32], new PingOptions(128, true));
					SetText(results, (timing.Status == IPStatus.Success) ? "\t" + timing.RoundtripTime.ToString() + "ms" : "\t*");
				}
				catch (PingException e)
				{
					Update_Button(sendit, true);
					Update_Button(stopit, false);
					SetText(results, " Interrupted\r\n");
					Console.WriteLine(e.Message);
					return;
				}
				catch (ArgumentNullException e)
				{
					//SetText(results, "\t\t");
					Console.WriteLine(e.Message);
				}

				string hostName = null;
				if (databox3.Checked)
				{
					try
					{
						IPHostEntry hostEntry = Dns.GetHostEntry(reply.Address);
						if (hostEntry.HostName != null && hostEntry.HostName != string.Empty)
							hostName = hostEntry.HostName;
					}
					catch (ArgumentNullException e)
					{
						hostName = reply.Status.ToString();
						Console.WriteLine(e.Message);
					}
					catch (SocketException e)
					{
						hostName = reply.Address.ToString();
						Console.WriteLine(e.Message);
					}
				}
				else
				{
					try
					{
						hostName = reply.Address.ToString();
					}
					catch (NullReferenceException e)
					{
						hostName = reply.Status.ToString();
						Console.WriteLine(e.Message);
					}
				}

				SetText(results, "\t" + hostName + "\r\n");
				ttl++;
			}
			SetText(results, ((!traceing) ? "Trace complete\r\n" : "Trace Stopped, ttl expired\r\n"));
			pingSender.Dispose();
			Update_Button(sendit, true);
			Update_Button(stopit, false);
		}
	}

	public class frmUPnP : Form
	{
		public Form1 MyParentForm;
		private Label label1 = new Label();
		private Label label2 = new Label();
		private Label label3 = new Label();
		private Label label4 = new Label();
		private TextBox results = new TextBox();
		private TextBox port = new TextBox();
		private TextBox address = new TextBox();
		private ComboBox comboBox1 = new ComboBox();
		private Button refresh = new Button();
		private Button add = new Button();
		private Button delete = new Button();
		private _UPnPNat nat = new _UPnPNat();

		public frmUPnP()
		{
			Text = "FreeMeter UPnP NAT Utility";
			Size = new Size(392, 278);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			MinimizeBox = true;
			MaximizeBox = false;
			ControlBox = true;
			ShowIcon = false;
			Load += new EventHandler(frmUPnP_Load);

			label1.Parent = this;
			label1.Text = "Current UPnP NAT Mappings:";
			label1.Font = new Font("Tahoma", 8);
			label1.Size = new Size(300, 13);
			label1.Location = new Point(3, 5);

			refresh.Parent = this;
			refresh.Text = "Refresh";
			refresh.TabIndex = 6;
			refresh.Font = new Font("Tahoma", 8);
			refresh.Size = new Size(50, 20);
			refresh.Location = new Point(334, 2);
			refresh.Click += new EventHandler(RefreshClick);

			label2.Parent = this;
			label2.Text = "Port";
			label2.Font = new Font("Tahoma", 8);
			label2.Size = new Size(25, 13);
			label2.Location = new Point(3, 235);

			port.Parent = this;
			port.Font = new Font("Tahoma", 8);
			port.TabIndex = 1;
			port.MaximumSize = new Size(40, 16);
			port.MaxLength = 5;
			port.Location = new Point(28, 234);

			label3.Parent = this;
			label3.Text = "Fwd to";
			label3.Font = new Font("Tahoma", 8);
			label3.Size = new Size(38, 13);
			label3.Location = new Point(76, 235);

			address.Parent = this;
			address.Font = new Font("Tahoma", 8);
			address.TabIndex = 2;
			address.MaximumSize = new Size(95, 16);
			address.MaxLength = 15;
			address.Location = new Point(113, 234);

			label4.Parent = this;
			label4.Text = "Proto";
			label4.Font = new Font("Tahoma", 8);
			label4.Size = new Size(32, 13);
			label4.Location = new Point(214, 235);

			comboBox1.FormattingEnabled = true;
			comboBox1.Font = new Font("Small Fonts", 6);
			comboBox1.Location = new Point(247, 234);
			comboBox1.Size = new Size(40, 12);
			comboBox1.TabIndex = 3;
			comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBox1.Items.Add("TCP");
			comboBox1.Items.Add("UDP");
			this.Controls.Add(comboBox1);
			comboBox1.SelectedIndex = 0;

			add.Parent = this;
			add.Size = new Size(40, 20);
			add.Text = "Add";
			add.TabIndex = 4;
			add.Location = new Point(302, 233);
			add.Click += new EventHandler(ButtonAddOnClick);

			delete.Parent = this;
			delete.Size = new Size(40, 20);
			delete.Text = "Rem";
			delete.TabIndex = 5;
			delete.Location = new Point(344, 233);
			delete.Click += new EventHandler(ButtonDeleteOnClick);

			results.Parent = this;
			results.Multiline = true;
			results.TabIndex = 7;
			results.Size = new Size(380, 207);
			results.Location = new Point(3, 24);
			results.BorderStyle = BorderStyle.FixedSingle;
			results.BackColor = Color.Black;
			results.ForeColor = Color.Silver;
			results.Font = new Font("Tahoma", 8);
			results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			results.WordWrap = false;
		}

		private void enumerate_mappings(object sender, DoWorkEventArgs e)
		{
			foreach (PortMappingInfo info in nat.PortMappings)
				SetText(results, info.Description + " - " + info.ExternalPort + " -> " + info.InternalHostName + ":" + info.InternalPort + " " + info.Protocol + "\r\n");
		}
		void frmUPnP_Load(object sender, EventArgs e)
		{
			BackgroundWorker nater = new BackgroundWorker();
			nater.WorkerReportsProgress = false;
			nater.DoWork += new DoWorkEventHandler(enumerate_mappings);
			nater.RunWorkerAsync();
			nater.Dispose();
		}
		void RefreshClick(object obj, EventArgs ea)
		{
			results.Text = "";
			BackgroundWorker nater = new BackgroundWorker();
			nater.WorkerReportsProgress = false;
			nater.DoWork += new DoWorkEventHandler(enumerate_mappings);
			nater.RunWorkerAsync();
			nater.Dispose();
		}
		void ButtonAddOnClick(object obj, EventArgs ea)
		{
			IPAddress ret;
			if (address.Text.Trim() != "" && port.Text.Trim() != "")
			{
				if (IPAddress.TryParse(address.Text.Trim(), out ret))
				{
					try
					{
						PortMappingInfo pmi = new PortMappingInfo("FreeMeter", comboBox1.SelectedItem.ToString(), ret.ToString(), int.Parse(port.Text), null, int.Parse(port.Text), true);
						nat.AddPortMapping(pmi);
						results.Text = "Successfully Added...\r\n\r\n";
						BackgroundWorker nater = new BackgroundWorker();
						nater.WorkerReportsProgress = false;
						nater.DoWork += new DoWorkEventHandler(enumerate_mappings);
						nater.RunWorkerAsync();
						nater.Dispose();
					}
					catch (FormatException)
					{
						results.AppendText("Input was not formatted correctly.\r\n");
					}
					catch (COMException e)
					{
						results.AppendText("Port " + port.Text.Trim() + " " + comboBox1.SelectedItem.ToString() + " was unavailble: " + e.Message + "\r\n");
					}
					catch (ArgumentException)
					{
						results.AppendText("Value was out of range (e.g. ports are 0-65535).\r\n");
					}
				}
				else results.AppendText("Fwd to must be an IP address (e.g. 192.168.0.2).\r\n");
			}
			else results.AppendText("Input was blank.\r\n");
		}
		void ButtonDeleteOnClick(object obj, EventArgs ea)
		{
			if (port.Text.Trim() != "")
			{
				try
				{
					PortMappingInfo pmi = new PortMappingInfo("FreeMeter", comboBox1.SelectedItem.ToString(), null, int.Parse(port.Text), null, int.Parse(port.Text), true);
					nat.RemovePortMapping(pmi);
					results.Text = "Successfully Removed...\r\n\r\n";
					BackgroundWorker nater = new BackgroundWorker();
					nater.WorkerReportsProgress = false;
					nater.DoWork += new DoWorkEventHandler(enumerate_mappings);
					nater.RunWorkerAsync();
					nater.Dispose();
				}
				catch (FormatException)
				{
					results.AppendText("Input was not formatted correctly.\r\n");
				}
				catch (FileNotFoundException)
				{
					results.AppendText("No such mapping to remove.\r\n");
				}
				catch (ArgumentException)
				{
					results.AppendText("Value was out of range (e.g. ports are 0-65535).\r\n");
				}
				catch (COMException e)
				{
					results.AppendText("Error removing Port " + port.Text.Trim() + " " + comboBox1.SelectedItem.ToString() + ": " + e.Message + "\r\n");
				}
			}
			else results.AppendText("Input was blank.\r\n");
		}

		//set the text of a control in a threadsafe manner
		delegate void SetTextCallback(TextBox l, string t);
		private void SetText(TextBox l, string t)
		{
			if (!l.IsDisposed)
			{
				if (l.InvokeRequired)
				{
					SetTextCallback d = new SetTextCallback(SetText);
					try
					{
						this.Invoke(d, new Object[] { l, t });
					}
					catch (ObjectDisposedException e)
					{
						Console.WriteLine(e.ToString());
					}
				}
				else
					l.AppendText(t);
			}
		}
	}

	public class PortMappingInfo
	{
		private bool enabled;
		private string description;
		private string internalHostName;
		private int internalPort;
		private IPAddress externalIPAddress;
		private int externalPort;
		private string protocol;

		public PortMappingInfo(string description, string protocol, string internalHostName, int internalPort, IPAddress externalIPAddress, int externalPort, bool enabled)
		{
			this.enabled = enabled;
			this.description = description;
			this.internalHostName = internalHostName;
			this.internalPort = internalPort;
			this.externalIPAddress = externalIPAddress;
			this.externalPort = externalPort;
			this.protocol = protocol;
		}
		public string InternalHostName
		{
			get { return internalHostName; }
		}
		public int InternalPort
		{
			get { return internalPort; }
		}
		public IPAddress ExternalIPAddress
		{
			get { return externalIPAddress; }
		}
		public int ExternalPort
		{
			get { return externalPort; }
		}
		public string Protocol
		{
			get { return protocol; }
		}
		public bool Enabled
		{
			get { return enabled; }
		}
		public string Description
		{
			get { return description; }
		}
	}

	public class _UPnPNat
	{
		private UPnPNAT upnp;

		public _UPnPNat()
		{
			try
			{
				UPnPNAT nat = new UPnPNAT();
				if (nat.NATEventManager != null && nat.StaticPortMappingCollection != null)
					upnp = nat;
			}
			catch { }

			if (upnp == null) // No configurable UPNP NAT is available.
				throw new NotSupportedException();
		}
		public PortMappingInfo[] PortMappings
		{
			get
			{
				ArrayList portMappings = new ArrayList();

				// Enumerates the ports without using the foreach statement (causes the interop to fail).
				int count = upnp.StaticPortMappingCollection.Count;
				IEnumerator enumerator = upnp.StaticPortMappingCollection.GetEnumerator();
				enumerator.Reset();

				for (int i = 0; i <= count; i++)
				{
					IStaticPortMapping mapping = null;
					try
					{
						if (enumerator.MoveNext())
							mapping = (IStaticPortMapping)enumerator.Current;
					}
					catch { }

					if (mapping != null)
						portMappings.Add(new PortMappingInfo(mapping.Description, mapping.Protocol.ToUpper(), mapping.InternalClient, mapping.InternalPort, IPAddress.Parse(mapping.ExternalIPAddress), mapping.ExternalPort, mapping.Enabled));
				}

				// copies the ArrayList to an array of PortMappingInfo.
				PortMappingInfo[] portMappingInfos = new PortMappingInfo[portMappings.Count];
				portMappings.CopyTo(portMappingInfos);

				return portMappingInfos;
			}
		}

		public void AddPortMapping(PortMappingInfo portMapping)
		{
			upnp.StaticPortMappingCollection.Add(portMapping.ExternalPort, portMapping.Protocol, portMapping.InternalPort, portMapping.InternalHostName, portMapping.Enabled, portMapping.Description);
		}

		public void RemovePortMapping(PortMappingInfo portMapping)
		{
			upnp.StaticPortMappingCollection.Remove(portMapping.ExternalPort, portMapping.Protocol);
		}
	}
}// EOF