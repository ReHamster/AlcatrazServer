using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
	public class StatisticData
	{
		public StatisticData()
		{

		}
		public int boardId { get; set; }
		public IEnumerable<int> propertyIds { get; set; }
	}

	public class StatisticValueVariant
	{
		public StatisticValueVariant()
		{

		}

		public int valueInt32 { get; set; }
		public long valueInt64 { get; set; }
		public double valueDouble { get; set; }
		public string valueString { get; set; }
		public byte typeValue { get; set; }
	}

	public class StatisticWriteValue
	{
		public StatisticWriteValue()
		{

		}
		public byte propertyId { get; set; }
		public StatisticValueVariant value { get; set; }
		public byte writePolicy { get; set; }
		public byte friendComparison { get; set; }
	}

	public class StatisticWriteWithBoard
	{
		public StatisticWriteWithBoard()
		{

		}
		public int boardId { get; set; }
		public IEnumerable<StatisticWriteValue> statisticList { get; set; }
	}

	public class StatisticReadValue
	{
		public StatisticReadValue()
		{

		}
		public byte propertyId { get; set; }
		public StatisticValueVariant value { get; set; }
		public byte rankingCriterionIndex { get; set; }
		public StatisticValueVariant sliceScore { get; set; }
		public StatisticValueVariant scoreLostForNextSlice { get; set; }
	}

	public class StatisticReadValueByBoard
	{
		public StatisticReadValueByBoard()
		{

		}
		public int boardId { get; set; }
		public int rank { get; set; }
		public float score { get; set; }
		public IEnumerable<StatisticReadValue> scores { get; set; }
		public DateTime lastUpdate { get; set; }
	}

	public class ScoreListRead
	{
		public ScoreListRead()
		{

		}
		public uint pid { get; set; }
		public string pname { get; set; }
		public IEnumerable<StatisticReadValueByBoard> scoresByBoard { get; set; }
	}

	public class LeaderboardData
	{
		public LeaderboardData()
		{

		}
		public int boardId { get; set; }
		public int columnId { get; set; }
	}
}
