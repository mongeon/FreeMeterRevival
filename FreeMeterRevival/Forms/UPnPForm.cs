using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;

namespace FreeMeterRevival.Forms
{
    public partial class UPnPForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public MainForm MyParentForm;

        private _UPnPNat nat = new _UPnPNat();

        public UPnPForm()
        {
            InitializeComponent();
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
                        PortMappingInfo pmi = new PortMappingInfo("FreeMeterRevival", comboBox1.SelectedItem.ToString(), ret.ToString(), int.Parse(port.Text), null, int.Parse(port.Text), true);
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
                    PortMappingInfo pmi = new PortMappingInfo("FreeMeterRevival", comboBox1.SelectedItem.ToString(), null, int.Parse(port.Text), null, int.Parse(port.Text), true);
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
}
