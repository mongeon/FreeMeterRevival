using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace FreeMeterRevival.Forms
{
	public partial class Totals_LogForm : ComponentFactory.Krypton.Toolkit.KryptonForm
	{
		enum TimeSpan { Day = 0, Week = 1, Month = 2, Year = 3 };
		enum DownloadDirection { Upload, Download, Both };

        public class Day
        {
            [XmlAttribute("date")]    
            public string date;

            [XmlAttribute("traffic")]
            public double traffic;
        }

        [XmlRoot("Totals")]
        public class Totals_LogData
        {
            [XmlIgnore]
            public Hashtable date_downloads;

            [XmlArray("downloads")]
            public Day[] TotalDownloads
            {
                get { return SerializeHash(date_downloads); }
                set { date_downloads = DeSerializeHash(value); }
            }

            [XmlIgnore]
            public Hashtable date_uploads;

            [XmlArray("uploads")]
            public Day[] TotalUploads
            {
                get { return SerializeHash(date_uploads); }
                set { date_uploads = DeSerializeHash(value); }
            }
                                 
            [XmlElement("counter_start_date")]
            public DateTime counter_date;

            [XmlElement("counter_uploaded")]
            public double counter_uploaded;

            [XmlElement("counter_downloaded")]
            public double counter_downloaded;

            public Totals_LogData()
            {
                date_downloads = new Hashtable();
                date_uploads = new Hashtable();
            }

            public Day[] SerializeHash(Hashtable hash)
            {
                Day[] serialized = new Day[hash.Count];

                int pos = 0;
                foreach (string key in hash.Keys)
                {
                    serialized[pos] = new Day();
                    serialized[pos].date = key;
                    serialized[pos].traffic = (double)hash[key];
                    pos++;
                }

                return serialized;
            }

            public Hashtable DeSerializeHash(Day[] hash)
            {
                Hashtable deserialized = new Hashtable();

                for (int i = 0; i < hash.Length; i++)
                    deserialized[hash[i].date] = (double)hash[i].traffic;

                return deserialized;
            }


        }

        private Totals_LogData log_data;
		private ContextMenuStrip little_menu;

		//limits
		private double max_upload;
		private double max_download;
		private double max_both;
		private TimeSpan limits_period;
		private bool limits_check;
		private bool limits_notified;

		public Totals_LogForm()
		{
			limits_notified = false;
			InitializeComponent();

			little_menu = new ContextMenuStrip();
            LoadData();
		}

		public void LoadCofing()
		{ }

		public void SaveConfiguration(XmlTextWriter writer)
		{
			writer.WriteElementString("LogsUnit",		choose_unit.Text);
			writer.WriteElementString("LogsPeriod",		choose_period.Text);
			writer.WriteElementString("LimitsCheck",	limitsCheckBox.Checked.ToString());
			writer.WriteElementString("LimitsUpload",	(max_upload / (1024 * 1024)).ToString());
			writer.WriteElementString("LimitsDownload", (max_download / (1024 * 1024)).ToString());
			writer.WriteElementString("LimitsBoth",		(max_both / (1024 * 1024)).ToString());
			writer.WriteElementString("LimitsPeriod",	((int)limits_period).ToString());
		}

		public void LoadConfiguration(Hashtable xml)
		{
			try
			{
				choose_unit.Text = xml["LogsUnit"].ToString();
				choose_period.Text = xml["LogsPeriod"].ToString();
				PopulateTable();

				maxUploadBox.Text = xml["LimitsUpload"].ToString();
				maxDownloadBox.Text = xml["LimitsDownload"].ToString();
				maxBothBox.Text = xml["LimitsBoth"].ToString();
				periodComboBox.SelectedIndex = int.Parse(xml["LimitsPeriod"].ToString());
				limitsCheckBox.Checked = Boolean.Parse(xml["LimitsCheck"].ToString());
			}
			catch
			{ }
		}

        public void SaveData()
        {
            string app_dir = Application.ExecutablePath;
            app_dir = app_dir.Remove(app_dir.LastIndexOf('\\') + 1);

            XmlSerializer s = new XmlSerializer(typeof(Totals_LogData));
            TextWriter w = new StreamWriter(app_dir + "totals.xml");
            s.Serialize(w, log_data);
            w.Close();
        }

		private void LoadData()
		{
            string app_dir = Application.ExecutablePath;
            app_dir = app_dir.Remove(app_dir.LastIndexOf('\\') + 1);

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(Totals_LogData));
                TextReader r = new StreamReader(app_dir + "totals.xml");
                log_data = (Totals_LogData)s.Deserialize(r);
                r.Close();
            }
            catch
            {
                log_data = new Totals_LogData();
            }

			SetNewCounterDate();
            PopulateTable();
		}

        private void PopulateTable()
        {
            Hashtable period_up = new Hashtable();
            Hashtable period_down = new Hashtable();

            foreach (string day in log_data.date_uploads.Keys)
            {
                string s;
                if (choose_period.Text == "daily")
                    s = DateTime.Parse(day).ToString("dd-MM-yyyy");
                else if (choose_period.Text == "monthly")
                    s = DateTime.Parse(day).ToString("MMMM yyyy");
                else if (choose_period.Text == "weekly")
                    s = "to be implemented";
                else
                    s = "Total";


                if (!period_up.Contains(s))
                    period_up.Add(s, (double)log_data.date_uploads[day]);
                else
                    period_up[s] = (double)period_up[s] + (double)log_data.date_uploads[day];


                if (!period_down.Contains(s))
                    period_down.Add(s, (double)log_data.date_downloads[day]);
                else
                    period_down[s] = (double)period_down[s] + (double)log_data.date_downloads[day];

            }

            listView1.Items.Clear();

            foreach (string period in period_up.Keys)
                listView1.Items.Add(new ListViewItem(new string[] { "", Value((double)period_down[period], choose_unit.Text), Value((double)period_up[period], choose_unit.Text), Value((double)period_up[period] + (double)period_down[period], choose_unit.Text), period }));
        }

        protected override void Dispose(bool disposing)
        {
            SaveData();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		private void Totals_Log_Load(object sender, EventArgs e)
		{
			CenterToScreen();
			periodComboBox.SelectedIndex = 0;
		}

		private delegate void UpdateDataCallback(double up, double down);

		public void UpdateData(double up, double down) 
		{
			if (InvokeRequired)
			{
				UpdateDataCallback d = new UpdateDataCallback(UpdateData);
                Invoke(d, new object[] {up, down});
			}	
			else
			{
                string today = DateTime.Now.ToShortDateString();

                if (!log_data.date_downloads.Contains(today))	
				{
                    log_data.date_downloads.Add(today, 0.0);
                    log_data.date_uploads.Add(today, 0.0);
				}

                log_data.date_uploads[today] = ((double)log_data.date_uploads[today]) + up;
                log_data.date_downloads[today] = ((double)log_data.date_downloads[today]) + down;

                label_today_uploaded.Text = Value((double)log_data.date_uploads[today], null);
                label_today_downloaded.Text = Value((double)log_data.date_downloads[today], null);
                label_today_both.Text = Value((double)log_data.date_uploads[today] + (double)log_data.date_downloads[today], null);

                log_data.counter_uploaded += up;
                log_data.counter_downloaded += down;

                label_counter_uploaded.Text = Value(log_data.counter_uploaded, null);
                label_counter_downloaded.Text = Value(log_data.counter_downloaded, null);
                label_counter_both.Text = Value(log_data.counter_uploaded + log_data.counter_downloaded, null);

				listView1.Items.Clear();
				PopulateTable();


				if (limits_check && !limits_notified)
				{ 
					double uploaded = GetTraffic(limits_period, DownloadDirection.Upload);
					double downloaded = GetTraffic(limits_period, DownloadDirection.Download);
					double both = GetTraffic(limits_period, DownloadDirection.Both);

					if (max_upload != 0 && max_upload < uploaded)
						NotifyOfExceeded(DownloadDirection.Upload, uploaded, max_upload);
					
					if (max_download != 0 && max_download < downloaded)
						NotifyOfExceeded(DownloadDirection.Download, downloaded, max_download);

					if (max_both != 0 && max_both < both)
						NotifyOfExceeded(DownloadDirection.Both, both, max_both);
				}
				
			}
		}

		private void NotifyOfExceeded(DownloadDirection downloadDirection, double amount, double max_allowed)
		{
			string msg = string.Format("You have exceeded maximum {0} limit ({1}/{2}) for this {3}.", downloadDirection.ToString(), Value(amount, null), Value(max_allowed, null), limits_period.ToString());

			limits_notified = true;
			//MainForm.m_notifyicon.ShowBalloonTip (int.MaxValue, "Bandwidth limit exceeded", msg, ToolTipIcon.Warning);
		}

		private double GetTraffic(TimeSpan limits_period, DownloadDirection downloadDirection)
		{
			double downloaded = 0;
			double uploaded = 0;


			string period = "";
			
			switch (limits_period)
			{
				case TimeSpan.Day: 
									period = DateTime.Now.ToString("dd-MM-yyyy");
									break;

				case TimeSpan.Week:
									period = "";
									break;

				case TimeSpan.Month:
									period = DateTime.Now.ToString("MM-yyyy");
									break;

				case TimeSpan.Year:
									period = DateTime.Now.ToString("yyyy");
									break;
			}


			foreach (string day in log_data.date_uploads.Keys)
			{
				string s = "";
				switch (limits_period)
				{
					case TimeSpan.Day: 
										s = DateTime.Parse(day).ToString("dd-MM-yyyy");
										break;

					case TimeSpan.Week:
										s = "";
										break;

					case TimeSpan.Month:
										s = DateTime.Parse(day).ToString("MM-yyyy");
										break;

					case TimeSpan.Year:
										s = DateTime.Parse(day).ToString("yyyy");
										break;
				}

				if (s == period)
				{
					uploaded += (double)log_data.date_uploads[day];
					downloaded += (double)log_data.date_downloads[day];
				}
			}


			switch (downloadDirection)
			{
				case DownloadDirection.Upload: 
												return uploaded;

				case DownloadDirection.Download:
												return downloaded;

				case DownloadDirection.Both:
												return uploaded + downloaded;
			}

			return 0;
		}

        private void SetNewCounterDate()
        {
            counter.Text = "Counter started:  " + log_data.counter_date.ToString("dd-MM-yyyy HH:mm:ss");
        }

		public static string Value(double value, string unit)
		{
			double div = 1;

            if (unit == null)
            {
                if (value < 1024)
                    unit = " B";
                else if (value < 1024 * 1024)
                    unit = "KB";
                else if (value < 1024 * 1024 * 1024)
                    unit = "MB";
                else
                    unit = "GB";
            }

            if (unit == " B")
                div = 1;
            else if (unit == "KB")
                div = 1024;
            else if (unit == "MB")
                div = 1024 * 1024;
            else if (unit == "GB")
                div = 1024 * 1024 * 1024;
            

            String string_value = (value/div).ToString("0.00");

			return string_value.Replace(',','.') + " " + unit;
		}

		private void Totals_Log_FormClosing(object sender, FormClosingEventArgs e)
		{
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                Visible = false;
                e.Cancel = true;
            }
		}

		private void choose_unit_Click(object sender, EventArgs e)
		{
			string[] units = { "KB", "MB", "GB" };

            little_menu.Items.Clear();

            foreach (string unit in units)
                little_menu.Items.Add(new ToolStripMenuItem(unit, null, SetUnitClick));

			little_menu.Show(choose_unit, new Point(0, 0),ToolStripDropDownDirection.Left);
		}

        private void SetPeriodClick(object sender, EventArgs e)
        {
            choose_period.Text = ((ToolStripMenuItem) sender).Text;
            PopulateTable();
        }

        private void SetUnitClick(object sender, EventArgs e)
        {
            choose_unit.Text = ((ToolStripMenuItem) sender).Text;
            PopulateTable();
        }

		private void choose_period_Click(object sender, EventArgs e)
		{
			string[] units = { "daily", "weekly", "monthly", "total" };

			little_menu.Items.Clear();

			foreach (string unit in units)
                little_menu.Items.Add(new ToolStripMenuItem(unit,null, SetPeriodClick));

			little_menu.Show(choose_period, new Point(choose_period.Width, 0),ToolStripDropDownDirection.Right);
		}

        private void export_Click(object sender, EventArgs e)
        {
        	string[] units = { "csv", "html" };

            little_menu.Items.Clear();

			foreach (string unit in units)
                little_menu.Items.Add(new ToolStripMenuItem(unit,null, ExportTheData));

            little_menu.Show(export, new Point(0, 0), ToolStripDropDownDirection.Left);    
        }

        private void reset_Click(object sender, EventArgs e)
        {
            log_data.counter_date = DateTime.Now;
            log_data.counter_downloaded = 0;
            log_data.counter_uploaded = 0;

            SetNewCounterDate();
        }
        
        private void ExportTheData(object sender, EventArgs e)
        {
            string export_type = ((ToolStripMenuItem) sender).Text;
            SaveFileDialog dialog = new SaveFileDialog();
            
            if (export_type == "csv")
            {
            	dialog.Filter = "Comma separated values (*.csv)|*.csv"; 
            	
            	if (dialog.ShowDialog() != DialogResult.OK)
	            	return;
            	
            	TextWriter file = new StreamWriter(dialog.FileName);
            	
            	file.Write("; Downloads\r\n");
            	foreach (Day d in log_data.TotalDownloads)
            		file.Write(String.Format("{0},{1}\r\n", d.date, d.traffic));
            	
            	file.Write("; Uploads\r\n");
				foreach (Day d in log_data.TotalUploads)
            		file.Write(String.Format("{0},{1}\r\n", d.date, d.traffic));            	
					           
				file.Close();
            }
            else if (export_type == "html")
            {
            	dialog.Filter = "HTML Summary (*.html)|*.html"; 
            	
            	if (dialog.ShowDialog() != DialogResult.OK)
	            	return;
            	
            	string header = "<html><body style=\"font-family: Trebuchet MS, Tahoma, Verdana;\"><br/><br/><center><table border=\"0\" cellpadding=\"6\" cellspacing=\"0\" style=\"border: dotted 1px gray; width: 450px\">" + 
            					"<tr style=\"background-color: #8EC1FF;\"><td>period</td><td>downloaded</td><td>uploaded</td><td>summary</td></tr>";
            	string footer = "</table></center></body></html>";
            	string row = "<tr style=\"background-color: {0};\"><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>";
            	string row_style1 = "#FFFFFF";
            	string row_style2 = "#F2F2F2";
            	string color;
            	
            	double total_up = 0;
            	double total_down = 0;
            	

            	int i = 0;

            	TextWriter file = new StreamWriter(dialog.FileName);
            	
            	file.Write(header);
            	
            	while (i < log_data.TotalDownloads.Length)
            	{
            		double up = log_data.TotalUploads[i].traffic;
            		double down = log_data.TotalDownloads[i].traffic;
            		
            		
            		if (i % 2 == 0)
            			color = row_style1;
            		else
            			color = row_style2;            			
            		
            		file.Write(String.Format(row, color, log_data.TotalDownloads[i].date, down, up, up + down));
            		total_up += up;
            		total_down += down;
            		i++;
            	}
            	
            	file.Write(String.Format(row, "#8EC1FF","<b>totals</b>", total_down, total_up,  total_down + total_up));
            	file.Write(footer);
				file.Close();
            }
        }

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			limits_check = limitsCheckBox.Checked;
			limits_notified = false;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			limits_period = (TimeSpan)periodComboBox.SelectedIndex;
			limits_notified = false;
		}

		private void textBox3_TextChanged(object sender, EventArgs e)
		{
			if (maxBothBox.Text != "")
			{
				if (!double.TryParse(maxBothBox.Text, out max_both))
					maxBothBox.Text = "";
				else
				{
					limits_notified = false;
					max_both = max_both * 1024 * 1024;
				}
			}
			else
				max_both = 0;
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			if (maxDownloadBox.Text != "")
			{
				if (!double.TryParse(maxDownloadBox.Text, out max_download))
					maxDownloadBox.Text = "";
				else
				{
					limits_notified = false;
					max_download = max_download * 1024 * 1024;
				}
			}
			else
				max_download = 0;
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (maxUploadBox.Text != "")
			{
				if (!double.TryParse(maxUploadBox.Text, out max_upload))
					maxUploadBox.Text = "";
				else
				{
					limits_notified = false;
					max_upload = max_upload * 1024 * 1024;
				}
			}
			else
				max_upload = 0;
		}
	}
}
