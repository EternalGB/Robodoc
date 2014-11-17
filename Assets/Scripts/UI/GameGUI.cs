using UnityEngine;
using System.Collections.Generic;

public class GameGUI : MonoBehaviour
{

	enum GameScreen
	{
		GAME,PAUSE,START,END
	};


	PlayerBall pb;
	ScoreArea scoreArea;

	GameScreen screen;

	public Level level;
	public Goal goal;
	

	public List<GameObject> difficultyPrefabs;
	static string[] difficulties = 
	{
		"Easy","Medium","Hard","Very Hard"
	};
	public int difficulty = 0;

	bool doneGoalCompleted;

	public TextList controlScheme;
	public GameObject gameUI;
	public GameObject pauseUI;

	public float displayTime;

	void Start()
	{
		Time.timeScale = 0;
		if(Application.loadedLevelName == "Arcade") {
			screen = GameScreen.GAME;
			difficulty = 1;
			Time.timeScale = 1;
			//GameObject.Instantiate(difficultyPrefabs[difficulty]);
			GameObject.Find("BGMusic").GetComponent<AudioSource>().Play();
		} else
			screen = GameScreen.START;
		pb = GameObject.Find ("PlayerBall").GetComponent<PlayerBall>();
		//timeRemaining += Time.timeSinceLevelLoad;




		if(level == null) {
			level = Resources.Load<Level>("Levels/" + PlayerPrefs.GetString("LevelName","01-Circle"));
			goal = level.possibleGoals[PlayerPrefs.GetInt("GoalIndex",0)];
		}


		doneGoalCompleted = false;
	}

	void Update()
	{
		goal.UpdateTime();
		displayTime = goal.displayTime;

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
			screen = GameScreen.END;
			OnGoalCompleted();
		}

#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.F1)) {
			Time.timeScale = 0;
			screen = GameScreen.END;
			OnGoalCompleted();
		}
#endif
	}

	bool Paused()
	{
		return screen != GameScreen.GAME;
	}

	public void TogglePause()
	{
		if(screen == GameScreen.PAUSE) {
			screen = GameScreen.GAME;
			PlayerPrefs.SetInt("Controller",controlScheme.index);
			PlayerPrefs.Save();
			pauseUI.SetActive(false);
		} else if(screen == GameScreen.GAME) {
			screen = GameScreen.PAUSE;
			pauseUI.SetActive(true);
			controlScheme.DisplayText(PlayerPrefs.GetInt("Controller",1));
		}
	}



	void OnGoalCompleted()
	{
		if(!doneGoalCompleted) {
			level.SetHighScore(goal,difficulty,goal.EvaluateSuccess());
			if(Application.loadedLevelName != "Arcade")
				ProgressionManager.UpdateProgression(PlayerPrefs.GetInt("LevelIndex"),PlayerPrefs.GetInt("GoalIndex"));
			doneGoalCompleted = true;
		}
	}

	public void GoToMainMenu()
	{
		PlayerPrefs.SetInt("FromGameGUI",1);
		PlayerPrefs.SetString("MenuScreen",screen.ToString());
		Application.LoadLevel("MainMenu");
	}

	void StartLevel(int difficulty)
	{
		screen = GameScreen.GAME;
		Time.timeScale = 1;
		GameObject.Instantiate(difficultyPrefabs[difficulty]);
		GameObject.Find("BGMusic").GetComponent<AudioSource>().Play();
	}
	


	/*
	 * 
	public List<GameObject> bonusBalls;
	public List<GameObject> colourBalls;
	public List<GameObject> badBalls;
	
	public float minSpeed,maxSpeed;
	public float ballPerSecond;
	public float badBallChance;


	public void SetDifficulty(int difficulty)
	{
		List<GameObject> tmpColourBalls = new List<GameObject>(colourBalls);
		colourBalls = new List<GameObject>();
		for(int i = 0; i < difficulty+1 && i < tmpColourBalls.Count; i++)
			colourBalls.Add(tmpColourBalls[i]);

		List<GameObject> tmpBadBalls = new List<GameObject>(badBalls);
		badBalls = new List<GameObject>();
		for(int i = 0; i < difficulty && i < tmpBadBalls.Count; i++)
			badBalls.Add (tmpBadBalls[i]);

		minInitSpeed += 1f*(difficulty-1);
		maxInitSpeed += 1f*(difficulty-1);
		ballsPerSec += 1f*(difficulty-1);
		badBallChance += 0.05f*(difficulty-1);
	}

	public void SetDifficulty()
	{
		LevelInitiator init = li.GetComponent<LevelInitiator>();
		goodBallPrefabs = new List<GameObject>(init.colourBalls);
		goodBallPrefabs.AddRange(init.bonusBalls);
		badBallPrefabs = new List<GameObject>(init.badBalls);
		minInitSpeed = init.minSpeed;
		maxInitSpeed = init.maxSpeed;
		ballsPerSec = init.ballPerSecond;
		badBallChance = init.badBallChance;
	}
	*/
	

}

