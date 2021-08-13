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
		public byte typeValue { get; set; }         // VariantType

		public void Assign(StatisticValueVariant newValue)
		{
			switch((VariantType)typeValue)
			{
				case VariantType.int32:
					valueInt32 = newValue.valueInt32;
					break;
				case VariantType.int64:
					valueInt64 = newValue.valueInt64;
					break;
				case VariantType.Float:
				case VariantType.Double:
					valueDouble = newValue.valueDouble;
					break;
				case VariantType.String:
					valueString = newValue.valueString;
					break;
			}
		}

		public void Add(StatisticValueVariant newValue)
		{
			switch ((VariantType)typeValue)
			{
				case VariantType.int32:
					valueInt32 += newValue.valueInt32;
					break;
				case VariantType.int64:
					valueInt64 += newValue.valueInt64;
					break;
				case VariantType.Float:
				case VariantType.Double:
					valueDouble += newValue.valueDouble;
					break;
				case VariantType.String:
					// not supported
					break;
			}
		}

		public void Subtract(StatisticValueVariant newValue)
		{
			switch ((VariantType)typeValue)
			{
				case VariantType.int32:
					valueInt32 -= newValue.valueInt32;
					break;
				case VariantType.int64:
					valueInt64 -= newValue.valueInt64;
					break;
				case VariantType.Float:
				case VariantType.Double:
					valueDouble -= newValue.valueDouble;
					break;
				case VariantType.String:
					// not supported
					break;
			}
		}

		public void ReplaceIfMin(StatisticValueVariant newValue)
		{
			// FIXME: might be incorrect and flipped!
			switch ((VariantType)typeValue)
			{
				case VariantType.int32:
					if(newValue.valueInt32 < valueInt32)
						valueInt32 = newValue.valueInt32;
					break;
				case VariantType.int64:
					if (newValue.valueInt64 < valueInt64)
						valueInt64 = newValue.valueInt64;
					break;
				case VariantType.Float:
				case VariantType.Double:
					if (newValue.valueDouble < valueDouble)
						valueDouble = newValue.valueDouble;
					break;
				case VariantType.String:
					// not supported
					break;
			}
		}

		public void ReplaceIfMax(StatisticValueVariant newValue)
		{
			// FIXME: might be incorrect and flipped!
			switch ((VariantType)typeValue)
			{
				case VariantType.int32:
					if (newValue.valueInt32 > valueInt32)
						valueInt32 = newValue.valueInt32;
					break;
				case VariantType.int64:
					if (newValue.valueInt64 > valueInt64)
						valueInt64 = newValue.valueInt64;
					break;
				case VariantType.Float:
				case VariantType.Double:
					if (newValue.valueDouble > valueDouble)
						valueDouble = newValue.valueDouble;
					break;
				case VariantType.String:
					// not supported
					break;
			}
		}
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
			switch(policy)
			{
				case StatisticPolicy.Overwrite:
					value.Assign(newValue);
					break;
				case StatisticPolicy.Add:
					value.Add(newValue);
					break;
				case StatisticPolicy.Sub:
					value.Subtract(newValue);
					break;
				case StatisticPolicy.ReplaceIfMin:
					value.ReplaceIfMin(newValue);
					break;
				case StatisticPolicy.ReplaceIfMax:
					value.ReplaceIfMax(newValue);
					break;
			}

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
