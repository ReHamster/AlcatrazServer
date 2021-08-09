namespace DSFServices.DDL.Models
{
	public class PresenceElement
	{
		public uint principalId { get; set; }
		public bool isConnected { get; set; }
		public int phraseId { get; set; }
		public byte[] argument { get; set; }
	}
}
