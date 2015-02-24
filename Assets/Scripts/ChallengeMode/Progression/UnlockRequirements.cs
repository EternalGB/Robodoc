using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnlockRequirements
{

	public delegate bool UnlockFunc();

	public static bool AlwaysUnlocked()
	{
		return true;
	}
	
	public static UnlockFunc RankPointRequirement(int amount)
	{
		return () => ChallengeProgressionManager.Instance.progress.rankPoints >= amount;
	}

}

