using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Level : ScriptableObject
{
	
	public string sceneName;
	public int difficulty;
	public Goal[] possibleGoals;
	public int goalIndex;

	public float[] highScores;

	public void SetHighScore(Goal goal, float score)
	{
		for(int i = 0; i < possibleGoals.Length; i++) {
			if(possibleGoals[i].Equals(goal))
				highScores[i] = Mathf.Max(highScores[i],score);
		}
	}

	public Goal ActiveGoal()
	{
		return possibleGoals[goalIndex];
	}




}

