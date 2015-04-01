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
	public GameObject endUI;

	public BGController BGImage;
	public float displayTime;
	protected bool paused = false;

	public GameObject countdownCanvas;
	public Text startingCountdownText;
	bool gameStarted;
	bool countdownStarted;
	float startTimer = 3.99f;

	float initCameraPos;
	float maxCameraZoom = 70;
	float cameraZoom;
	float actualCameraZoom;
	float zoomLerpTimer;
	float zoomLerpSpeed = 0.1f;

	public AudioMixer masterMixer;

	protected void Start()
	{

		pb = GameObject.Find ("PlayerBall").GetComponent<PlayerBall>();
		doneGoalCompleted = false;
		initCameraPos = Camera.main.transform.position.z;
		ScoreCalculator.ScorePredictionUpdated += HandleScorePrediction;
		ScoreCalculator.PlayerScored += HandlePlayerScored;

		Time.timeScale = 0;
		gameStarted = false;
		countdownStarted = false;

		InitGame();
	}

	protected abstract void InitGame();

	protected void StartCountdown()
	{
		countdownStarted = true;
	}

	protected void Update()
	{
		if(countdownStarted) {
			if(!gameStarted) {
				startTimer -= Time.unscaledDeltaTime;
				if(startTimer <= 1) {
					gameStarted = true;
					countdownCanvas.SetActive(false);
					Time.timeScale = 1;
				}
				startingCountdownText.text = Mathf.Floor(startTimer).ToString();
			} else {
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
				
				if(Input.GetButtonDown("Pause")) {
					TogglePause();
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
				masterMixer.FindSnapshot("Unpaused").TransitionTo(0.1f);

			} else if(!paused) {
				pauseUI.SetActive(true);
				paused = true;
				//controlScheme.ChangeText(PlayerPrefs.GetInt("Controller",1));
				Time.timeScale = 0;
				masterMixer.FindSnapshot("Paused").TransitionTo(0.1f);

			}
		}
	}



	protected void OnGoalCompleted()
	{
		if(!doneGoalCompleted) {
			endUI.SetActive(true);
			DoGameEnd();
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

}

