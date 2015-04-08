using UnityEngine;
using System.Collections;

public class BallMachineUpgrader : Unlockable
{

	public ArcadeManager manager;
	public int additionalBadBalls;
	public float speedIncr;
	public float badRateIncr, goodRateIncr;



	public override void UnlockEffect ()
	{
		manager.initBadBalls += additionalBadBalls;
		manager.bm.IncreaseGoodRateScaler(goodRateIncr);
		manager.bm.IncreaseBadRateScaler(badRateIncr);
		manager.bm.IncreaseSpeedScaler(speedIncr);
	}


}

