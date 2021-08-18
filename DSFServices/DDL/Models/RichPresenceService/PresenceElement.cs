using QNetZ.DDL;

namespace DSFServices.DDL.Models
{
	public class PresenceElement
	{
		public uint principalId { get; set; }
		public bool isConnected { get; set; }
		public int phraseId { get; set; }
		public qBuffer argument { get; set; }
	}
}
