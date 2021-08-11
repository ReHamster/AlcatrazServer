using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using RDVServices.DDL.Models;
using System.Collections.Generic;

namespace RDVServices.Services
{
	[RMCService(QNetZ.RMCProtocolId.NATTraversalService)]
	public class NATTraversalService : RMCServiceBase
	{
		[RMCMethod(1)] 
		public RMCResult RequestProbeInitiation(IEnumerable<StationURL> urlTargetList)
		{
			UNIMPLEMENTED();
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
