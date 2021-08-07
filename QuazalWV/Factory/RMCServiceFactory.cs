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
		static Dictionary<RMCProtocolId, Func<RMCServiceBase>> s_FactoryFuncs = new Dictionary<RMCProtocolId, Func<RMCServiceBase>>();

		public static Func<RMCServiceBase> GetServiceFactory(RMCProtocolId protocolId)
		{
			Func<RMCServiceBase> existingFactory;
			if (s_FactoryFuncs.TryGetValue(protocolId, out existingFactory))
			{
				return existingFactory;
			}

			Assembly asm = Assembly.GetExecutingAssembly();
			var classList = asm.GetTypes()
								  .Where(t => string.Equals(t.Namespace, "QuazalWV.Services", StringComparison.Ordinal))
								  .ToArray();

			// search for new controller
			foreach (var protoClass in classList)
			{
				var protocolAttribute = protoClass.GetCustomAttribute<RMCServiceAttribute>();

				if (protocolAttribute == null)
					continue;

				if (protocolAttribute.ProtocolId == protocolId)
				{
					var createFunc = Expression.Lambda<Func<RMCServiceBase>>(
							Expression.New(protoClass.GetConstructor(Type.EmptyTypes))
						).Compile();

					s_FactoryFuncs.Add(protocolId, createFunc);

					return createFunc;
				}
			}

			return null;
		}

		public static Type GetServiceType(RMCProtocolId protocolId)
		{
			var factoryFunc = GetServiceFactory(protocolId);
			if (factoryFunc == null)
				return null;

			return factoryFunc.Method.ReturnType;
		}

		public static MethodInfo GetServiceMethodById(Type rmcServiceType, uint methodId)
		{
			MethodInfo bestMethod = null;

			// find suitable method which DO have attribute with ID
			var allMethods = rmcServiceType.GetMethods();
			foreach (var method in allMethods)
			{
				var rmcMethodAttr = (RMCMethodAttribute)method.GetCustomAttributes(typeof(RMCMethodAttribute), true).SingleOrDefault();
				if (rmcMethodAttr != null)
				{
					if (rmcMethodAttr.MethodId == methodId)
					{
						bestMethod = method;
						break;
					}
				}
			}

			return bestMethod;
		}
	}
}
