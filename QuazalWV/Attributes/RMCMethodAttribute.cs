using System;

namespace QuazalWV.Attributes
{
	/// <summary>
	/// RMC method attribute for identifying function as a method handler
	/// Quazal::Protocol::AddMethodID
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class RMCMethodAttribute : Attribute
	{
		public readonly int MethodId;
		public readonly string Name;

		public RMCMethodAttribute(int methodId, string name = null)
		{
			MethodId = methodId;
			Name = name;
		}
	}
}