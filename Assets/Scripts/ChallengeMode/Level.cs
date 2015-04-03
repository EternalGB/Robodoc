using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Level : ScriptableObject
{

	public string displayName;
	public string sceneName;
	public ChallengeGoal goal;
	public BallMachine ballMachinePrefab;
	public int pointsNeededToUnlock;
	public float[] ranks;
	public LevelProgress progress;


	public bool Unlocked
	{
		get
		{
			return ChallengeProgressionManager.Instance.progress.rankPoints >= pointsNeededToUnlock;
		}
	}

	public void SaveProgress()
	{
		Util.SaveToPlayerPrefs<LevelProgress>(name + "progress",progress);
	}

	public void LoadProgress()
	{
		LevelProgress loadedProgress;
		if(Util.TryLoadFromPlayerPrefs<LevelProgress>(name + "progress",out loadedProgress))
			progress = loadedProgress;
		else
			progress = new LevelProgress{rank = 0, unlocked = false, score = 0};
	}

	public void ResetProgress()
	{
		progress.unlocked = false;
		if(goal.GetType().Equals(typeof(ScoreTarget)))
		   progress.score = float.MaxValue;
		else
			progress.score = 0;
		progress.rank = 0;
	}

	public int FindRank(float score)
	{
		int rank = 0;
		for(int i = 0; i < ranks.Length; i++)
			if(goal.ScoreComparitor(score,ranks[i]) > 0)
				rank = i+1;
		return rank;
	}


	[Serializable]
	public class LevelProgress
	{

		public int rank;
		public bool unlocked;
		public float score;


	}

}

