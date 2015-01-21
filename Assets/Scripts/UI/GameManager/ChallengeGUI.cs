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
		goal = Resources.Load<Goal>(goalPath);

		level = Resources.Load<Level>(levelPath);

		bm = ballMachine.GetComponent<BallMachine>();
		//bm.StartSpawning();
	}

	protected override void DoGameEnd ()
	{

		float result = level.goal.EvaluateSuccess();
		int rank = 0;
		for(int i = 0; i < level.ranks.Length; i++)
			if(result >= level.ranks[i])
				rank = i;
		level.LoadProgress();
		if(rank > level.progress.rank)
			level.progress.rank = rank;
		if(result > level.progress.score)
			level.progress.score = result;
		level.SaveProgress();

		//TODO display progress in end window
		endUI.GetComponentInChildren<ChallengeEndDisplay>().EndDisplay(level);
	}




}

