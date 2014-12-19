using UnityEngine;
using System.Collections;

public class ArrowedMessageTimed : ArrowedMessage
{

	bool completed = false;
	public float duration;

	protected override void InitEvent ()
	{

	}

	public override void Activate ()
	{
		StartCoroutine(Timers.Countdown(duration,() => completed = true));
		base.Activate ();
	}

	public override bool Completed ()
	{
		return completed;
	}

}

