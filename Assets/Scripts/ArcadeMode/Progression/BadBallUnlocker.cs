using UnityEngine;
using System.Collections;

public class BadBallUnlocker : Unlockable
{

	int index;
	float highScoreRequirement;

	public BadBallUnlocker (int index, float highScoreRequirement)
	{
		this.index = index;
		this.highScoreRequirement = highScoreRequirement;
	}
	

	public override bool ConditionMet ()
	{
		return ArcadeStats.HighScore >= highScoreRequirement;
	}

	protected override void UnlockEffect ()
	{
		ArcadeProgression.BadBallIndex = index;
	}

}

