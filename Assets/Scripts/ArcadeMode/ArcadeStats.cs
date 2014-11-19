using UnityEngine;
using System.Collections;

public class ArcadeStats
{

	static void AddToScoreTotal(float score)
	{
		float total = 0;
		Util.TryLoadFromPlayerPrefs<float>("ArcadeScoreTotal",out total);
		total += score;
		Util.SaveToPlayerPrefs("ArcadeScoreTotal",total);
	}

	public static float HighScore
	{
		get
		{
			float score = 0;
			Util.TryLoadFromPlayerPrefs("ArcadeHighScore",out score);
			return score;
		}
		set
		{
			float score = 0;
			Util.TryLoadFromPlayerPrefs<float>("ArcadeHighScore",out score);
			if(value > score) {
				Util.SaveToPlayerPrefs<float>("ArcadeHighScore",value);
			}
			AddToScoreTotal(value);
		}
	}

	public static float TotalScore
	{
		get
		{
			float total = 0;
			Util.TryLoadFromPlayerPrefs("ArcadeTotalScore",out total);
			return total;
		}
	}

}

