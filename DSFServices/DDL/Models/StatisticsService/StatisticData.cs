using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSFServices.DDL.Models
{
	public enum VariantType
	{
		None = 0,
		int32 = 1,
		int64 = 2,
		Float = 3,
		Double = 4,
		String = 5,
	}
	public enum StatisticPolicy
	{
		Add = 0,
		Sub = 1,
		Overwrite = 2,
		ReplaceIfMin = 3,
		ReplaceIfMax = 4,
	};

	public enum RankingOrder
	{
		Ascending = 0,
		Descending = 1,
	}

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
			typeValue = (byte)VariantType.None;
			valueString = "";
		}

		public int valueInt32 { get; set; }
		public long valueInt64 { get; set; }
		public double valueDouble { get; set; }
		public string valueString { get; set; }
		public byte typeValue { get; set; }			// VariantType
	}

	public class StatisticWriteValue
	{
		public StatisticWriteValue()
		{
		}

		public byte propertyId { get; set; }
		public StatisticValueVariant value { get; set; }
		public byte writePolicy { get; set; }       // StatisticPolicy
		public bool friendComparison { get; set; }
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

		public StatisticReadValue(byte _propertyId, StatisticsBoardValue boardValue)
		{
			propertyId = _propertyId;
			value = boardValue.value;
			rankingCriterionIndex = boardValue.rankingCriterionIndex;
			sliceScore = boardValue.sliceScore;
			scoreLostForNextSlice = boardValue.scoreLostForNextSlice;
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
			scores = new List<StatisticReadValue>();
		}

		public int boardId { get; set; }
		public int rank { get; set; }
		public float score { get; set; }
		public ICollection<StatisticReadValue> scores { get; set; }
		public DateTime lastUpdate { get; set; }
	}

	public class ScoreListRead
	{
		public ScoreListRead()
		{
			scoresByBoard = new List<StatisticReadValueByBoard>();
		}

		public uint pid { get; set; }
		public string pname { get; set; }
		public ICollection<StatisticReadValueByBoard> scoresByBoard { get; set; }
	}

	public class LeaderboardData
	{
		public LeaderboardData()
		{

		}

		public int boardId { get; set; }
		public int columnId { get; set; }
	}

	//---------------------------------------------------------------
	// Database stuff

	public class StatisticsBoardValue
	{
		public StatisticsBoardValue()
		{
			value = new StatisticValueVariant();
			sliceScore = new StatisticValueVariant();
			scoreLostForNextSlice = new StatisticValueVariant();
			rankingCriterionIndex = 1;
		}

		public StatisticsBoardValue(StatisticDesc desc) : this()
		{
			value.typeValue = sliceScore.typeValue = scoreLostForNextSlice.typeValue = (byte)desc.statType;
		}

		public StatisticsBoardValue(StatisticValueVariant initialValue) : this()
		{
			value = initialValue;
		}

		public void UpdateValueWithPolicy(StatisticValueVariant newValue, StatisticPolicy policy)
		{
			value = newValue;

			// scoreLostForNextSlice is diff?
			// sliceScore hmmm?
		}

		public StatisticValueVariant value { get; set; }
		public byte rankingCriterionIndex { get; set; }
		public StatisticValueVariant sliceScore { get; set; }
		public StatisticValueVariant scoreLostForNextSlice { get; set; }
	}

	/*
	public class StatisticsBoard
	{
		public StatisticsBoard()
		{
			properties = new Dictionary<int, StatisticsBoardValue>();
			rank = 0;
			score = 0.0f;
			lastUpdate = DateTime.UtcNow;
		}

		public uint playerId { get; set; }
		public int boardId { get; set; }
		public int rank { get; set; }
		public float score { get; set; }
		public DateTime lastUpdate { get; set; }

		public IDictionary<int, StatisticsBoardValue> properties { get; set; }		// by propertyId
	}*/

}
