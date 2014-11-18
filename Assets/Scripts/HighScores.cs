using UnityEngine;
using System.Collections.Generic;


public class HighScores
{

	public static Dictionary<string,List<float>> scores;

	public static void SetScore(Level level, Goal goal, int difficulty, float score) 
	{
		Debug.Log ("Attempting to set High Score for " + level.displayName + " with goal " + goal.displayName + " to " + score);
		if(scores == null)
			LoadScores();
		List<float> oldScores;
		string key = level.name + goal.name;
		if(scores.TryGetValue(key,out oldScores)) {
			scores[key][difficulty] = Mathf.Max(oldScores[difficulty],score);
		} else {
			List<float> tmpScores = new List<float>();
			for(int k = 0; k <= Level.maxDifficulty; k++)
				tmpScores.Add(0);
			tmpScores[difficulty] = score;
			scores.Add(key,tmpScores);
		}
		SaveScores();
	}
	
	static void LoadScores()
	{
		if(!Util.TryLoadFromPlayerPrefs<Dictionary<string,List<float>>>("HighScores",out scores))
			InitScores();
	}

	public static void SaveScores()
	{
		Util.SaveToPlayerPrefs<Dictionary<string,List<float>>>("HighScores",scores);
	}

	static void InitScores()
	{
		Level[] levels = Resources.LoadAll<Level>("Levels");
		scores = new Dictionary<string, List<float>>();
		for(int i = 0; i < levels.Length; i++) {
			for(int j = 0; j < levels[i].possibleGoals.Length; j++) {
				List<float> tmpScores = new List<float>();
				for(int k = 0; k <= Level.maxDifficulty; k++)
					tmpScores.Add(0);
				scores.Add(levels[i].name + levels[i].possibleGoals[j].name,tmpScores);
			}
		}
		List<float> endlessScore = new List<float>();
		endlessScore.Add(0);
		scores.Add("Endless" + "Endless",endlessScore);
	}

	public static Dictionary<string,List<float>> GetAllScores()
	{
		LoadScores();
		return scores;
	}

	public static List<float> GetScores(Level level, Goal goal) 
	{
		return GetAllScores()[level.name + goal.name];
	}

	public static float GetScore(Level level, Goal goal, int difficulty)
	{
		return GetScore(level.name,goal.name,difficulty);
	}

	public static float GetScore(string levelName, string goalName, int difficulty)
	{
		return GetAllScores()[levelName + goalName][difficulty];
	}

}

