using UnityEngine;
using System.Collections.Generic;

public class GameGUI : MonoBehaviour
{

	enum GameScreen
	{
		GAME,PAUSE,START,END
	};

	public GUISkin defaultSkin;


	

	float origWidth = 1920;
	float origHeight = 1080;
	Vector3 scale;

	PlayerBall pb;
	ScoreArea scoreArea;

	GameScreen screen;

	public Level level;
	public Goal goal;
	bool countUpwards = false;
	float timeRemaining;

	int controller;

	public List<GameObject> difficultyPrefabs;
	static string[] difficulties = 
	{
		"Easy","Medium","Hard","Very Hard"
	};
	public int difficulty = 0;

	bool doneGoalCompleted;
	

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


		controller = PlayerPrefs.GetInt("Controller",0);

		if(level == null) {
			level = Resources.Load<Level>("Levels/" + PlayerPrefs.GetString("LevelName","01-Circle"));
			goal = level.possibleGoals[PlayerPrefs.GetInt("GoalIndex",0)];
		}

		if(goal.GetType().BaseType == typeof(TimeLimitGoal)) {
			timeRemaining = ((TimeLimitGoal)goal).timeLimit;
		} else if(goal.GetType() == typeof(Arcade)){
			timeRemaining = ((Arcade)goal).timeRemaining;
		} else {
			countUpwards = true;
		}

		doneGoalCompleted = false;
	}

	void Update()
	{
		if(goal.GetType() == typeof(Arcade)) {
			timeRemaining = ((Arcade)goal).displayTime;
		} else
			timeRemaining -= Time.deltaTime;
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

	void TogglePause()
	{
		if(screen == GameScreen.PAUSE) {
			screen = GameScreen.GAME;
			PlayerPrefs.SetInt("Controller",controller);
			PlayerPrefs.Save();
		} else if(screen == GameScreen.GAME)
			screen = GameScreen.PAUSE;
	}

	void OnGUI()
	{
		scale.x = Screen.width/origWidth;
		scale.y = Screen.height/origHeight;
		scale.z = 1;
		Matrix4x4 lastMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero,Quaternion.identity,scale);
		GUISkin unityDef = GUI.skin;
		GUI.skin = defaultSkin;

		GUI.BeginGroup(new Rect(0,0,1920,1080),defaultSkin.GetStyle("Overlay"));

		if(screen == GameScreen.PAUSE) {
			GUILayout.BeginArea(new Rect(640,320,640,480),defaultSkin.GetStyle("MenuBox"));
			GUILayout.Label ("Paused",defaultSkin.GetStyle("Score"));
			GUILayout.Label ("Controls",defaultSkin.GetStyle("SmallerText"));
			controller = GUILayoutExtras.ArrowedSelector(controller,MainMenuGUI.controlSchemes,
			                                new GUIStyle[] {defaultSkin.GetStyle("ArrowButtonLeft"),defaultSkin.GetStyle("ArrowButtonRight")},
			defaultSkin.GetStyle("ArrowedSelectorLabel"));
			if(GUILayout.Button ("Restart",defaultSkin.GetStyle("MediumButton"))) {
				screen = GameScreen.GAME;
				Time.timeScale = 1;
				Application.LoadLevel(Application.loadedLevel);
			}
			if(GUILayout.Button ("Level Select",defaultSkin.GetStyle("MediumButton"))) {
				Time.timeScale = 1;
				GoToMainMenu(MainMenuGUI.MenuScreen.LEVELSELECT);
			}
			if(GUILayout.Button ("Main Menu",defaultSkin.GetStyle("MediumButton"))) {
				Time.timeScale = 1;
				GoToMainMenu(MainMenuGUI.MenuScreen.MAIN);
			}
			GUILayout.EndArea ();
		} else if(screen == GameScreen.START) {
			GUILayout.BeginArea(new Rect(640,360,640,360),defaultSkin.GetStyle("MenuBox"));

			if(GUILayout.Button ("Start",defaultSkin.GetStyle("LargeButton"))) {
				StartLevel(difficulty);
			}
			if(GUILayout.Button ("Level Select",defaultSkin.GetStyle("MediumButton"))) {
				Time.timeScale = 1;
				GoToMainMenu(MainMenuGUI.MenuScreen.LEVELSELECT);
			}
			if(GUILayout.Button ("Main Menu",defaultSkin.GetStyle("MediumButton"))) {
				Time.timeScale = 1;
				GoToMainMenu(MainMenuGUI.MenuScreen.MAIN);
			}

			GUILayout.Label("Difficulty",defaultSkin.GetStyle("Title"));
			GUILayout.BeginVertical(defaultSkin.GetStyle("GridBox"));
			//difficulty = GUILayout.SelectionGrid(difficulty,difficulties,1,defaultSkin.GetStyle("MediumButton"));
			difficulty = GUILayoutExtras.ArrowedSelector(
				difficulty,difficulties,
				new GUIStyle[] {defaultSkin.GetStyle("ArrowButtonLeft"),defaultSkin.GetStyle("ArrowButtonRight")},
			defaultSkin.GetStyle("MediumButton"));
			GUILayout.EndVertical();

			GUILayout.EndArea ();
		} else if(screen == GameScreen.END) {

			GUILayout.BeginArea(new Rect(640,360,640,360),defaultSkin.GetStyle("MenuBox"));
			GUILayout.Label("Game Over",defaultSkin.GetStyle("Title"));
			goal.DisplaySuccess(defaultSkin.GetStyle("Score"));
			if(GUILayout.Button ("Retry",defaultSkin.GetStyle("MediumButton"))) {
				screen = GameScreen.GAME;
				Time.timeScale = 1;
				Application.LoadLevel(Application.loadedLevel);
			}
			if(Application.loadedLevelName != "Arcade")
				if(GUILayout.Button ("Level Select",defaultSkin.GetStyle("MediumButton"))) {
					Time.timeScale = 1;
					GoToMainMenu(MainMenuGUI.MenuScreen.LEVELSELECT);
				}
			if(GUILayout.Button ("Main Menu",defaultSkin.GetStyle("MediumButton"))) {
				Time.timeScale = 1;
				GoToMainMenu(MainMenuGUI.MenuScreen.MAIN);
			}
			GUILayout.EndArea ();
		} else if(screen == GameScreen.GAME) {
			//Top left area
			GUILayout.BeginArea(new Rect(20,20,400,400));
			if(countUpwards) {
				GUILayout.Label("Time",defaultSkin.GetStyle ("Score"));
				GUILayout.Label(Util.FormatTime(Time.timeSinceLevelLoad),defaultSkin.GetStyle("Score"));
			} else {
				GUILayout.Label("Time Remaining",defaultSkin.GetStyle ("Score"));
				GUILayout.Label(Util.FormatTime(timeRemaining),defaultSkin.GetStyle("Score"));
			}
			GUILayout.EndArea();

			//top right area
			GUILayout.BeginArea(new Rect(1500,20,400,400));
			GUILayout.Label("Combo Multiplier",defaultSkin.GetStyle ("Score"));
			GUILayout.Label("X" + ScoreCalculator.Instance.comboMulti.ToString(), defaultSkin.GetStyle("Score"));
			GUILayout.EndArea();

			//bottom left area
			GUILayout.BeginArea(new Rect(20,660,400,400));
			GUILayout.FlexibleSpace();
			goal.DisplayProgress(defaultSkin.GetStyle("Score"),defaultSkin.GetStyle ("NextScore"));
			GUILayout.EndArea();

			//bottom right area
			GUILayout.BeginArea(new Rect(1500,660,400,400));
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Bombs",defaultSkin.GetStyle("Score"));
			GUILayout.Label (pb.numBombs.ToString(),defaultSkin.GetStyle("Score"));
			GUILayout.EndArea();

		}

		GUI.EndGroup();
		GUI.skin = unityDef;
		GUI.matrix = lastMat;
	}

	void OnDisable()
	{
		PlayerPrefs.SetInt("Controller",controller);
		PlayerPrefs.Save();
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

	void GoToMainMenu(MainMenuGUI.MenuScreen screen)
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

