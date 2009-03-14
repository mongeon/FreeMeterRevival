using System;
using System.IO;
using System.Net.Sockets;

namespace FreeMeterRevival
{

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

}
