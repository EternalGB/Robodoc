using UnityEngine;
using System.Collections.Generic;

public abstract class GameGUI : MonoBehaviour
{


	protected PlayerBall pb;
	protected ScoreArea scoreArea;

	public Level level;
	public Goal goal;

	protected bool doneGoalCompleted;

	public TextList controlScheme;
	public GameObject gameUI;
	public GameObject pauseUI;
	public GameObject endUI;

	public BGController BGImage;
	public float displayTime;
	protected bool paused = false;

	protected void Start()
	{
		pb = GameObject.Find ("PlayerBall").GetComponent<PlayerBall>();
		doneGoalCompleted = false;
		InitGame();
	}

	protected abstract void InitGame();

	protected void Update()
	{
		goal.UpdateTime();
		displayTime = goal.displayTime;
		BGImage.UpdatePos(Mathf.Clamp(1 - displayTime/120f,0,1));

		if(Input.GetButtonDown("Pause")) {
			TogglePause();
		}

		if(Paused()) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}

		if(goal.Completed()) {
			Time.timeScale = 0;
			OnGoalCompleted();
		}

		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.F1)) {
			Time.timeScale = 0;
			OnGoalCompleted();
		}
		#endif
	}

	public bool Paused()
	{
		return paused;
	}

	public void TogglePause()
	{
		if(paused) {
			PlayerPrefs.SetInt("Controller",controlScheme.index);
			PlayerPrefs.Save();
			pauseUI.SetActive(false);
			paused = false;
		} else if(!paused) {
			pauseUI.SetActive(true);
			paused = true;
			controlScheme.DisplayText(PlayerPrefs.GetInt("Controller",1));
		}
	}



	protected void OnGoalCompleted()
	{
		if(!doneGoalCompleted) {
			DoGameEnd();
			endUI.SetActive(true);
			paused = true;
			doneGoalCompleted = true;
		}
	}

	protected abstract void DoGameEnd();

	public void GoToMainMenu()
	{
		PlayerPrefs.SetInt("FromGameGUI",1);
		paused = false;
		Time.timeScale = 1;
		Application.LoadLevel("MainMenu");
	}
	
	public void RetryLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
		goal.ResetGoal();
	}


}

