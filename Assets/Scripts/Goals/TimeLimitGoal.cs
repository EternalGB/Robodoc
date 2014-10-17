using UnityEngine;
using System.Collections;

public abstract class TimeLimitGoal : Goal
{

	public float timeLimit;
	float startTime;
	
	void OnEnable()
	{
		startTime = Time.timeSinceLevelLoad;
	}
	
	public override bool Completed ()
	{
		return (Time.timeSinceLevelLoad - startTime) > timeLimit;
	}

}

