using DSFServices;
using QNetZ;
using RDVServices;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BackendDebugServer
{
	public partial class Form1 : Form
    {
		private void LogPrintFunc(int priority, string s, Color color)
		{
			var box = richTextBox1;

			box.Invoke(new Action(delegate
			{
				box.SelectionStart = box.TextLength;
				box.SelectionLength = 0;
				box.SelectionColor = color;

				box.AppendText(s + "\n");
				box.SelectionColor = box.ForeColor;
				box.ScrollToCaret();
			}));
		}

		public Form1()
        {
            InitializeComponent();

			QLog.ClearLog();
			QLog.LogFunction = (int priority, string s, Color color) =>
			{
				LogPrintFunc(priority, s, color);
			};

            toolStripComboBox1.SelectedIndex = 0;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;

			TCPServer.Start();
            BackendServicesServer.Start();
            RDVServer.Start();           
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

			TCPServer.Stop();
            BackendServicesServer.Stop();
            RDVServer.Stop();
			Global.PurgeAllPlayers();

			toolStripButton1.Enabled = true;
            toolStripButton2.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            TCPServer.Stop();
            BackendServicesServer.Stop();
            RDVServer.Stop();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            new DecryptTool().Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            new LogFilter().Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboBox1.SelectedIndex)
            {
                default:
                case 0:
                    QLog.MinPriority = 1;
                    break;
                case 1:
                    QLog.MinPriority = 2;
                    break;
                case 2:
                    QLog.MinPriority = 5;
                    break;
                case 3:
                    QLog.MinPriority = 10;
                    break;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            new PacketGenerator().Show();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            new UDPProcessor().Show();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            new SendNotification().Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NotificationQueue.Update(BackendServicesServer.packetHandler);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            QLog.EnablePacketLogging = toolStripButton9.Checked;
        }
    }
}
