using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{

	public GameObject mainMenu;
	public GameObject levelSelect;

	public GameObject tutorialButton;

	public TextList controlScheme;
	public Text arcadeScoreDisplay;

	void Start()
	{

		arcadeScoreDisplay.text = ArcadeStats.HighScore.ToString();
		controlScheme.DisplayText(PlayerPrefs.GetInt("Controller",1));
		tutorialButton.SetActive(PlayerPrefs.GetInt("TutorialCompleted",0) != 0);
	}

	public void LaunchArcade()
	{
		if(PlayerPrefs.GetInt("TutorialCompleted",0) == 0)
			LaunchTutorial();
		else {
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

	public void DisplayMainMenu()
	{
		mainMenu.SetActive(true);
		levelSelect.SetActive(false);
	}

	public void DisplayLevelSelect()
	{
		mainMenu.SetActive(false);
		levelSelect.SetActive(true);
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
		ChallengeHighScores.Clear();
		ArcadeStats.Clear();
		PlayerPrefs.DeleteKey("TutorialCompleted");
		Application.LoadLevel(Application.loadedLevel);
	}

}

