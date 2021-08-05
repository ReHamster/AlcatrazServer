using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.Services
{
	[RMCService(RMCProtocol.LocalizationService)]
	public class LocalizationService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult GetLocaleCode()
		{
			return Result("en-US");
		}

		[RMCMethod(2)]
		public void SetLocaleCode(string localeCode)
		{
			SendResponseWithACK(new RMCPResponseEmpty());
		}
	}
}
