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
		MAIN,LEVELSELECT,CONTROLS,HELP
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
		if(screen == MenuScreen.HELP || screen == MenuScreen.CONTROLS) {
			if(Input.anyKeyDown)
				screen = MenuScreen.MAIN;
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
			case MenuScreen.CONTROLS:
				ControlsGUI();
				break;
			case MenuScreen.HELP:
				HelpGUI();
				break;
			default:
				MainGUI();
				break;
		}


		GUI.skin = unityDef;
		GUI.matrix = lastMat;
	}

	void MainGUI()
	{
		GUI.Label(new Rect(0,100,1920,100),"Balls!", defaultSkin.GetStyle("Title"));
		GUILayout.BeginArea(new Rect(800,300,320,480));

		GUILayout.Label ("Controls",defaultSkin.GetStyle("Score"));
		GUILayout.BeginVertical(defaultSkin.GetStyle ("GridBox"));

		controller = GUILayout.SelectionGrid(controller,controlSchemes,1);
		GUILayout.EndVertical();
		if(GUILayout.Button ("How To Play")) {
			screen = MenuScreen.HELP;
		}
		if(GUILayout.Button ("Controls")) {
			screen = MenuScreen.CONTROLS;
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

	void ControlsGUI()
	{
		GUILayout.BeginArea(new Rect(640,160,640,680),defaultSkin.GetStyle("MenuBox"));
		GUILayout.Label ("Keyboard",defaultSkin.GetStyle("Title"));
		GUILayout.Label ("Movement",defaultSkin.GetStyle("Score"));
		GUILayout.Label ("W,A,S,D or Arrow Keys",defaultSkin.GetStyle ("SmallerText"));
		GUILayout.Label ("Spin",defaultSkin.GetStyle("Score"));
		GUILayout.Label ("'g' and 'h' or ',' and '.'",defaultSkin.GetStyle ("SmallerText"));
		GUILayout.Label ("Bombs",defaultSkin.GetStyle("Score"));
		GUILayout.Label ("Space",defaultSkin.GetStyle ("SmallerText"));

		GUILayout.Label ("Mouse",defaultSkin.GetStyle("Title"));
		GUILayout.Label ("Movement",defaultSkin.GetStyle("Score"));
		GUILayout.Label ("Mouse Cursor",defaultSkin.GetStyle ("SmallerText"));
		GUILayout.Label ("Spin",defaultSkin.GetStyle("Score"));
		GUILayout.Label ("Left and Right Mouse Buttons",defaultSkin.GetStyle ("SmallerText"));
		GUILayout.Label ("Bombs",defaultSkin.GetStyle("Score"));
		GUILayout.Label ("Middle Mouse Button",defaultSkin.GetStyle ("SmallerText"));
		GUILayout.EndArea();
	}

	void HelpGUI()
	{
		GUI.DrawTexture(new Rect(0,0,1920,1080),howToPlayImage);
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
		switch(diff) {
		case 0:
			return "Easy";
		case 1:
			return "Medium";
		case 2:
			return "Hard";
		case 3:
			return "Very Hard";
		default:
			return "UNKNOWN";
		}
	}


}

