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
			// return qList Quazal::qList<Quazal::UPlayReward>* rewardList

			var rewardListData = "3D 02 00 00 78 01 26 00 00 00 06 80 00 00 04 00 00 00 0F 00 44 52 56 35 52 45 57 41 52 44 30 31 50 43 00 14 00 45 78 63 6C 75 73 69 76 65 20 57 61 6C 6C 70 61 70 65 72 00 2D 00 44 6F 77 6E 6C 6F 61 64 20 74 68 65 20 44 52 49 56 45 52 20 53 61 6E 20 46 72 61 6E 63 69 73 63 6F 20 57 61 6C 6C 70 61 70 65 72 2E 00 00 00 00 00 0D 00 44 6F 77 6E 6C 6F 61 64 61 62 6C 65 00 05 00 44 52 56 35 00 01 00 00 00 03 00 50 43 00 01 0D 00 44 52 56 35 52 45 57 41 52 44 30 32 00 1B 00 54 61 6E 6E 65 72 27 73 20 44 61 79 20 4F 66 66 20 43 68 61 6C 6C 65 6E 67 65 00 41 00 54 65 61 72 20 74 68 72 6F 75 67 68 20 52 75 73 73 69 61 6E 20 48 69 6C 6C 20 69 6E 20 54 61 6E 6E 65 72 E2 80 99 73 20 69 63 6F 6E 69 63 20 44 6F 64 67 65 20 43 68 61 6C 6C 65 6E 67 65 72 2E 00 14 00 00 00 0B 00 55 6E 6C 6F 63 6B 61 62 6C 65 00 05 00 44 52 56 35 00 01 00 00 00 03 00 50 43 00 01 0D 00 44 52 56 35 52 45 57 41 52 44 30 33 00 1E 00 44 6F 64 67 65 20 43 68 61 72 67 65 72 20 53 52 54 38 20 50 6F 6C 69 63 65 20 43 61 72 00 43 00 55 6E 6C 6F 63 6B 73 20 74 68 65 20 44 6F 64 67 65 20 43 68 61 72 67 65 72 20 53 52 54 38 20 50 6F 6C 69 63 65 20 43 61 72 20 66 6F 72 20 75 73 65 20 69 6E 20 4F 6E 6C 69 6E 65 20 67 61 6D 65 73 2E 00 1E 00 00 00 0B 00 55 6E 6C 6F 63 6B 61 62 6C 65 00 05 00 44 52 56 35 00 01 00 00 00 03 00 50 43 00 01 0D 00 44 52 56 35 52 45 57 41 52 44 30 34 00 19 00 53 61 6E 20 46 72 61 6E 63 69 73 63 6F 20 43 68 61 6C 6C 65 6E 67 65 73 00 40 00 46 6F 75 72 20 43 68 61 6C 6C 65 6E 67 65 73 20 74 68 61 74 20 73 68 6F 77 63 61 73 65 20 64 69 66 66 65 72 65 6E 74 20 61 72 65 61 73 20 6F 66 20 53 61 6E 20 46 72 61 6E 63 69 73 63 6F 2E 00 28 00 00 00 0B 00 55 6E 6C 6F 63 6B 61 62 6C 65 00 05 00 44 52 56 35 00 01 00 00 00 03 00 50 43 00 01 ";

			var stream = new MemoryStream(Helper.ParseByteArray(rewardListData));
			var rewardList = DDLSerializer.ReadObject<List<UPlayReward>>(stream);

			// return 
			return Result(rewardList);
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
