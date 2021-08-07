﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuazalWV;

namespace GROBackendWV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Log.ClearLog();
            Log.box = richTextBox1;
            DBHelper.Init();
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
                    Log.MinPriority = 1;
                    break;
                case 1:
                    Log.MinPriority = 2;
                    break;
                case 2:
                    Log.MinPriority = 5;
                    break;
                case 3:
                    Log.MinPriority = 10;
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
            Log.enablePacketLogging = toolStripButton9.Checked;
        }
    }
}
