using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace QuazalWV.RMCServices
{
	public class UplayActionPlatform
	{
		public string m_platformCode { get; set; }
		public bool m_completed { get; set; }
		public string m_specificKey { get; set; }
	}

	public class UplayAction
	{
		public UplayAction()
		{
			m_platforms = new List<UplayActionPlatform>();
		}

		public string m_code { get; set; }
		public string m_name { get; set; }
		public string m_description { get; set; }
		public ushort m_value { get; set; }
		public string m_gameCode { get; set; }

		public IEnumerable<UplayActionPlatform> m_platforms { get; set; }
	}

	/// <summary>
	/// User friends service
	/// </summary>
	[RMCService(RMCP.PROTOCOL.UplayWinService)]
	class UbiWinService : RMCServiceBase
	{
		[RMCMethod(1)]
        public void GetActions()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(2)]
        public void GetActionsCompleted()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)]
        public void GetActionsCount()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)]
        public void GetActionsCompletedCount()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(5)]
        public void GetRewards()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(6)]
        public void GetRewardsPurchased()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(7)]
        public void UplayWelcome(string culture, string platformCode)
		{
			var result = new List<UplayAction>();

			var reply = new RMCPResponseDDL<List<UplayAction>>(result);
			SendResponseWithACK(reply);
		}

		[RMCMethod(8 )]
        public void SetActionCompleted()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(9)]
        public void SetActionsCompleted()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(10)]
        public void GetUserToken()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(11)]
        public void GetVirtualCurrencyUserBalance()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(12)]
		public void GetSectionsByKey()
		{
			UNIMPLEMENTED();
		}

	}
}
