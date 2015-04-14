using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public abstract class GameGUI : MonoBehaviour
{



	protected PlayerBall pb;
	protected ScoreArea scoreArea;
	
	public Goal goal;

	protected bool doneGoalCompleted;
	protected bool forceCompleted = false;

	//public ArrowedSelector controlScheme;
	public GameObject pauseUI;
	public GameObject settingsUI;
	public GameObject endUI;

	public BGController BGImage;
	public float displayTime;
	protected bool paused = false;
	bool settingsOpen = false;

	bool gameStarted;

	float initCameraPos;
	float maxCameraZoom = 70;
	float cameraZoom;
	float actualCameraZoom;
	float zoomLerpTimer;
	float zoomLerpSpeed = 0.1f;

	//public AudioMixer masterMixer;

	public delegate void GameEndEvent();
	public event GameEndEvent GameEnd;

	public delegate void PauseToggleEvent(bool paused);
	public event PauseToggleEvent PauseToggled;
	

	protected void Start()
	{

		pb = GameObject.Find ("PlayerBall").GetComponent<PlayerBall>();
		doneGoalCompleted = false;
		initCameraPos = Camera.main.transform.position.z;
		ScoreCalculator.ScorePredictionUpdated += HandleScorePrediction;
		ScoreCalculator.PlayerScored += HandlePlayerScored;

		Time.timeScale = 0;
		gameStarted = false;

		if(settingsUI == null)
			settingsUI = GameObject.FindGameObjectWithTag("SettingsMenu");

		InitGame();

		GameEnd += DoGameEnd;
	}

	protected abstract void InitGame();
	

	public void StartGame()
	{
		GameObject.FindGameObjectWithTag("BGMusic").GetComponent<AudioSource>().Play();

		Time.timeScale = 1;
		gameStarted = true;
	}

	protected void Update()
	{
		if(gameStarted) {
			/*
		if(Input.anyKeyDown && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
			Debug.Log (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name + " is now selected");
		*/
			
			goal.UpdateDisplayTime();
			displayTime = goal.displayTime;
			UpdateBackground();
			
			actualCameraZoom = Mathf.Lerp (actualCameraZoom,cameraZoom,zoomLerpTimer);
			zoomLerpTimer = Mathf.Clamp (zoomLerpTimer + zoomLerpSpeed*Time.deltaTime,0,1f);
			Camera.main.transform.position = new Vector3(0,0,initCameraPos - actualCameraZoom);

			if(Input.GetButtonDown("Cancel")) {
				if(settingsOpen) {
					Debug.Log ("Closing settings menu");
					HideSettings();
				} else if(!paused)
					TogglePause();
				else {
					Debug.Log ("Returning to main menu");
					GoToMainMenu();
				}
			}

			/*
		if(Paused()) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
		*/
			
			if(goal.Completed() || forceCompleted) {
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

	}

	protected virtual void UpdateBackground()
	{
		BGImage.UpdatePos(Mathf.Clamp(1 - displayTime/goal.initTime,0,1));
	}

	void HandlePlayerScored(float scoreIncrease)
	{
		zoomLerpTimer = 0;
		cameraZoom = 0;
	}

	void HandleScorePrediction(float scoreIncrease, int maxChain)
	{
		int maxDepth = ScoreCalculator.GetMaxActualDepth(pb.transform,0);
		SetCameraZoom(maxDepth);
	}

	void SetCameraZoom(int maxChain)
	{
		cameraZoom = Mathf.Clamp ((maxCameraZoom/15f)*maxChain,0,maxCameraZoom);
		zoomLerpTimer = 0;
	}

	public bool Paused()
	{
		return paused;
	}

	public void TogglePause()
	{
		if(!doneGoalCompleted) {
			if(paused) {
				//PlayerPrefs.SetInt("Controller",controlScheme.index);
				PlayerPrefs.Save();
				pauseUI.SetActive(false);
				paused = false;
				Time.timeScale = 1;
				//masterMixer.FindSnapshot("Unpaused").TransitionTo(0.1f);
			} else if(!paused) {
				pauseUI.SetActive(true);
				paused = true;
				//controlScheme.ChangeText(PlayerPrefs.GetInt("Controller",1));
				Time.timeScale = 0;
				//masterMixer.FindSnapshot("Paused").TransitionTo(0.1f);

			}
			PauseToggled(paused);
		}
	}



	protected void OnGoalCompleted()
	{
		if(!doneGoalCompleted) {
			endUI.SetActive(true);
			GameEnd();
			paused = true;
			doneGoalCompleted = true;
		}
	}

	protected abstract void DoGameEnd();

	public void GoToMainMenu()
	{
		ExitAndGoToMainMenu("MainMenu");
	}

	public void GoToChallenges()
	{
		ExitAndGoToMainMenu("Challenges");
	}

	void ExitAndGoToMainMenu(string menuScreenName)
	{
		goal.ResetGoal();
		paused = false;
		Time.timeScale = 1;
		PlayerPrefs.SetString("MenuScreen",menuScreenName);
		Application.LoadLevel("MainMenu");
	}

	public void RetryLevel()
	{
		goal.ResetGoal();
		Application.LoadLevel(Application.loadedLevel);
	}
	
	void OnDisable()
	{
		ScoreCalculator.PlayerScored -= HandlePlayerScored;
		ScoreCalculator.ScorePredictionUpdated -= HandleScorePrediction;
	}

	public void EndEarly()
	{
		forceCompleted = true;
	}

	public void ShowSettings()
	{
		pauseUI.SetActive(false);
		settingsUI.SetActive(true);
		settingsOpen = true;
	}

	public void HideSettings()
	{
		Settings.Save();
		settingsUI.SetActive(false);
		pauseUI.SetActive(true);
		settingsOpen = false;
	}
	
}

