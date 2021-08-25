using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.DriverUniqueIDService)]
	public class DriverUniqueIDService : RMCServiceBase
	{
		static uint UniqueIDCounter = 26434;

		[RMCMethod(2)]
		public RMCResult CreateUniqueID()
		{
			return Result(new { value = ++UniqueIDCounter });
		}
	}
}
