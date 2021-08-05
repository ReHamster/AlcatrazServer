using System;

namespace QuazalWV.Attributes
{
	/// <summary>
	/// RMC class attribute identifying class as a service/protocol hanlder
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class RMCServiceAttribute : Attribute
	{
		public readonly RMCProtocol ProtocolId;
		public RMCServiceAttribute(RMCProtocol protocolId)
		{
			ProtocolId = protocolId;
		}
	}
}