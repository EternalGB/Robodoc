using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Level : ScriptableObject
{
	
	public string sceneName;
	public int difficulty;
	public Goal[] possibleGoals;
	public int goalIndex;

	public List<HighScoreList> scoreLists;

	public static int maxDifficulty = 3;

	public void SetHighScore(Goal goal, int diff, float score)
	{
		for(int i = 0; i < possibleGoals.Length ; i++) {
			if(possibleGoals[i].Equals(goal)) {
				scoreLists[diff].highScores[i] = Mathf.Max(scoreLists[diff].highScores[i],score);
				return;
			}
		}
	}

	public Goal ActiveGoal()
	{
		return possibleGoals[goalIndex];
	}




}

