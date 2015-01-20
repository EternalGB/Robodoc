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
	public float[] ranks;
	public LevelProgress progress;


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

	[Serializable]
	public class LevelProgress
	{

		public int rank;
		public bool unlocked;
		public float score;

	}

}

