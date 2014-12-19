using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class TagBallsTimed : TagBalls
{

	public float duration;
	bool timerFinished = false;

	public override void Activate ()
	{
		StartCoroutine(Timers.Countdown(duration,() => timerFinished = true));
		base.Activate ();
	}

	protected override bool CheckCompleted ()
	{
		return timerFinished;
	}

}

