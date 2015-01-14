using UnityEngine;
using System.Collections.Generic;


public class ChallengeHighScores
{

	public static Dictionary<string,float> scores;

	public static void SetScore(Level level, float score) 
	{
		Debug.Log ("Attempting to set High Score for " + level.displayName + " to " + score);
		if(scores == null)
			LoadScores();
		float oldScore;
		string key = level.name;
		if(scores.TryGetValue(key,out oldScore)) {
			scores[key] = Mathf.Max(oldScore,score);
		} else {
			scores.Add(key,score);
		}
		SaveScores();
	}
	
	static void LoadScores()
	{
		if(!Util.TryLoadFromPlayerPrefs<Dictionary<string,float>>("HighScores",out scores))
			InitScores();
	}

	public static void SaveScores()
	{
		Util.SaveToPlayerPrefs<Dictionary<string,float>>("HighScores",scores);
	}

	static void InitScores()
	{
		Level[] levels = Resources.LoadAll<Level>("Levels");
		scores = new Dictionary<string, float>();
		for(int i = 0; i < levels.Length; i++) {
			scores.Add(levels[i].name,0);
		}
	}

	public static Dictionary<string,float> GetAllScores()
	{
		LoadScores();
		return scores;
	}
	
	public static float GetScore(Level level)
	{
		return GetScore(level.name);
	}

	public static float GetScore(string levelName)
	{
		return GetAllScores()[levelName];
	}

	public static void Clear()
	{
		PlayerPrefs.DeleteKey("HighScores");
	}

}

