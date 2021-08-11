using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using RDVServices.DDL.Models;
using System.Collections.Generic;
using System.Net;

namespace RDVServices.Services
{
	[RMCService(RMCProtocolId.NATTraversalService)]
	public class NATTraversalService : RMCServiceBase
	{
		[RMCMethod(1)] 
		public RMCResult RequestProbeInitiation(IEnumerable<StationURL> urlTargetList)
		{
			UNIMPLEMENTED();

			// IDK how it works...
			foreach(var urlTarget in urlTargetList)
			{
				var endp = new IPEndPoint(IPAddress.Parse(urlTarget.Address), urlTarget.Parameters["port"]);
				var qclient = Context.Handler.GetQClientByEndPoint(endp);

				// send InitiateProbe
				if(qclient != null)
					SendRMCCall(qclient, RMCProtocolId.NATTraversalService, 2, urlTarget);
			}

			return Error(0);
		}

		[RMCMethod(2)] 
		public RMCResult InitiateProbe(StationURL urlStationToProbe)
		{
			UNIMPLEMENTED();
			return Error(0);
		}

		[RMCMethod(3)] 
		public RMCResult RequestProbeInitiationExt(IEnumerable<StationURL> urlTargetList, StationURL urlStationToProbe)
		{
			UNIMPLEMENTED();
			return Error(0);
		}

		[RMCMethod(4)] 
		public RMCResult ReportNATTraversalResult(uint cid, bool result)
		{
			UNIMPLEMENTED();
			return Error(0);
		}

		[RMCMethod(5)] 
		public RMCResult ReportNATProperties(uint natmapping, uint natfiltering, uint rtt)
		{
			UNIMPLEMENTED();
			return Error(0);
		}

		[RMCMethod(6)] 
		public RMCResult GetRelaySignatureKey()
		{
			UNIMPLEMENTED();
			return Result(new RelaySignatureKey());
		}

		[RMCMethod(7)] 
		public RMCResult ReportNATTraversalResultDetail(uint cid, bool result, int detail, uint rtt)
		{
			UNIMPLEMENTED();
			return Error(0);
		}

	}
}
