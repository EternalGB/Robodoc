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

		arcadeScoreDisplay.text = HighScores.GetScore("Arcade","Arcade",0).ToString();
	}

	public void LaunchArcade()
	{
		Application.LoadLevel("Arcade");
		PlayerPrefs.SetInt ("LevelIndex",-1);
		PlayerPrefs.SetString("LevelName","Arcade");
		PlayerPrefs.SetInt("Controller",controlScheme.index);
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

}

