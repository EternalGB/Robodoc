using UnityEngine;
using System.Collections;

public class ColorBallUnlocker : Unlockable
{

	public ArcadeManager manager;
	public GameObject ball;
	public ArcadeStats.StatKeys stat;
	public float unlockRequirement;

	public override bool ConditionMet ()
	{
		return ArcadeStats.TotalScore >= unlockRequirement;
	}

	public override void UnlockEffect ()
	{
		manager.goodBallPrefabs.Add(ball);
	}

}

