using QNetZ.Factory;
using RDVServices;
using System;
using System.Linq;
using System.Reflection;

namespace DSFServices
{
	public static class ServiceFactoryDSF
	{
		public static void RegisterDSFServices()
		{
			ServiceFactoryRDV.RegisterRDVServices();

			Assembly asm = Assembly.GetExecutingAssembly();
			var classList = asm.GetTypes()
								  .Where(t => string.Equals(t.Namespace, "DSFServices.Services", StringComparison.Ordinal))
								  .ToArray();

			// search for new controller
			foreach (var protoClass in classList)
			{
				RMCServiceFactory.RegisterService(protoClass);
			}
		}
	}
}
