using QNetZ.DDL;

namespace RDVServices.DDL.Models
{
	public class RegisterResult
	{
		public uint retval { get; set; }
		public uint pidConnectionID { get; set; }
		public StationURL urlPublic { get; set; }
	}
}