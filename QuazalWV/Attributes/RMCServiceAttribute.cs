using System;

namespace QuazalWV.Attributes
{
	/// <summary>
	/// RMC class attribute identifying class as a service/protocol hanlder
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class RMCServiceAttribute : Attribute
	{
		public readonly RMCP.PROTOCOL ProtocolId;
		public RMCServiceAttribute(RMCP.PROTOCOL protocolId)
		{
			ProtocolId = protocolId;
		}
	}
}