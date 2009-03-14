using System;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace FreeMeterRevival.Forms
{
    public partial class TraceForm : Form
    {
        public MainForm MyParentForm;
        
        private static Thread tracer;
        
        //private string TraceTarget;
        private bool traceing = false;

        public TraceForm()
        {
            InitializeComponent();
            
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
}
