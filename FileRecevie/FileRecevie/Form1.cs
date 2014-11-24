using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FileRecevie
{
    public partial class Form1 : Form
    {
        Socket server;
        Socket client;
        public Form1()
        {
            InitializeComponent();
            Thread TempThread = new Thread(new ThreadStart(this.StartReceive));
            TempThread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void StartReceive()
        {
            while (true)
            {

                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(GetLocalIp()), 17000);

                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                server.Bind(ipep);

                server.Listen(10);

                client = server.Accept();

                //       IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;

                string SendFileName = System.Text.Encoding.Unicode.GetString(TransferFiles.ReceiveVarData(client));

                string path = Environment.CurrentDirectory+"/.."+"/../pic";
                //     string BagSize = System.Text.Encoding.Unicode.GetString(TransferFiles.ReceiveVarData(client));

                //      int bagCount = int.Parse(System.Text.Encoding.Unicode.GetString(TransferFiles.ReceiveVarData(client)));

                //      string bagLast = System.Text.Encoding.Unicode.GetString(TransferFiles.ReceiveVarData(client));

                string fileaddr = path + "//" + SendFileName;

                FileStream MyFileStream = new FileStream(fileaddr, FileMode.Create, FileAccess.Write);

                //        int SendedCount = 0;

                while (true)
                {
                    byte[] data = TransferFiles.ReceiveVarData(client);
                    if (data.Length == 0)
                    {
                        break;
                    }
                    else
                    {
                        // SendedCount++;
                        MyFileStream.Write(data, 0, data.Length);
                    }
                }
                richTextBox1.AppendText(fileaddr + " Created   \n");
                MyFileStream.Close();
                server.Close();
                client.Close();


            }


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (server != null)
            //{
            //    server.Close();
            //}
            //if (client != null)
            //{
            //    client.Close();
            //}
            System.Environment.Exit(0);



        }
        private string GetLocalIp()
        {
            string strHostIP = "";
            IPHostEntry oIPHost = Dns.Resolve(Environment.MachineName);
            if (oIPHost.AddressList.Length > 0)
                strHostIP = oIPHost.AddressList[0].ToString();
            return strHostIP;

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }
    }
}
