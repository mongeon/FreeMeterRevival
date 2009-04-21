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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace FreeMeterRevival.Forms
{

	public partial class MainForm : ComponentFactory.Krypton.Toolkit.KryptonForm
	{

		public string ClipData = "";
		public ArrayList MailServers = new ArrayList();

		#region <Name>

		public bool will_reboot = false;

		#endregion

		private Assembly myAssembly = Assembly.GetExecutingAssembly();
		private NetworkMonitor monitor = new NetworkMonitor();

		private int timerInterval = 1000;
		private int WLength, WHeight, scale;
		private bool m_closing = false;
		private string display_xscale, display_yscale;
		private int[] downlines = new int[16];
		private int[] uplines = new int[16];
		private int[] m_full_downlines, m_full_uplines; 
		private double[] full_downspeeds, full_upspeeds;
		public double downspeed = 0.0; //modified to from private -> public by miechu
		public double upspeed = 0.0; //modified to from private -> public by miechu
		private bool respond_to_latest = false;

		//Cool icon representation
		private bool icon_representation = false;
		private bool icons_loaded = false;
		private Image upload_icon_green;
		private Image upload_icon_red;
		private Image download_icon_green;
		private Image download_icon_red;

		//##TotalsLog##
		Totals_LogForm logs_form;
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

        public MainForm(SplashForm frmSplash)
		{
            frmSplash.ShowState("Initialize UI...");
            InitializeComponent();

            frmSplash.ShowState("Get the network adapter(s)...");
			if (monitor.Adapters.Length == 0)
			{
				MessageBox.Show("I can't find any network adapters on this computer.", "FreeMeter Revival Failed.");
				return;
			}
            foreach (NetworkAdapter adapter in monitor.Adapters)
            {
                ToolStripMenuItem tmp = new ToolStripMenuItem(adapter.name,null, new EventHandler(SetAdapter));
                m_interfaces.DropDownItems.Add(tmp);
                tmp.Checked = adapter.Enabled;
            }

            frmSplash.ShowState("Start the logs...");
			logs_form = new Totals_LogForm();

			RestoreRegistry();

            frmSplash.ShowState("Load the configuration...");
			try
			{
				SetDefaults();
				LoadConfiguration();
			}
			catch
			{}

            frmSplash.ShowState("Check the menus...");
			Check_Menus();

            frmSplash.ShowState("Set the transparency trackbar...");
			// Transparency Trackbar
			trackBar2.Location = new Point(2, WHeight - 14);
			trackBar2.Size = new Size(WLength - 15, 15);
			trackBar2.SendToBack();
			trackBar2.Hide();

            frmSplash.ShowState("Start the check process...");
            backgroundWorker1.RunWorkerAsync();

            frmSplash.ShowState("Launch check version background process...");
			Check_Version(this, new EventArgs());

            //hack to initially try to reduce the memory footprint of the app (admin only)
            try
            {
                Process loProcess = Process.GetCurrentProcess();
                loProcess.MaxWorkingSet = loProcess.MaxWorkingSet;
                loProcess.Dispose();
            }
            catch { }

            ShrinkTimer.Start();

            frmSplash.CloseWithSleep();


		}

		void UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			(new ErrorForm(e.ExceptionObject as Exception)).ShowDialog();
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
			DrawFullMeter(m_full_downlines,m_full_uplines);

			
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


		//transparency and opacity
		private void Trackbar2_Update(Object sender, EventArgs e)
		{
			TrackBar tb = (TrackBar)sender;
			this.Opacity = ((float)tb.Value) / 100;
		}
		private void Trackbar2_Hide(Object sender, MouseEventArgs e)
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
					m_full_downlines[i] = m_full_downlines[i + 1];
					m_full_uplines[i] = m_full_uplines[i + 1];
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
			m_full_downlines[full_downspeeds.Length - 1] = (int)(FullMeter.Height * downspeed / scale);
			m_full_uplines[full_downspeeds.Length - 1] = (int)(FullMeter.Height * upspeed / scale);
			if (m_full_downlines[full_downspeeds.Length - 1] > FullMeter.Height) m_full_downlines[full_downspeeds.Length - 1] = FullMeter.Height;
			if (m_full_uplines[full_downspeeds.Length - 1] > FullMeter.Height) m_full_uplines[full_downspeeds.Length - 1] = FullMeter.Height;

			if (autoscale_checked.Checked)
				Auto_Scale();
		}

		private void DrawIconRepresentation()
		{
			Bitmap b = new Bitmap(16, 16, PixelFormat.Format16bppRgb555);
			Graphics g = Graphics.FromImage((Image)b);
            g.FillRegion(new LinearGradientBrush(new PointF(b.Width / 2, 0), new PointF(b.Width / 2, b.Height ), Color.Gray, Color.Black), new Region(new Rectangle(0, 0, 16, 16)));

			if (!icon_representation)
			{
				//draw each line in the graph
				DrawNetworkGraph.DrawGraph(g, downlines, uplines,16,16 ,true);
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

        private void DrawFullMeter(int[] full_downlines, int[] full_uplines)
		{
            if (this.Visible && FullMeter.Width != 0 && FullMeter.Height != 0)
			{
                FullMeter.UpdateGraph(full_downlines, full_uplines);
                sbMainStatus.Text = display_xscale + " " + display_yscale;
			}
			DoStringOutput();
		}
		private void Auto_Scale()
		{
			if (FullMeter.Height * Max(full_downspeeds) / scale > FullMeter.Height || FullMeter.Height * Max(full_upspeeds) / scale > FullMeter.Height)
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
			else if (FullMeter.Height * Max(full_downspeeds) / scale < FullMeter.Height / 3 && FullMeter.Height * Max(full_upspeeds) / scale < FullMeter.Height / 3)
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
            sbMainDownload.Text = label1text;
            sbMainUpload.Text = label2text;
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
				if (this.Visible)
				{
					msMainFileShowMeter.Checked = false;
					Hide();
				}
				else
				{
					this.Show();
					msMainFileShowMeter.Checked = true;
				}
			}
			else Check_Menus();
		}
		private void Main_MouseDown(Object sender, MouseEventArgs e)
		{
			ptOffset = new Point(-e.X - FullMeter.Left, -e.Y - FullMeter.Top);
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
		private void MainForm_Resize(Object sender, EventArgs e)
		{
			if (ClientSize.Width > 40 && ClientSize.Height > 40)
			{
				WLength = ClientSize.Width;
				WHeight = ClientSize.Height;
			}

			FullMeter.Size = new Size(WLength - 18, WHeight - 60);
			trackBar2.Location = new Point(2, WHeight - 14);
			trackBar2.Size = new Size(WLength - 15, 15);

			display_xscale = "Time: " + (timerInterval * FullMeter.Width / 1000).ToString() + "s ";

			//resize arrays to match new window size
			int[] temp = new int[FullMeter.Width];
			double[] temp2 = new double[FullMeter.Width];
			if (m_full_downlines.Length <= temp.Length)
				Array.Copy(m_full_downlines, 0, temp, temp.Length - m_full_downlines.Length, m_full_downlines.Length);
			else
				Array.Copy(m_full_downlines, m_full_downlines.Length - temp.Length, temp, 0, temp.Length);
			m_full_downlines = temp;

			temp = new int[FullMeter.Width];
			if (m_full_uplines.Length <= temp.Length)
				Array.Copy(m_full_uplines, 0, temp, temp.Length - m_full_uplines.Length, m_full_uplines.Length);
			else
				Array.Copy(m_full_uplines, m_full_uplines.Length - temp.Length, temp, 0, temp.Length);
			m_full_uplines = temp;

			temp2 = new double[FullMeter.Width];
			if (full_downspeeds.Length <= temp2.Length)
				Array.Copy(full_downspeeds, 0, temp2, temp2.Length - full_downspeeds.Length, full_downspeeds.Length);
			else
				Array.Copy(full_downspeeds, full_downspeeds.Length - temp2.Length, temp2, 0, temp2.Length);
			full_downspeeds = temp2;

			temp2 = new double[FullMeter.Width];
			if (full_upspeeds.Length <= temp2.Length)
				Array.Copy(full_upspeeds, 0, temp2, temp2.Length - full_upspeeds.Length, full_upspeeds.Length);
			else
				Array.Copy(full_upspeeds, full_upspeeds.Length - temp2.Length, temp2, 0, temp2.Length);
			full_upspeeds = temp2;

			
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
                return Properties.Settings.Default.UploadColor;
			}
		}
		public void RGB_to_HSL(Color c, ref double h, ref double s, ref double l)
		{
			h = c.GetHue();
			s = c.GetSaturation();
			l = c.GetBrightness();
		}


		

		public void Check_Menus()
		{
            //interval_tenth.Checked = interval_fifth.Checked = interval_half.Checked = interval_1.Checked = false;
            //scale_33.Checked = scale_56.Checked = scale_64.Checked = scale_128.Checked = scale_256.Checked = scale_512.Checked = scale_640.Checked = scale_1000.Checked = scale_1500.Checked = scale_2000.Checked = scale_3000.Checked = scale_5000.Checked = scale_7000.Checked = scale_10000.Checked = scale_11000.Checked = scale_32000.Checked = scale_54000.Checked = scale_100000.Checked = scale_1000000.Checked = scale_custom.Checked = false;

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
                case 4200: scale_33.Checked = true; display_yscale = "Scale: 33.6 kb"; break;
                case 7000: scale_56.Checked = true; display_yscale = "Scale: 56 kb"; break;
                case 8000: scale_64.Checked = true; display_yscale = "Scale: 64 kb"; break;
                case 16000: scale_128.Checked = true; display_yscale = "Scale: 128 kb"; break;
                case 32000: scale_256.Checked = true; display_yscale = "Scale: 256 kb"; break;
                case 64000: scale_512.Checked = true; display_yscale = "Scale: 512 kb"; break;
                case 80000: scale_640.Checked = true; display_yscale = "Scale: 640 kb"; break;
                case 128000: scale_1000.Checked = true; display_yscale = "Scale: 1 mb"; break;
                case 192000: scale_1500.Checked = true; display_yscale = "Scale: 1.5 mb"; break;
                case 256000: scale_2000.Checked = true; display_yscale = "Scale: 2 mb"; break;
                case 384000: scale_3000.Checked = true; display_yscale = "Scale: 3 mb"; break;
                case 640000: scale_5000.Checked = true; display_yscale = "Scale: 5 mb"; break;
                case 896000: scale_7000.Checked = true; display_yscale = "Scale: 7 mb"; break;
                case 1280000: scale_10000.Checked = true; display_yscale = "Scale: 10 mb"; break;
                case 1408000: scale_11000.Checked = true; display_yscale = "Scale: 11 mb"; break;
                case 4096000: scale_32000.Checked = true; display_yscale = "Scale: 32 mb"; break;
                case 6912000: scale_54000.Checked = true; display_yscale = "Scale: 54 mb"; break;
                case 12800000: scale_100000.Checked = true; display_yscale = "Scale: 100 mb"; break;
                case 128000000: scale_1000000.Checked = true; display_yscale = "Scale: 1 gb"; break;
                default: scale_custom.Checked = true; display_yscale = "Scale: custom (" + Totals_LogForm.Value(scale, null) + ")"; break;
            }
		}

		// handlers for menu clicks
		private void SetTimerInterval(Object sender, EventArgs e)
		{
            foreach (ToolStripMenuItem m in m_interval_menu.DropDownItems)
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
					display_xscale = "Time: " + timerInterval * FullMeter.Width / 1000 + "s ";
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

            if (m_full_downlines.Length > 0)
            {


                for (int i = 0; i < m_full_downlines.Length; i++)
                {
                    m_full_downlines[i] = (int)(FullMeter.ClientSize.Height * full_downspeeds[i] / scale);
                    m_full_uplines[i] = (int)(FullMeter.ClientSize.Height * full_upspeeds[i] / scale);
                }
                for (int i = 0; i < downlines.Length; i++)
                {
                    downlines[i] = 16 * (int)full_downspeeds[m_full_downlines.Length - 16 + i] / scale;
                    uplines[i] = 16 * (int)full_upspeeds[m_full_downlines.Length - 16 + i] / scale;
                }
            }
        }

		private void SetScale_MenuClick(Object sender, EventArgs e)//from a menu click to change graph scale
		{
            foreach (ToolStripMenuItem m in m_scale_menu.DropDownItems)
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
										GetValueForm g = new GetValueForm("Provide custom scale in bytes (1024B = 1KB)");

										if (g.ShowDialog() == DialogResult.OK)
										{
											try
											{
												scale = int.Parse(g.Value);
												m.Text = "custom (" + Totals_LogForm.Value(scale, null) + ")";
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
                foreach (ToolStripMenuItem m in m_interfaces.DropDownItems)
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
            FullMeter.ShowSummary = graphs_summary.Checked;
		}

		private void SetGraph_Download(Object sender, EventArgs e)
		{
			graphs_download.Checked = !graphs_download.Checked;
		}
		
		private void SetGraph_Upload(Object sender, EventArgs e)
		{
			graphs_upload.Checked = !graphs_upload.Checked;
		}
		
		
		private void Avg_Click(Object sender, EventArgs e)
		{
			//avg_checked.Checked = !avg_checked.Checked;
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
			if (this.Visible)
			{
				msMainFileShowMeter.Checked = false;
				Hide();
			}
			else
			{
				Show();
				msMainFileShowMeter.Checked = true;

				Rectangle rect = Screen.PrimaryScreen.WorkingArea;

				if (this.Location.X > rect.Width || this.Location.Y > rect.Height || this.Location.Y < 0 || this.Location.X < 0)
					this.CenterToScreen();
			}
		}

		private void TopMost_Click(Object sender, EventArgs e)
		{
			msMainFileTopmost.Checked = !msMainFileTopmost.Checked;
			TopMost = msMainFileTopmost.Checked;
		}

		private void SimpleNotifyIcon_Click(Object sender, EventArgs e)
		{
			msMainFileSimpleIcon.Checked = !msMainFileSimpleIcon.Checked;
			icon_representation = msMainFileSimpleIcon.Checked;
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
		
		private void Ping_Click(Object sender, EventArgs e)
		{
			PingForm frm = new PingForm();
			frm.MyParentForm = this;
			frm.Show(this);
		}
		private void Trace_Click(Object sender, EventArgs e)
		{
			TraceForm frm = new TraceForm();
			frm.MyParentForm = this;
			frm.Show(this);
		}
		private void UPnP_Click(Object sender, EventArgs e)
		{
            UPnPForm frm = new UPnPForm();
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
            //TODO: Create my own web service to get this info
            AssemblyName ThisAssemblyName = myAssembly.GetName();
            string FriendlyVersion =  ThisAssemblyName.Version.Major + "." + ThisAssemblyName.Version.Minor + "." + ThisAssemblyName.Version.Build;
            if (!respond_to_latest)
                Thread.Sleep(30000);
            try
            {
                WebRequest w = WebRequest.Create("http://freemeterrevival.codeplex.com/Wiki/View.aspx?title=version");
            
                Stream sw = w.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(sw);
                string line = sr.ReadLine();
                while (!line.Contains(":::::"))
                {
                     line = sr.ReadLine();
                }
                
                string[] split = line.Split(new char[]{':'}, StringSplitOptions.RemoveEmptyEntries);
                line = split[1];
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
                Registry.CurrentUser.DeleteSubKey("Software\\FreeMeterRevival");
			}
			catch
			{ }
		}

		protected override void Dispose(bool disposing)
		{
			if (backgroundWorker1 != null)
				backgroundWorker1.CancelAsync();

			SaveConfiguration();
			
			if (disposing)
				m_notifyicon.Dispose();

			//base.Dispose(disposing);
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
			msMainFileTopmost.Checked		= bool.Parse(xml["TopMost"].ToString());
			msMainFileSimpleIcon.Checked = bool.Parse(xml["SimpleNotifyIcon"].ToString());
			clip_watch.Checked			= bool.Parse(xml["ClipWatch"].ToString());
			LogEnabled					= bool.Parse(xml["LogEnabled"].ToString());

			timerInterval = int.Parse(xml["TimerInterval"].ToString());
			LogInterval = int.Parse(xml["LogInterval"].ToString());
			scale = int.Parse(xml["GraphScale"].ToString());

			WLength = int.Parse(xml["DispWidth"].ToString());
			WHeight = int.Parse(xml["DispHeight"].ToString());
			m_full_downlines = new int[WLength];
			m_full_uplines = new int[WLength];
			full_downspeeds = new double[WLength];
			full_upspeeds = new double[WLength];
			ClientSize = new Size(WLength, WHeight);

			if (xml["WindowIsVisible"].ToString().ToLower() == "true")
			{
				Show();
				Location = new Point(int.Parse(xml["WindowX"].ToString()), int.Parse(xml["WindowY"].ToString()));
				msMainFileShowMeter.Checked = true;
			}
			else
			{
				Location = new Point(int.Parse(xml["WindowX"].ToString()), int.Parse(xml["WindowY"].ToString()));
				msMainFileShowMeter.Checked = false;
				Hide();
               
			}
            

			if (clip_watch.Checked)
				ClipTimer.Start();

			LogTimer.Interval = LogInterval * 60000;

			if (LogEnabled)
				LogTimer.Start();

			trackBar2.Maximum = 100;
			trackBar2.Minimum = 30;
			trackBar2.Value = int.Parse(xml["Trans"].ToString());
			Opacity = ((float)trackBar2.Value) / 100;


			TopMost = msMainFileTopmost.Checked;
			icon_representation = msMainFileSimpleIcon.Checked;

			logs_form.LoadConfiguration(xml);

			graphs_summary.Checked = bool.Parse(xml["GraphSummary"].ToString());
            FullMeter.ShowSummary = graphs_summary.Checked;
		}

		private void SetDefaults()
		{
			timerInterval = 1000;
			scale = 7000;
			avg_checked.Checked = false;

			WLength = 126;
			WHeight = 64;
			m_full_downlines = new int[WLength];
			m_full_uplines = new int[WLength];
			full_downspeeds = new double[WLength];
			full_upspeeds = new double[WLength];
			ClientSize = new Size(WLength, WHeight);

			units_kbytes.Checked = true;
			units_kbits.Checked = false;
			graphs_download.Checked = true;
			graphs_upload.Checked = true;

			autoscale_checked.Checked = true;
			this.CenterToScreen(); // Location
			msMainFileTopmost.Checked = true;

			msMainFileShowMeter.Checked = true;
			Show();


			trackBar2.Maximum = 100;
			trackBar2.Minimum = 30;
			trackBar2.Value = 100;
			Opacity = ((float)trackBar2.Value) / 100;

			clip_watch.Checked = false;
			LogInterval = 5;
			LogEnabled = false;

		}

		private void SaveConfiguration()
		{
            Properties.Settings.Default.Save();
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
			writer.WriteElementString("TopMost", msMainFileTopmost.Checked.ToString());
			writer.WriteElementString("SimpleNotifyIcon", msMainFileSimpleIcon.Checked.ToString());
			writer.WriteElementString("ClipWatch", clip_watch.Checked.ToString());
			writer.WriteElementString("LogEnabled", LogEnabled.ToString());

			writer.WriteElementString("TimerInterval", timerInterval.ToString());
			writer.WriteElementString("LogInterval", LogInterval.ToString());


            if (this.Visible)
            {

                writer.WriteElementString("WindowX", this.Location.X.ToString());
                writer.WriteElementString("WindowY", this.Location.Y.ToString());
                //writer.WriteElementString("DispWidth", ClientSize.Width.ToString());
                //writer.WriteElementString("DispHeight", ClientSize.Height.ToString());
            }
            else
            {
                writer.WriteElementString("WindowX", Convert.ToInt32( Screen.PrimaryScreen.WorkingArea.Width/2).ToString());
                writer.WriteElementString("WindowY", Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height / 2).ToString());
                writer.WriteElementString("DispWidth","640"/*ClientSize.Width.ToString()*/);
                writer.WriteElementString("DispHeight","480" /*ClientSize.Height.ToString()*/);
            }
            writer.WriteElementString("DispWidth", "640"/*ClientSize.Width.ToString()*/);
            writer.WriteElementString("DispHeight", "480" /*ClientSize.Height.ToString()*/);

			if (this.Visible)
				writer.WriteElementString("WindowIsVisible", "True");
			else
				writer.WriteElementString("WindowIsVisible", "False");

			writer.WriteElementString("GraphScale", scale.ToString());
			writer.WriteElementString("Trans", trackBar2.Value.ToString());

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
		private void ShowTotalsLog_Click(Object sender, EventArgs e)
		{
			logs_form.Show(this);
		}

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ElapsedTimer();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
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
                        backgroundWorker1.ReportProgress(0);
                        
                        QueryPerformanceCounter(out count2);
                        long time_ms = (count2 - count1) * 1000 / freq;
                        if (time_ms > (long)timerInterval)
                            time_ms = (long)timerInterval;
                        Thread.Sleep((int)((long)timerInterval - time_ms));
                    }
                }
                else
                    MessageBox.Show("I can't find QueryPerformanceFrequency()", "FreeMeter Revival Failed.");
            }
            else
                MessageBox.Show("I failed to use QueryPerformanceFrequency()", "FreeMeter Revival Failed.");

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.ApplicationExitCall:
                case CloseReason.None:
                case CloseReason.TaskManagerClosing:
                case CloseReason.WindowsShutDown:
                case CloseReason.FormOwnerClosing:

                case CloseReason.MdiFormClosing:
                    e.Cancel = false;
                    m_closing = true;
                    break;

                case CloseReason.UserClosing:
                    this.Hide();
                    e.Cancel = true;
                    m_closing = false;
                
                    break;
            }
        }

        #region Menus
        private void msMainFileExit_Click(object sender, EventArgs e)
        {
            m_closing = true;
            Application.Exit();
        }
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Show / Hide the status bar when the checked state change
        /// </summary>
        ///----------------------------------------------------------------------------------------
        private void msMainWindowStatusBar_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StatusBarVisible = msMainWindowStatusBar.Checked;
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm frmOptions = new OptionsForm();
            frmOptions.ShowDialog();
            
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {

        }






	}




	
}// EOF