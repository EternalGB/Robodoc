using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{

	public GameObject mainMenu;

	public GameObject tutorialButton;

	public ArrowedSelector controlScheme;
	public Text arcadeScoreDisplay;

	public GameObject currentUI;

	public GameObject tutorialConfirmationWindow;

	void Start()
	{
		currentUI.SetActive(true);

		arcadeScoreDisplay.text = ArcadeStats.HighScore.ToString();
		controlScheme.ChangeText(PlayerPrefs.GetInt("Controller",1));
		tutorialButton.SetActive(PlayerPrefs.GetInt("TutorialCompleted",0) != 0);
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
			UpdateControlScheme();
			PlayerPrefs.Save();
		}
	}

	public void LaunchTutorial()
	{
		Application.LoadLevel("Tutorial");
		UpdateControlScheme();
		PlayerPrefs.Save();
	}

	public void SwapUI(GameObject ui)
	{

		currentUI.SetActive(false);
		ui.SetActive(true);
		currentUI = ui;
		UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentUI);
		//Debug.Log (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name + " is now selected");
	}

	public void UpdateControlScheme()
	{
		PlayerPrefs.SetInt("Controller",controlScheme.index);
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

