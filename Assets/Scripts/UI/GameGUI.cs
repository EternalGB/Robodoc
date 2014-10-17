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
	float maxTime;

	int controller;

	public List<GameObject> difficultyPrefabs;
	static string[] difficulties = 
	{
		"Easy","Medium","Hard","Very Hard"
	};
	public int difficulty = 0;

	void Start()
	{
		Time.timeScale = 0;
		screen = GameScreen.START;
		pb = GameObject.Find ("PlayerBall").GetComponent<PlayerBall>();
		maxTime += Time.timeSinceLevelLoad;


		controller = PlayerPrefs.GetInt("Controller",0);

		if(level == null) {
			level = Resources.Load<Level>("Levels/" + Application.loadedLevelName);
			goal = level.possibleGoals[PlayerPrefs.GetInt("GoalIndex",0)];
		}

		if(goal.GetType().BaseType == typeof(TimeLimitGoal)) {
			maxTime = ((TimeLimitGoal)goal).timeLimit;
		} else {
			countUpwards = true;
		}
	}

	void Update()
	{
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
			level.SetHighScore(goal,goal.EvaluateSuccess());
		}
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
			GUILayout.BeginVertical(defaultSkin.GetStyle("GridBox"));
			controller = GUILayout.SelectionGrid(controller,MainMenuGUI.controlSchemes,1);
			GUILayout.EndVertical();
			if(GUILayout.Button ("Restart")) {
				screen = GameScreen.GAME;
				Time.timeScale = 1;
				Application.LoadLevel(Application.loadedLevel);
			}
			if(GUILayout.Button ("Level Select")) {
				Time.timeScale = 1;
				PlayerPrefs.SetString("MenuScreen","LEVELSELECT");
				Application.LoadLevel("MainMenu");
			}
			if(GUILayout.Button ("Main Menu")) {
				Time.timeScale = 1;
				PlayerPrefs.SetString("MenuScreen","MAIN");
				Application.LoadLevel("MainMenu");
			}
			GUILayout.EndArea ();
		} else if(screen == GameScreen.START) {
			GUILayout.BeginArea(new Rect(640,320,640,480),defaultSkin.GetStyle("MenuBox"));
			GUILayout.Label("Difficulty",defaultSkin.GetStyle("Title"));
			GUILayout.BeginVertical(defaultSkin.GetStyle("GridBox"));
			difficulty = GUILayout.SelectionGrid(difficulty,difficulties,1);
			GUILayout.EndVertical();
			if(GUILayout.Button ("Start")) {
				screen = GameScreen.GAME;
				Time.timeScale = 1;
				GameObject.Instantiate(difficultyPrefabs[difficulty]);
				GameObject.Find("BGMusic").GetComponent<AudioSource>().Play();
			}
			if(GUILayout.Button ("Level Select")) {
				Time.timeScale = 1;
				PlayerPrefs.SetString("MenuScreen","LEVELSELECT");
				Application.LoadLevel("MainMenu");
			}
			if(GUILayout.Button ("Main Menu")) {
				Time.timeScale = 1;
				PlayerPrefs.SetString("MenuScreen","MAIN");
				Application.LoadLevel("MainMenu");
			}
			GUILayout.EndArea ();
		} else if(screen == GameScreen.END) {
			GUILayout.BeginArea(new Rect(640,320,640,320),defaultSkin.GetStyle("MenuBox"));
			GUILayout.Label("Game Over",defaultSkin.GetStyle("Title"));
			goal.DisplaySuccess(defaultSkin.GetStyle("Score"));
			if(GUILayout.Button ("Retry")) {
				screen = GameScreen.GAME;
				Time.timeScale = 1;
				Application.LoadLevel(Application.loadedLevel);
			}
			if(GUILayout.Button ("Level Select")) {
				Time.timeScale = 1;
				PlayerPrefs.SetString("MenuScreen","LEVELSELECT");
				Application.LoadLevel("MainMenu");
			}
			if(GUILayout.Button ("Main Menu")) {
				Time.timeScale = 1;
				PlayerPrefs.SetString("MenuScreen","MAIN");
				Application.LoadLevel("MainMenu");
			}
			GUILayout.EndArea ();
		} else if(screen == GameScreen.GAME) {
			GUILayout.BeginArea(new Rect(10,320,480,1000));
			if(countUpwards) {
				GUILayout.Label("Time",defaultSkin.GetStyle ("Score"));
				GUILayout.Label(Util.FormatTime(Time.timeSinceLevelLoad),defaultSkin.GetStyle("Score"));
			} else {
				GUILayout.Label("Time Remaining",defaultSkin.GetStyle ("Score"));
				GUILayout.Label(Util.FormatTime(maxTime - Time.timeSinceLevelLoad),defaultSkin.GetStyle("Score"));
			}
			goal.DisplayProgress(defaultSkin.GetStyle("Score"),defaultSkin.GetStyle ("NextScore"));
			GUILayout.Label("Combo Multiplier",defaultSkin.GetStyle ("Score"));
			GUILayout.Label("X" + ScoreCalculator.Instance.comboMulti.ToString(), defaultSkin.GetStyle("Score"));
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

