using DSFServices;
using QNetZ;
using RDVServices;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BackendDebugServer
{
	public partial class Form1 : Form
    {
		int oldPlayerCount = 0;

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

			dataGridView1.AutoGenerateColumns = false;

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
            QConfiguration.Instance = QConfiguration.MakeDevelopmentConfiguration(serverBindAddress.Text);

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
			NetworkPlayers.PurgeAllPlayers();

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
                    QLog.MinPriority = 4;
                    break;
                case 3:
                    QLog.MinPriority = 5;
                    break;
                case 4:
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
			var bs = (BindingSource)dataGridView1.DataSource;

			//if(oldPlayerCount != NetworkPlayers.Players.Count)
			{
				if(bs == null)
				{
					bs = new BindingSource();
					dataGridView1.DataSource = bs;
				}
				
				bs.DataSource = NetworkPlayers.Players.Select(x => new {
					PID = x.PID,
					Name = x.Name,
					RVCID = x.RVCID,
					PartyId = x.GameData().CurrentGatheringId,
					SesID = x.GameData().CurrentSession != null ? x.GameData().CurrentSession.m_sessionID : uint.MaxValue
				});
			}
			oldPlayerCount = NetworkPlayers.Players.Count;
		}

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            QLog.EnablePacketLogging = toolStripButton9.Checked;
        }
    }
}
