using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.DriverUniqueIDService)]
	public class DriverUniqueIDService : RMCServiceBase
	{
		[RMCMethod(2)]
		public RMCResult CreateUniqueID()
		{
			uint uniqueID = 26434;

			UNIMPLEMENTED();

			// return 0E 00 00 00 76 01 5A 00 00 00 02 80 00 00 42 67 00 00 

			return Result(new { value = uniqueID });
		}
	}
}
