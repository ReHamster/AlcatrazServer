using QuazalWV.Attributes;
using QuazalWV.DDL.Models;
using QuazalWV.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.Services
{
	[RMCService(RMCProtocolId.DriverG2WService)]
	public class DriverG2WService : RMCServiceBase
	{
		[RMCMethod(3)]
		public RMCResult UnlockG2W(List<UnlockInputData> unlocksIds, string table)
		{
			return Error(0);
		}

		[RMCMethod(4)]
		public RMCResult UploadedReplay(string replayid, string title, string description)
		{
			return Error(0);
		}
	}
}
