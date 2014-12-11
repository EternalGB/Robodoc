using UnityEngine;
using System.Collections.Generic;

public abstract class GameGUI : MonoBehaviour
{



	protected PlayerBall pb;
	protected ScoreArea scoreArea;

	public Level level;
	public Goal goal;

	protected bool doneGoalCompleted;

	public ArrowedSelector controlScheme;
	public GameObject gameUI;
	public GameObject pauseUI;
	public GameObject endUI;

	public BGController BGImage;
	public float displayTime;
	protected bool paused = false;

	float initCameraPos;
	public float maxCameraZoom;
	float cameraZoom;
	float actualCameraZoom;
	float zoomLerpTimer;
	float zoomLerpSpeed = 0.1f;

	protected void Start()
	{

		pb = GameObject.Find ("PlayerBall").GetComponent<PlayerBall>();
		doneGoalCompleted = false;
		initCameraPos = Camera.main.transform.position.z;
		ScoreCalculator.ScorePredictionUpdated += HandleScorePrediction;
		ScoreCalculator.PlayerScored += HandlePlayerScored;
		InitGame();
	}

	protected abstract void InitGame();

	protected void Update()
	{
		if(Input.anyKeyDown && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
			Debug.Log (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name + " is now selected");

		goal.UpdateTime();
		displayTime = goal.displayTime;
		BGImage.UpdatePos(Mathf.Clamp(1 - displayTime/120f,0,1));

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
		if(paused) {
			PlayerPrefs.SetInt("Controller",controlScheme.index);
			PlayerPrefs.Save();
			pauseUI.SetActive(false);
			paused = false;
			Time.timeScale = 1;
		} else if(!paused) {
			pauseUI.SetActive(true);
			paused = true;
			controlScheme.ChangeText(PlayerPrefs.GetInt("Controller",1));
			Time.timeScale = 0;
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
		goal.ResetGoal();
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

	public void UpdateControlScheme()
	{
		PlayerPrefs.SetInt("Controller",controlScheme.index);
	}

	void OnDisable()
	{
		ScoreCalculator.PlayerScored -= HandlePlayerScored;
		ScoreCalculator.ScorePredictionUpdated -= HandleScorePrediction;
	}


}

