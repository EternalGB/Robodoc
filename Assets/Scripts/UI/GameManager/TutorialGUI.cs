using UnityEngine;
using System.Collections.Generic;

public class TutorialGUI : GameGUI
{

	public GameObject messageBox;
	public GameObject targetPrefab;

	public TutorialEventManager tutorialManager;
	bool tutorialFinished = false;
	bool forcedPause = true;

	protected override void InitGame ()
	{
		Time.timeScale = 0;
		tutorialManager.BeginTutorial();
		tutorialManager.TutorialFinished += TutorialEnd;
		goal.UpdateTime();
	}

	void Update()
	{

		if(tutorialFinished) {
			goal.UpdateTime();
			displayTime = goal.displayTime;
			BGImage.UpdatePos(Mathf.Clamp(1 - displayTime/120f,0,1));
		}

		if(Input.GetButtonDown("Pause")) {
			TogglePause();
		}

	}

	protected override void DoGameEnd ()
	{
		throw new System.NotImplementedException ();
	}

	void TutorialEnd()
	{
		tutorialFinished = true;
		forcedPause = false;
		messageBox.SetActive(false);
		SetTutorialCompleted();
	}

	public void SetTutorialCompleted()
	{
		PlayerPrefs.SetInt("TutorialCompleted",1);
	}

	public void ToggleForcePause()
	{
		if(forcedPause) {
			forcedPause = false;
			Time.timeScale = 1;
		} else {
			forcedPause = true;
			Time.timeScale = 0;
		}
	}

}

