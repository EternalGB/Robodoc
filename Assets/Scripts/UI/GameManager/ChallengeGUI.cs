using UnityEngine;
using System.Collections.Generic;

public class ChallengeGUI : GameGUI
{

	const string defaultMachinePath = "";
	const string defaultGoalPath = "";
	const string defaultLevelPath = "";

	BallMachine bm;
	Level level;
	
	public GoalRankDisplay grd;

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
		grd.Init(level,goal);
	}

	protected override void UpdateBackground ()
	{
		if(goal.GetType().Equals(typeof(ScoreTarget))) {
			BGImage.UpdatePos(Mathf.Clamp (displayTime/level.ranks[level.ranks.Length-1],0,1));
		} else
			base.UpdateBackground ();
	}

	protected override void DoGameEnd ()
	{

		float result = level.goal.EvaluateSuccess();

		level.LoadProgress();
		int rank = level.FindRank(result);
		if(level.progress.rank > rank)
			level.progress.rank = rank;
		//Debug.Log ("Finishing " + level.name + " last score: " + level.progress.score + " new score: " + result);
		if(((ChallengeGoal)goal).ScoreComparitor(result,level.progress.score) > 0) {
			level.progress.score = result;
		}
		level.SaveProgress();

		//TODO display progress in end window
		endUI.GetComponentInChildren<ChallengeEndDisplay>().EndDisplay(level);
	}




}

