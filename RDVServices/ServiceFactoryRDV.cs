using QNetZ.Factory;
using System;
using System.Linq;
using System.Reflection;

namespace RDVServices
{
	public static class ServiceFactoryRDV
	{
		public static void RegisterRDVServices()
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			var classList = asm.GetTypes()
								  .Where(t => string.Equals(t.Namespace, "RDVServices.Services", StringComparison.Ordinal))
								  .ToArray();

			// search for new controller
			foreach (var protoClass in classList)
			{
				RMCServiceFactory.RegisterService(protoClass);
			}
		}
	}
}
