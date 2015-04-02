using UnityEngine;
using System.Collections;

public class BallMachineUpgrader : Unlockable
{

	public ArcadeManager manager;
	public int additionalBadBalls;
	public float speedScaler;
	public float badSpawnRateScaler, goodSpawnRateScaler;



	public override void UnlockEffect ()
	{
		manager.initBadBalls += additionalBadBalls;
		for(int i = 0; i < manager.bm.badRateCurve.keys.Length; i++) {
			manager.bm.badRateCurve.keys[i].value *= badSpawnRateScaler;
		}
		for(int i = 0; i < manager.bm.goodRateCurve.keys.Length; i++) {
			manager.bm.badRateCurve.keys[i].value *= goodSpawnRateScaler;
		}
		for(int i = 0; i < manager.bm.bonusRateCurve.keys.Length; i++) {
			manager.bm.badRateCurve.keys[i].value *= goodSpawnRateScaler;
		}
		manager.bm.maxInitSpeed *= speedScaler;
		manager.bm.minInitSpeed *= speedScaler;
	}


}

