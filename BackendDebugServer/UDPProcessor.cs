using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QNetZ;

namespace BackendDebugServer
{
    public partial class UDPProcessor : Form
    {
        public UDPProcessor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                StringReader sr = new StringReader(rtb1.Text);
                string line;
                List<string> lines = new List<string>();
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line.Replace(" ", ""));
                StringBuilder sb = new StringBuilder();
                foreach (string l in lines)
                {
                    try
                    {
                        byte[] data = makeArray(l.Trim());
                        sb.Append(QLog.MakeDetailedPacketLog(data, false, toolStripButton2.Checked));
                    }
                    catch
                    {
                        sb.AppendLine("Cant process: " + l);
                    }
                }
                rtb1.Text = sb.ToString();
            }
            catch { MessageBox.Show("Error"); }
        }

        private byte[] makeArray(string s)
        {
			s = s.Trim().Replace(" ", "");

			MemoryStream m = new MemoryStream();
            for (int i = 0; i < s.Length / 2; i++)
                m.WriteByte(Convert.ToByte(s.Substring(i * 2, 2), 16));
            return m.ToArray();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(QPacket.MakeChecksum(makeArray(toolStripTextBox1.Text)).ToString("X2"));
            }
            catch
            { }
        }
    }
}
