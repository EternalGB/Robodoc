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

			if(goal.Completed()) {
				Time.timeScale = 0;
				OnGoalCompleted();
			}
		}

		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.F1)) {
			Time.timeScale = 0;
			OnGoalCompleted();
		}
		#endif

		if(Input.GetButtonDown("Pause")) {
			TogglePause();
		}

	}

	protected override void DoGameEnd ()
	{
		endUI.SetActive(true);
	}

	void TutorialEnd()
	{
		tutorialFinished = true;
		forcedPause = false;
		messageBox.SetActive(false);
		SetTutorialCompleted();
		ScoreCalculator.Instance.Reset();
		GameObject.Find("PlayerBall").GetComponent<PlayerBall>().numBombs = 3;
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

