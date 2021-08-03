using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System.IO;

namespace QuazalWV.RMCServices
{
	/// <summary>
	/// User friends service
	/// </summary>
	[RMCService(RMCP.PROTOCOL.PersistentStoreService)]
	public class PersistentStoreService : RMCServiceBase
	{
		[RMCMethod(1)]
		public void FindByGroup()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(2)]
		public void InsertItem()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)]
		public void RemoveItem()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)]
		public void GetItem(uint group, string strTag)
		{
			var reply = new RMCPacketResponseGetItem();

			var path = Path.Combine(Global.ServerFilesPath, strTag);

			if (File.Exists(path))
			{
				reply.buffer = File.ReadAllBytes(path);
				reply.result = true;
			}

			SendResponseWithACK(reply);
		}

		[RMCMethod(5)]
		public void InsertCustomItem()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(6)]
		public void GetCustomItem()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(7)]
		public void FindItemsBySQLQuery()
		{
			UNIMPLEMENTED();
		}

	}
}
