using UnityEngine;
using System.Collections.Generic;

public abstract class TimedTutorialEvent : TutorialEvent
{

	public float minTime,maxTime;
	public List<KeyCode> skipKeys;
	public bool mouseSkip;

	float startTime;
	bool skipPressed = false;

	void Update()
	{
		foreach(KeyCode key in skipKeys) {
			if(Input.GetKeyDown(key))
				skipPressed = true;
		}
		if(mouseSkip && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
			skipPressed = true;
	}

	public sealed override void Activate ()
	{
		startTime = Time.unscaledTime;
		StartEvent();
	}

	protected abstract void StartEvent();

	public override bool Completed ()
	{
		float timePassed = Time.unscaledTime - startTime;
		return (timePassed >= minTime && skipPressed) || timePassed >= maxTime;
	}

}

