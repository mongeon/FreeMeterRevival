using System;
using System.IO;
using System.Net.Sockets;

namespace FreeMeterRevival
{
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
}
