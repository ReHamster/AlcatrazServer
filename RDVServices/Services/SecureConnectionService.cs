using RDVServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RDVServices.Services
{
	/// <summary>
	/// Secure connection service protocol
	/// </summary>
	[RMCService(RMCProtocolId.SecureConnectionService)]
	public class SecureConnectionService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult Register(List<string> vecMyURLs)
		{
			// change address
			var rdvConnectionUrl = new StationURL(vecMyURLs.Last().ToString());
			rdvConnectionUrl.Address = Context.Client.endpoint.Address.ToString();
			rdvConnectionUrl.Parameters["type"] = 3;

			var result = new RegisterResult()
			{
				pidConnectionID = Context.Client.info.RVCID,
				retval = (int)RMCErrorCode.Core_NoError,
				urlPublic = rdvConnectionUrl
			};

			return Result(result);
		}

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
		public RMCResult RegisterEx(ICollection<StationURL> vecMyURLs, AnyData<UbiAuthenticationLoginCustomData> hCustomData)
		{
			if(hCustomData.data != null)
			{
				// change address
				var rdvConnectionUrl = new StationURL(vecMyURLs.Last().ToString());
				rdvConnectionUrl.Address = Context.Client.endpoint.Address.ToString();
				rdvConnectionUrl.Parameters["type"] = 3;

				var result = new RegisterResult()
				{
					pidConnectionID = Context.Client.info.RVCID,
					retval = (int)RMCErrorCode.Core_NoError,
					urlPublic = rdvConnectionUrl
				};

				return Result(result);
			}
			else
			{
				QLog.WriteLine(1, $"[RMC Secure] Error: Unknown Custom Data class {hCustomData.className}");
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
