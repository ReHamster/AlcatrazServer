using QuazalWV.Attributes;
using QuazalWV.DDL.Models;
using QuazalWV.Interfaces;
using System.Collections.Generic;

namespace QuazalWV.Services
{
	[RMCService(RMCProtocolId.DriverUniqueIDService)]
	public class DriverUniqueIDService : RMCServiceBase
	{
		[RMCMethod(2)]
		public RMCResult CreateUniqueID()
		{
			uint uniqueID = 1;

			UNIMPLEMENTED();

			return Result(new { value = uniqueID });
		}
	}
}
