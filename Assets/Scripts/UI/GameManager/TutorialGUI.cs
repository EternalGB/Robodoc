using UnityEngine;
using System.Collections.Generic;

public class TutorialGUI : GameGUI
{

	public GameObject messageBox;
	public GameObject targetPrefab;

	public TutorialEventManager tutorialManager;

	protected override void InitGame ()
	{
		//Time.timeScale = 0;
		tutorialManager.BeginTutorial();
		tutorialManager.TutorialFinished += TutorialEnd;
		goal.UpdateDisplayTime();

		StartCountdown();
	}


	protected override void DoGameEnd ()
	{
		endUI.SetActive(true);
	}

	public void TutorialEnd()
	{
		SetTutorialCompleted();
	}

	public void SetTutorialCompleted()
	{
		PlayerPrefs.SetInt("TutorialCompleted",1);
	}



}

