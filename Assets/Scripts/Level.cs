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


	public void SetHighScore(Goal goal, int diff, float score)
	{
		HighScores.SetScore(this,goal,diff,score);
	}

	/*
	public override bool Equals (object o)
	{
		if(o.GetType() == typeof(Level)) {
			return ((Level)o).sceneName.Equals(sceneName);
		} else
			return base.Equals (o);
	}

	public override int GetHashCode ()
	{
		return sceneName.GetHashCode();
	}
	*/



}

