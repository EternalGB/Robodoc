using UnityEngine;
using UnityEngine.UI;
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
		/*
		string machinePath;
		if(!Util.TryLoadFromPlayerPrefs<string>("MachinePath", out machinePath))
			machinePath = defaultMachinePath;
		string goalPath;
		if(!Util.TryLoadFromPlayerPrefs<string>("GoalPath",out goalPath))
			goalPath = defaultGoalPath;
		string levelPath;
		if(!Util.TryLoadFromPlayerPrefs<string>("LevelPath",out levelPath))
			levelPath = defaultLevelPath;
		*/
		level = SelectedLevel.level;

		GameObject ballMachine = (GameObject)Instantiate(level.ballMachinePrefab.gameObject);
		goal = level.goal;

		//level = Resources.Load<Level>(levelPath);

		bm = ballMachine.GetComponent<BallMachine>();
		//bm.StartSpawning();
		if(grd == null)
			grd = GameObject.Find ("NextRankArea").GetComponent<GoalRankDisplay>();
		if(grd != null)
			grd.Init(level,goal as ChallengeGoal);

		if(goal.GetType().IsAssignableFrom(typeof(ScoreTarget))) {
			GameObject targetDisplay = GameObject.Find ("ScoreTarget");
			if(targetDisplay)
				targetDisplay.GetComponent<Text>().text = (goal as ScoreTarget).targetScore.ToString();
		}


		StartCountdown();
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
		if(rank > level.progress.rank)
			level.progress.rank = rank;
		//Debug.Log ("Finishing " + level.name + " last score: " + level.progress.score + " new score: " + result);
		if(((ChallengeGoal)goal).ScoreComparitor(result,level.progress.score) > 0) {
			level.progress.score = result;
		}
		level.SaveProgress();

		endUI.GetComponentInChildren<ChallengeEndDisplay>().EndDisplay(result,rank,goal as ChallengeGoal);
	}




}

