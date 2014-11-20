using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{

	public GameObject mainMenu;
	public GameObject levelSelect;

	public TextList controlScheme;
	public Text arcadeScoreDisplay;

	void Start()
	{

		arcadeScoreDisplay.text = ArcadeStats.HighScore.ToString();
		controlScheme.DisplayText(PlayerPrefs.GetInt("Controller",1));
	}

	public void LaunchArcade()
	{
		Application.LoadLevel("Arcade");
		PlayerPrefs.SetInt ("LevelIndex",-1);
		PlayerPrefs.SetString("LevelName","Arcade");

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
		Application.LoadLevel(Application.loadedLevel);
	}

}

