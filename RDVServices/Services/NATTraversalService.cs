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

		/*
			EXAMPLE: 
			My client adddress: 192.168.100.3
			I'm Joining party.

			My client sends next: NATTraversalService.RequestProbeInitiation:
				{
					"Valid": true,
					"UrlScheme": "prudp",
					"Address": "192.168.100.10",
					"Parameters": {
					"port": 3074,
					"RVCID": 768398
					},
					"urlString": "prudp:/address=192.168.100.10;port=3074;RVCID=768398"
				}

			SO Server :
				Send NATTraversalService.InitiateProbe to (192.168.100.10) with (urlStationToProbe = 192.168.100.3)
					OR
				Send NATTraversalService.InitiateProbe to (192.168.100.3) with (urlStationToProbe = 192.168.100.10) ???
			
		*/
		public RMCResult RequestProbeInitiation(IEnumerable<StationURL> urlTargetList)
		{
			// urlTargetList contains all player urls (basicmcnally given by MatchMakingService.GetSessionURLs)
			// Server sends InitiateProbe to all players in that url with those URLs
			// Then clients communicate with each other...
			foreach (var urlTarget in urlTargetList)
			{
				var endp = new IPEndPoint(IPAddress.Parse(urlTarget.Address), urlTarget.Parameters["port"]);
				var qclient = Context.Handler.GetQClientByEndPoint(endp);

				// FIXME: I suspect that this is valid but who knows
				if(qclient != null)
				{
					var thisClientURL = new StationURL(
						"prudp",
						Context.Client.endpoint.Address.ToString(), 
						new Dictionary<string, int>() {
							{ "port", Context.Client.endpoint.Port },
							{ "RVCID", (int)Context.Client.info.RVCID }
						});

					SendRMCCall(qclient, RMCProtocolId.NATTraversalService, 2, thisClientURL);
				}
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
