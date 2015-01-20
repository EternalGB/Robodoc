using UnityEngine;
using System.Collections.Generic;

public class ChallengeGUI : GameGUI
{

	const string defaultMachinePath = "";
	const string defaultGoalPath = "";
	const string defaultLevelPath = "";

	BallMachine bm;
	Level level;

	protected override void InitGame ()
	{
		string machinePath;
		if(!Util.TryLoadFromPlayerPrefs<string>("MachinePath", out machinePath))
			machinePath = defaultMachinePath;
		string goalPath;
		if(!Util.TryLoadFromPlayerPrefs<string>("GoalPath",out goalPath))
			goalPath = defaultGoalPath;
		string levelPath;
		if(!Util.TryLoadFromPlayerPrefs<string>("LevelPath",out levelPath))
			levelPath = defaultLevelPath;


		GameObject ballMachine = (GameObject)Instantiate(Resources.Load<GameObject>(machinePath));
		goal = (Goal)Instantiate(Resources.Load<Goal>(goalPath));

		level = (Level)Instantiate(Resources.Load<Level>(levelPath));

		bm = ballMachine.GetComponent<BallMachine>();
		bm.StartSpawning();
	}

	protected override void DoGameEnd ()
	{

	}

	protected void StartLevel(int difficulty)
	{
		Time.timeScale = 1;
		GameObject.Find("BGMusic").GetComponent<AudioSource>().Play();
	}



}

