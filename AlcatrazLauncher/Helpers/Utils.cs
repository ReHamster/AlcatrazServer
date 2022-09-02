using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcatrazLauncher.Helpers
{
	public static class Utils
	{
		public static bool CheckLoginCharacterAllowed(char chr)
		{
			return chr >= '0' && chr <= '9' 
				|| chr >= 'a' && chr <= 'z' 
				|| chr >= 'A' && chr <= 'Z' 
				|| chr == '_';
		}
	}
}
