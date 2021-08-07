using QuazalWV;
using QuazalWV.DDL.Models;
using System;
using System.Windows.Forms;

namespace GROBackendWV
{
	public partial class SendNotification : Form
    {
        public SendNotification()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
			var packetHandler = BackendServicesServer.packetHandler;

			try
            {
				foreach (var client in packetHandler.Clients)
                {
					var evtData = new NotificationEvent((NotificationType)Convert.ToUInt32(txt_type.Text), Convert.ToUInt32(txt_subType.Text))
					{
						m_pidSource = Convert.ToUInt32(txt_sourcePID.Text),
						m_uiParam1 = Convert.ToUInt32(txt_param1.Text),
						m_uiParam2 = Convert.ToUInt32(txt_param2.Text),
						m_strParam = txt_stringParam.Text,
						m_uiParam3 = Convert.ToUInt32(txt_param3.Text)
					};

                    NotificationQueue.AddNotification(evtData, client, 0);
                }

            }
            catch { }
        }
    }
}
