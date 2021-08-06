using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using QuazalWV.DDL.Models;
using System.Collections.Generic;
using QuazalWV.DDL;

namespace QuazalWV.Services
{
	/// <summary>
	/// Secure connection service protocol
	/// </summary>
	[RMCService(RMCProtocolId.SecureConnectionService)]
	public class SecureConnectionService : RMCServiceBase
	{
		[RMCMethod(2)]
		public void RequestConnectionData()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)]
		public void RequestUrls()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)]
		public RMCResult RegisterEx(IEnumerable<string> stationUrls, AnyData<UbiAuthenticationLoginCustomData> hCustomData)
		{
			if(hCustomData.data != null)
			{
				var rdvConnectionString = $"prudps:/address={ Global.serverBindAddress };port={ Global.serverBindPort };CID=1;PID={Context.Client.info.PID};sid=1;stream=3;type=2";

				var result = new RegisterResult()
				{
					pidConnectionID = 78,
					retval = (int)RMCErrorCode.Core_NoError,
					urlPublic = rdvConnectionString
				};

				return Result(result);
			}
			else
			{
				Log.WriteLine(1, $"[RMC Secure] Error: Unknown Custom Data class {hCustomData.className}");
			}

			return Error((int)RMCErrorCode.RendezVous_ClassNotFound);
		}

		[RMCMethod(5)]
		public void TestConnectivity()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(6)]
		public void UpdateURLs()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(7)]
		public void ReplaceURL()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(8)]
		public void SendReport()
		{
			UNIMPLEMENTED();
		}
	}
}
