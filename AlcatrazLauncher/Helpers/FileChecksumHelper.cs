using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AlcatrazLauncher.Helpers
{
	public static class FileChecksumHelper
	{
		public static string GetFileSHA1(string FileName)
		{
			using (FileStream fs = new FileStream(FileName, FileMode.Open))
			using (BufferedStream bs = new BufferedStream(fs))
			{
				using (SHA1Managed sha1 = new SHA1Managed())
				{
					byte[] hash = sha1.ComputeHash(bs);
					StringBuilder formatted = new StringBuilder(2 * hash.Length);
					foreach (byte b in hash)
					{
						formatted.AppendFormat("{0:X2}", b);
					}

					return formatted.ToString();
				}
			}
		}
	}
}
