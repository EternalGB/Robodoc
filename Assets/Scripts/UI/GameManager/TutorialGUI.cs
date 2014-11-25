using UnityEngine;
using System.Collections.Generic;

public class TutorialGUI : GameGUI
{

	public GameObject messageBox;
	public GameObject targetPrefab;

	public TutorialEventManager tutorialManager;


	protected override void InitGame ()
	{
		paused = true;
		tutorialManager.BeginTutorial();
	}

	void Update()
	{
		base.Update();
		if(tutorialManager.Finished)
			messageBox.SetActive(false);
	}

	protected override void DoGameEnd ()
	{
		throw new System.NotImplementedException ();
	}

}

