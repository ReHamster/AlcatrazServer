using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace DSFServices.Services
{
	/// <summary>
	/// Uplay achievements service
	/// </summary>
	[RMCService(RMCProtocolId.UplayWinService)]
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
		public RMCResult GetRewardsPurchased(int startRowIndex, int maximumRows, string sortExpression, string cultureName, string platformCode)
		{
			var rewards = new List<UPlayReward>()
			{
				new UPlayReward() // uselsess but we still adding it
                {
					m_code = "DRV5REWARD01PC",
					m_name = "Exclusive Wallpaper",
					m_description = "Download the DRIVER San Francisco Wallpaper.",
					m_rewardTypeName = "Downloadable",
					m_gameCode = "DRV5",
					m_value = 0,
					m_platforms = new List<UPlayRewardPlatform>()
                    {
						new UPlayRewardPlatform()
                        {
							m_platformCode = platformCode,
							m_purchased = true
                        }
					}
				},
				new UPlayReward()
                {
					m_code = "DRV5REWARD02",
					m_name = "Tanner's Day Off Challenge",
					m_description = "Tear through Russian Hill in Tannerâ\u0080\u0099s iconic Dodge Challenger.",
					m_rewardTypeName = "Unlockable",
					m_gameCode = "DRV5",
					m_value = 20,
					m_platforms = new List<UPlayRewardPlatform>()
					{
						new UPlayRewardPlatform()
						{
							m_platformCode = platformCode,
							m_purchased = true
						}
					}
				},
				new UPlayReward()
				{
					m_code = "DRV5REWARD03",
					m_name = "Dodge Charger SRT8 Police Car",
					m_description = "Unlocks the Dodge Charger SRT8 Police Car for use in Online games.",
					m_rewardTypeName = "Unlockable",
					m_gameCode = "DRV5",
					m_value = 30,
					m_platforms = new List<UPlayRewardPlatform>()
					{
						new UPlayRewardPlatform()
						{
							m_platformCode = platformCode,
							m_purchased = true
						}
					}
				},
				new UPlayReward()
				{
					m_code = "DRV5REWARD04",
					m_name = "San Francisco Challenges",
					m_description = "Four Challenges that showcase different areas of San Francisco.",
					m_rewardTypeName = "Unlockable",
					m_gameCode = "DRV5",
					m_value = 40,
					m_platforms = new List<UPlayRewardPlatform>()
					{
						new UPlayRewardPlatform()
						{
							m_platformCode = platformCode,
							m_purchased = true
						}
					}
				},
			};

			// return 
			return Result(rewards);
		}

		[RMCMethod(7)]
		public RMCResult UplayWelcome(string culture, string platformCode)
        {
            var result = new List<UplayAction>();
			return Result(result);
		}

		[RMCMethod(8)]
		public void SetActionCompleted()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(9)]
		public RMCResult SetActionsCompleted(IEnumerable<string> actionCodeList, string cultureName, string platformCode)
		{
			var actionList = new List<UplayAction>();
			UNIMPLEMENTED();
			return Result(actionList);
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
