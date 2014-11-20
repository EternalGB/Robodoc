using UnityEngine;
using System.Collections;

public class ColorBallUnlocker : Unlockable
{

	int index;
	float totalScoreRequirement;

	public ColorBallUnlocker (int index, float totalScoreRequirement)
	{
		this.index = index;
		this.totalScoreRequirement = totalScoreRequirement;
	}
	

	public override bool ConditionMet ()
	{
		return ArcadeStats.TotalScore >= totalScoreRequirement;
	}

	protected override void UnlockEffect ()
	{
		ArcadeProgression.MaxColorBalls = index;
	}

}

