using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenuUI : MonoBehaviour
{

	public GameObject mainMenu;
	public List<GameObject> menuScreens;
	GameObject currentScreen;
	Dictionary<string,GameObject> menuScreenDict;

	public GameObject tutorialButton;

	//public ArrowedSelector controlScheme;
	public Text arcadeScoreDisplay;

	public GameObject tutorialConfirmationWindow;

	void Start()
	{
		arcadeScoreDisplay.text = ArcadeStats.HighScore.ToString();
		//controlScheme.ChangeText(PlayerPrefs.GetInt("Controller",1));
		tutorialButton.SetActive(PlayerPrefs.GetInt("TutorialCompleted",0) != 0);

		menuScreenDict = new Dictionary<string,GameObject>();
		foreach(GameObject screen in menuScreens) {
			menuScreenDict.Add(screen.name,screen);
		}
		currentScreen = mainMenu;

		ChangeUIScreen(PlayerPrefs.GetString("MenuScreen","MainMenu"));
		PlayerPrefs.DeleteKey("MenuScreen");
	}

	void Update()
	{
		/*
		if(Input.anyKeyDown && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
			Debug.Log (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name + " is now selected");
			*/
	}

	public void LaunchArcade()
	{

		if(PlayerPrefs.GetInt("TutorialCompleted",0) == 0) {
			mainMenu.SetActive(false);
			tutorialConfirmationWindow.SetActive(true);
		} else {
			Application.LoadLevel("Arcade");
			PlayerPrefs.SetInt ("LevelIndex",-1);
			PlayerPrefs.SetString("LevelName","Arcade");
			PlayerPrefs.Save();
		}
	}

	public void LaunchTutorial()
	{
		Application.LoadLevel("Tutorial");
		PlayerPrefs.Save();
	}

	public void ChangeUIScreen(string name)
	{
		GameObject screen;
		if(menuScreenDict.TryGetValue(name, out screen)) {
			ChangeUIScreen(screen);
		} else {
			Debug.LogError("No such screen in dict called " + name);
			ChangeUIScreen(mainMenu);
		}
	}

	void ChangeUIScreen(GameObject screen)
	{
		currentScreen.SetActive(false);
		screen.SetActive(true);
		UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(screen);
		currentScreen = screen;
		//Debug.Log (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name + " is now selected");
	}


	public void GoToScene(string sceneName)
	{
		Application.LoadLevel(sceneName);
	}

	public void ResetProgress()
	{
		//TODO challenge progress clearing
		//ChallengeHighScores.Clear();
		ArcadeStats.Clear();
		PlayerPrefs.DeleteKey("TutorialCompleted");
		Application.LoadLevel(Application.loadedLevel);
		ArcadeProgression.Clear();

		/*
		#if UNITY_EDITOR
		PlayerPrefs.SetInt("TutorialCompleted",1);
		#endif
		*/
	}

	public void SetTutorialCompleted()
	{
		PlayerPrefs.SetInt("TutorialCompleted",1);
	}

}

