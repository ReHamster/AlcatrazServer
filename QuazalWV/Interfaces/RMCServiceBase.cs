using QuazalWV.Attributes;
using QuazalWV.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.Interfaces
{
	public class RMCServiceBase
	{
		RMCContext _context;

		public RMCContext Context {
			get { return _context; }
			set {
				// make it so user won't override it
				if(_context == null)
					_context = value;
			} 
		}

		public MethodInfo GetServiceMethodById(uint methodId)
		{
			return RMCServiceFactory.GetServiceMethodById(GetType(), methodId);
		}

		protected void SendResponseWithACK(RMCPResponse reply, bool useCompression = true, uint error = 0)
		{
			RMC.SendResponseWithACK(_context.Handler, _context.Packet, _context.RMC, _context.Client, reply, useCompression, error);
		}

		protected RMCResult Result<T>(T reply) where T: class
		{
			return new RMCResult(new RMCPResponseDDL<T>(reply));
		}

		protected RMCResult Error(uint code)
		{
			return new RMCResult(new RMCPResponseEmpty(), true, code);
		}

		// This is for reverse-engineering
		protected void UNIMPLEMENTED(string additionalMessage = "")
		{
			var stackTrace = new StackTrace();
			var method = stackTrace.GetFrame(1).GetMethod();

			var methodName = method.Name;

			var rmcMethodAttr = (RMCMethodAttribute)method.GetCustomAttributes(typeof(RMCMethodAttribute), true).SingleOrDefault();
			if(rmcMethodAttr != null && !string.IsNullOrWhiteSpace(rmcMethodAttr.Name))
				methodName = rmcMethodAttr.Name;

			Log.WriteLine(1, $"[RMC] Error: Method '{ _context.RMC.proto }.{ methodName }' is unimplemented");

			if(!string.IsNullOrWhiteSpace(additionalMessage))
				Log.WriteLine(1, $"[RMC] error info: { additionalMessage } ");
		}
	}
}
