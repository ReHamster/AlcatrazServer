using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.LocalizationService)]
	public class LocalizationService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult GetLocaleCode()
		{
			return Result("en-US");
		}

		[RMCMethod(2)]
		public RMCResult SetLocaleCode(string localeCode)
		{
			return Error(0);
		}
	}
}
