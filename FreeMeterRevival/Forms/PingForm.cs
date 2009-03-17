using System;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FreeMeterRevival.Forms
{
    public partial class PingForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public MainForm MyParentForm;
        
        private static Thread pinger;

        private int sentcount = 0, recvcount = 0;
        private ArrayList times = new ArrayList();
        private string PingTarget;
        private bool pinging = false;

        public PingForm()
        {
            InitializeComponent();
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
}
