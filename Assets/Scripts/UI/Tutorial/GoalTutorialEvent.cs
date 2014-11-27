using UnityEngine;
using System.Collections;

public abstract class GoalTutorialEvent : TutorialEvent
{

	bool completed = false;

	protected void SetCompleted()
	{
		completed = true;
	}

	public override bool Completed ()
	{
		return completed;
	}

}

