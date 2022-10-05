using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace RDVServices.Services
{
	[RMCService(RMCProtocolId.RemoteLogDeviceService)]
	public class RemoteLogDeviceService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult Log(string strLine)
		{
			QLog.WriteLine(1, $"Recieved from PID={Context.Client.Info.PID}: {strLine}");
			return Error(0);
		}
	}
}
