using UnityEngine;
using System.Collections;

public class CallNamedEvent : TutorialEvent
{

	public MonoBehaviour script;
	public string methodName;
	bool completed = false;

	protected override void InitEvent ()
	{

	}

	public override void Activate ()
	{
		script.SendMessage(methodName);
		completed = true;
	}

	public override void Deactivate ()
	{

	}

	public override bool Completed ()
	{
		return completed;
	}

}

