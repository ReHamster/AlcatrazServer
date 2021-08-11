using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.DriverUniqueIDService)]
	public class DriverUniqueIDService : RMCServiceBase
	{
		static uint UniqueIDCounter = 26434; // FIXME: is it too simplistic?

		[RMCMethod(2)]
		public RMCResult CreateUniqueID()
		{
			return Result(new { value = ++UniqueIDCounter });
		}
	}
}
