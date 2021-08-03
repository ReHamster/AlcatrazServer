using QuazalWV.Attributes;
using QuazalWV.Interfaces;

namespace QuazalWV.RMCServices
{
	/// <summary>
	/// Hermes Game Info protocol
	/// </summary>
	[RMCService(RMCP.PROTOCOL.GameInfoService)]
	class GameInfoService : RMCServiceBase
	{
		[RMCMethod(5)]
		public void GetFileInfoList(uint indexStart, uint numElements, string stringSearch)
		{
			var reply = new RMCPacketResponseGetFileList();

			// OnlineConfig is requested
			reply.fileList.Add(new PersistentInfo {
				m_name = "OnlineConfig.ini", 
				m_size = 8574
			});

			SendResponseWithACK(reply);
		}
	}
}
