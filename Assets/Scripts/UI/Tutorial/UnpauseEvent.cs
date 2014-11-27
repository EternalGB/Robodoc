using UnityEngine;
using System.Collections;

public class UnpauseEvent : TutorialEvent
{

	public TutorialGUI ui;

	protected override void InitEvent ()
	{

	}

	public override void Activate ()
	{
		ui.ToggleForcePause();
	}

	public override void Deactivate ()
	{

	}

	public override bool Completed ()
	{
		return true;
	}

}

