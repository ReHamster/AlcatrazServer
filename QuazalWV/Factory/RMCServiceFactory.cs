using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.Factory
{
	static class RMCServiceFactory
	{
		static Dictionary<RMCP.PROTOCOL, Func<RMCServiceBase>> s_FactoryFuncs = new Dictionary<RMCP.PROTOCOL, Func<RMCServiceBase>>();

		public static RMCServiceBase GetServiceInstance(RMCP.PROTOCOL protocolId)
		{
			Func<RMCServiceBase> existingFactory;
			if (s_FactoryFuncs.TryGetValue(protocolId, out existingFactory))
			{
				return existingFactory();
			}

			Assembly asm = Assembly.GetExecutingAssembly();
			var classList = asm.GetTypes()
								  .Where(t => string.Equals(t.Namespace, "QuazalWV.RMCServices", StringComparison.Ordinal))
								  .ToArray();

			// search for new controller
			foreach(var protoClass in classList)
			{
				var protocolAttribute = protoClass.GetCustomAttribute<RMCServiceAttribute>();
				if(protocolAttribute.ProtocolId == protocolId)
				{
					var createFunc = Expression.Lambda<Func<RMCServiceBase>>(
							Expression.New(protoClass.GetConstructor(Type.EmptyTypes))
						).Compile();

					s_FactoryFuncs.Add(protocolId, createFunc);

					return createFunc();
				}
			}

			return null;
		}
	}
}
