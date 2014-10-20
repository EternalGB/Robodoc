using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class HighScores
{

	public static Dictionary<string,List<float>> scores;

	public static void SetScore(Level level, Goal goal, int difficulty, float score) 
	{
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
		string data = PlayerPrefs.GetString("HighScores");
		if(!string.IsNullOrEmpty(data)) {
			//Binary formatter for loading back
			BinaryFormatter b = new BinaryFormatter();
			//Create a memory stream with the data
			MemoryStream m = new MemoryStream(Convert.FromBase64String(data));
			//Load back the scores
			scores = (Dictionary<string,List<float>>)b.Deserialize(m);
		} else {
			InitScores();
		}
	}

	public static void SaveScores()
	{
		//Get a binary formatter
		var b = new BinaryFormatter();
		//Create an in memory stream
		var m = new MemoryStream();
		//Save the scores
		b.Serialize(m, scores);
		//Add it to player prefs
		PlayerPrefs.SetString("HighScores",Convert.ToBase64String(m.GetBuffer()));
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
	}

	public static Dictionary<string,List<float>> GetAllScores()
	{
		if(scores == null)
			LoadScores();
		return scores;
	}

	public static List<float> GetScores(Level level, Goal goal) 
	{
		return GetAllScores()[level.name + goal.name];
	}

}

