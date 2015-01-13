using UnityEngine;
using System.Collections.Generic;

public class ArcadeStats
{

	public enum StatKeys
	{
		TotalScore,HighScore,MaxCombo,LongestChain,DelinkersHit,EjectorsHit,FreezersHit,GloopersHit,InfectorsHit,ShockersHit
	}

	public static Dictionary<StatKeys,string> keyNames = new Dictionary<StatKeys, string>
	{
		{StatKeys.HighScore,"ArcadeHighScore"},
		{StatKeys.LongestChain,"ArcadeLongestChain"},
		{StatKeys.TotalScore,"ArcadeScoreTotal"},
		{StatKeys.MaxCombo,"ArcadeMaxCombo"},
		{StatKeys.DelinkersHit,"ArcadeDelinkersHit"},
		{StatKeys.EjectorsHit,"ArcadeEjectorsHit"},
		{StatKeys.FreezersHit,"ArcadeFreezersHit"},
		{StatKeys.GloopersHit,"ArcadeGloopersHit"},
		{StatKeys.InfectorsHit,"ArcadeInfectorsHit"},
		{StatKeys.ShockersHit,"ArcadeShockersHit"}
	};

	static void AddToScoreTotal(float score)
	{
		float total = 0;
		LoadStat<float>(StatKeys.TotalScore,out total);
		total += score;
		SaveStat<float>(StatKeys.TotalScore,total);
	}

	public static float HighScore
	{
		get
		{
			float score = 0;
			LoadStat<float>(StatKeys.HighScore,out score);
			return score;
		}
		set
		{
			float score = 0;
			LoadStat<float>(StatKeys.HighScore,out score);
			if(value > score) {
				SaveStat<float>(StatKeys.HighScore,value);
			}
			AddToScoreTotal(value);
		}
	}

	public static float TotalScore
	{
		get
		{
			float total = 0;
			LoadStat<float>(StatKeys.TotalScore,out total);
			return total;
		}
	}

	public static int LongestChain
	{
		get
		{
			int length = 0;
			LoadStat<int>(StatKeys.LongestChain,out length);
			return length;
		}
		set
		{
			SaveStat<int>(StatKeys.LongestChain,value);
		}
	}

	public static float MaxCombo
	{
		get
		{
			float max = 0;
			LoadStat<float>(StatKeys.MaxCombo, out max);
			return max;
		}
		set
		{
			float max = 0;
			LoadStat<float>(StatKeys.MaxCombo, out max);
			if(value > max)
				SaveStat<float>(StatKeys.MaxCombo,value);
		}
	}

	public static int DelinkersHit
	{
		get
		{
			int num = 0;
			LoadStat<int>(StatKeys.DelinkersHit,out num);
			return num;
		}
		private set
		{
			SaveStat<int>(StatKeys.DelinkersHit,value);
		}
	}

	public static int EjectorsHit
	{
		get
		{
			int num = 0;
			LoadStat<int>(StatKeys.EjectorsHit,out num);
			return num;
		}
		private set
		{
			SaveStat<int>(StatKeys.EjectorsHit,value);
		}
	}

	public static int FreezersHit
	{
		get
		{
			int num = 0;
			LoadStat<int>(StatKeys.FreezersHit,out num);
			return num;
		}
		private set
		{
			SaveStat<int>(StatKeys.FreezersHit,value);
		}
	}

	public static int GloopersHit
	{
		get
		{
			int num = 0;
			LoadStat<int>(StatKeys.GloopersHit,out num);
			return num;
		}
		private set
		{
			SaveStat<int>(StatKeys.GloopersHit,value);
		}
	}

	public static int InfectorsHit
	{
		get
		{
			int num = 0;
			LoadStat<int>(StatKeys.InfectorsHit,out num);
			return num;
		}
		private set
		{
			SaveStat<int>(StatKeys.InfectorsHit,value);
		}
	}

	public static int ShockersHit
	{
		get
		{
			int num = 0;
			LoadStat<int>(StatKeys.ShockersHit,out num);
			return num;
		}
		private set
		{
			SaveStat<int>(StatKeys.ShockersHit,value);
		}
	}



	public static void BadBallHit(BadBall bb)
	{
		System.Type ballType = bb.GetType();
		if(ballType.Equals(typeof(Delinker))) {
			DelinkersHit += 1;
		} else if(ballType.Equals(typeof(Ejector))) {
			EjectorsHit += 1;
		} else if(ballType.Equals(typeof(Freezer))) {
			FreezersHit += 1;
		} else if(ballType.Equals(typeof(Shocker))) {
			ShockersHit += 1;
		} else if(ballType.Equals(typeof(Infector))) {
			InfectorsHit += 1;
		} else if(ballType.Equals(typeof(Glooper))) {
			GloopersHit += 1;
		}
	}

	static void LoadStat<T>(StatKeys key, out T value)
	{
		Util.TryLoadFromPlayerPrefs<T>(keyNames[key],out value);
	}

	static void SaveStat<T>(StatKeys key, T value)
	{
		Util.SaveToPlayerPrefs(keyNames[key],value);
	}

	public static float GetStat(StatKeys key)
	{
		switch(key) {
		case StatKeys.DelinkersHit:
			return DelinkersHit;
		case StatKeys.EjectorsHit:
			return EjectorsHit;
		case StatKeys.FreezersHit:
			return FreezersHit;
		case StatKeys.GloopersHit:
			return GloopersHit;
		case StatKeys.InfectorsHit:
			return InfectorsHit;
		case StatKeys.ShockersHit:
			return ShockersHit;
		case StatKeys.HighScore:
			return HighScore;
		case StatKeys.LongestChain:
			return LongestChain;
		case StatKeys.TotalScore:
			return TotalScore;
		case StatKeys.MaxCombo:
			return MaxCombo;
		default:
			return -1;
		}
	}

	public static void Clear()
	{
		foreach(StatKeys key in System.Enum.GetValues(typeof(StatKeys))) {
			PlayerPrefs.DeleteKey(keyNames[key]);
		}
	}

}

