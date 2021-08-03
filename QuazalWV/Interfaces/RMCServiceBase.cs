using QuazalWV.Attributes;
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

		protected void SendResponseWithACK(RMCPResponse reply, bool useCompression = true, uint error = 0)
		{
			RMC.SendResponseWithACK(_context.Client.udp, _context.Packet, _context.RMC, _context.Client, reply, useCompression, error);
		}

		protected void UNIMPLEMENTED()
		{
			var stackTrace = new StackTrace();
			var method = stackTrace.GetFrame(1).GetMethod();

			var methodName = method.Name;

			var rmcMethodAttr = (RMCMethodAttribute)method.GetCustomAttributes(typeof(RMCMethodAttribute), true).SingleOrDefault();
			if(rmcMethodAttr != null && !string.IsNullOrWhiteSpace(rmcMethodAttr.Name))
				methodName = rmcMethodAttr.Name;

			Log.WriteLine(1, $"Error: Method '{ methodName }' is unimplemented in '{ _context.RMC.proto }'");
		}
	}
}
