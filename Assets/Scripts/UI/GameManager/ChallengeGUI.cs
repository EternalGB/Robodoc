using UnityEngine;
using System.Collections.Generic;

public class ChallengeGUI : GameGUI
{

	public List<GameObject> difficultyPrefabs;
	static string[] difficulties = 
	{
		"Easy","Medium","Hard","Very Hard"
	};
	public int difficulty = 0;

	protected override void InitGame ()
	{
		/*
		if(level == null) {
			level = Resources.Load<Level>("Levels/" + PlayerPrefs.GetString("LevelName","01-Circle"));
			goal = level.possibleGoals[PlayerPrefs.GetInt("GoalIndex",0)];
		}
		*/
		GameObject.FindWithTag("BallMachine").GetComponent<BallMachine>().StartSpawning();
	}

	protected override void DoGameEnd ()
	{
		//level.SetHighScore(goal,difficulty,goal.EvaluateSuccess());
		ProgressionManager.UpdateProgression(PlayerPrefs.GetInt("LevelIndex"),PlayerPrefs.GetInt("GoalIndex"));
	}

	protected void StartLevel(int difficulty)
	{
		Time.timeScale = 1;
		GameObject.Instantiate(difficultyPrefabs[difficulty]);
		GameObject.Find("BGMusic").GetComponent<AudioSource>().Play();
	}

}

