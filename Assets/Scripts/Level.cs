using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Level : ScriptableObject
{

	public string displayName;
	public string sceneName;
	public int difficulty;
	public Goal[] possibleGoals;

	public static int maxDifficulty = 3;

	public int GoalUnlockedIndex
	{
		get
		{
			return PlayerPrefs.GetInt(name + "GoalUnlocked",0);
		}
		set
		{
			PlayerPrefs.SetInt(name + "GoalUnlocked",value);
		}
	}

	public void SetHighScore(Goal goal, int diff, float score)
	{
		HighScores.SetScore(this,goal,diff,score);
	}
	



}

