using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour
{

	int LevelComparitor(Level l1, Level l2)
	{
		return l1.difficulty - l2.difficulty;
	}

	enum MenuScreen
	{
		MAIN,LEVELSELECT
	};

	public static GUIContent[] controlSchemes = new GUIContent[]
	{
		new GUIContent("Keyboard"),
		new GUIContent("Mouse")
	};

	public GUISkin defaultSkin;

	public Texture howToPlayImage;

	float origWidth = 1920;
	float origHeight = 1080;
	Vector3 scale;

	int controller = 1;

	bool displayHelp = false;
	
	Vector2 levelScrollPos = Vector2.zero;
	int levelSelection = -1;
	//Vector2 goalScrollPos
	
	Level[] levels;

	MenuScreen screen = MenuScreen.MAIN;

	void Start()
	{
		controller = PlayerPrefs.GetInt("Controller",1);
		levels = Resources.LoadAll<Level>("Levels");
		System.Array.Sort<Level>(levels,LevelComparitor);
		screen = (MenuScreen)System.Enum.Parse(typeof(MenuScreen),PlayerPrefs.GetString("MenuScreen","MAIN"));
	}

	void Update()
	{
		if(screen == MenuScreen.MAIN) {
			if(displayHelp && Input.anyKeyDown)
				displayHelp = false;
		}
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

		switch(screen) {
			case MenuScreen.MAIN:
				MainGUI();
				break;
			case MenuScreen.LEVELSELECT:
				LevelSelectGUI();
				break;
			default:
				MainGUI();
				break;
		}

		if(screen == MenuScreen.MAIN) {
			MainGUI();
		} else if(screen == MenuScreen.LEVELSELECT) {

		}



		GUI.skin = unityDef;
		GUI.matrix = lastMat;
	}

	void MainGUI()
	{
		if(displayHelp) {
			GUI.DrawTexture(new Rect(0,0,1920,1080),howToPlayImage);
		} else {
			GUI.Label(new Rect(0,100,1920,100),"Balls!", defaultSkin.GetStyle("Title"));
			GUILayout.BeginArea(new Rect(800,300,320,480));

			GUILayout.Label ("Controls",defaultSkin.GetStyle("Score"));
			GUILayout.BeginVertical(defaultSkin.GetStyle ("GridBox"));

			controller = GUILayout.SelectionGrid(controller,controlSchemes,1);
			GUILayout.EndVertical();
			if(GUILayout.Button ("How To Play")) {
				displayHelp = true;
			}
			if(GUILayout.Button ("Select Level")) {
				screen = MenuScreen.LEVELSELECT;
			}
			GUILayout.Box ("",defaultSkin.GetStyle("EmptySpace"),GUILayout.Height (20));
			if(GUILayout.Button ("Credits")) {
				Application.LoadLevel("Credits");
			}
			GUILayout.EndArea ();
		}
	}


	void LevelSelectGUI()
	{
		GUILayout.BeginArea(new Rect(384,100,1152,880));
		GUILayout.Label("Level Select", defaultSkin.GetStyle("Title"));
		GUILayout.BeginArea(new Rect(0,100,1152,780));
		levelScrollPos = GUILayout.BeginScrollView(levelScrollPos);
		for(int i = 0; i < levels.Length; i++) {
			if(GUILayout.Button(levels[i].name,defaultSkin.GetStyle("LevelButton"))) {
				if(levelSelection == i)
					levelSelection = -1;
				else
					levelSelection = i;
				
			}
			//GUI.Label(GUILayoutUtility.GetLastRect(),"Difficulty: " + DifficultyToString(levels[i].difficulty),defaultSkin.GetStyle("DifficultyText"));
			if(levelSelection == i) {
				for(int j = 0; j < levels[i].possibleGoals.Length; j++) {
					GUILayout.BeginVertical();
					if(GUILayout.Button (levels[i].possibleGoals[j].displayName,defaultSkin.GetStyle("GoalButton"))) {
						//levels[i].goalIndex = j;
						LaunchLevel(i,j);
					}
					GUILayout.BeginHorizontal(defaultSkin.GetStyle("HighScoreBox"));
					for(int k = 0; k <= Level.maxDifficulty; k++) {
						GUILayout.Label(DifficultyToString(k) + ": " + levels[i].possibleGoals[j].FormatSuccess(levels[i].scoreLists[k].highScores[j]),
						                defaultSkin.GetStyle("YourBest"));
					}
					GUILayout.EndHorizontal();
					GUILayout.EndVertical();
				}
			}
		}
		GUILayout.EndScrollView();
		if(GUILayout.Button("Main Menu")) {
			screen = MenuScreen.MAIN;
		}
		GUILayout.EndArea();
		GUILayout.EndArea();
	}

	void LaunchLevel(int levelIndex, int goalIndex)
	{
		PlayerPrefs.SetInt("Controller",controller);
		PlayerPrefs.SetInt("GoalIndex",goalIndex);
		PlayerPrefs.Save();
		Application.LoadLevel(levels[levelIndex].sceneName);
	}

	string DifficultyToString(int diff)
	{
		if(diff == 0)
			return "Easy";
		if(diff == 1)
			return "Medium";
		if(diff == 2)
			return "Hard";
		if(diff == 3)
			return "Very Hard";
		return "UNKNOWN";
	}


}

