using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Tier : ScriptableObject
{

	public string displayName;
	public List<Level> levels;
	public int pointsNeededToUnlock;
	public TierProgress progress;


	public bool Unlocked
	{
		get
		{
			return ChallengeProgressionManager.Instance.progress.rankPoints >= pointsNeededToUnlock;
		}
	}
	
	public void SaveProgress()
	{
		Util.SaveToPlayerPrefs<TierProgress>(name + "progress",progress);
		if(levels != null)
			foreach(Level level in levels)
				level.SaveProgress();
	}

	public void LoadProgress()
	{
		TierProgress loadedProgress;
		if(Util.TryLoadFromPlayerPrefs<TierProgress>(name + "progress",out loadedProgress))
			progress = loadedProgress;
		else
			progress = new TierProgress{unlocked = false};
		if(levels != null)
			foreach(Level level in levels)
				level.LoadProgress();
	}



	[Serializable]
	public class TierProgress
	{
		public bool unlocked;
	}

}



