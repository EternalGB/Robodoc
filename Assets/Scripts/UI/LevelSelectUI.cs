using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectUI : MonoBehaviour
{
	
	public GameObject levelButtonPrefab;
	public GameObject goalButtonPrefab;
	public GameObject goalGroupPrefab;
	public GameObject highScoreGroupPrefab;
	public GameObject highScoreTextPrefab;
	Level[] levels;
	RectTransform rectTrans;


	void Start()
	{

		rectTrans = GetComponent<RectTransform>();
		RecreateLevelGUI();
	}

	void RecreateLevelGUI()
	{
		foreach(RectTransform child in transform)
		{
			if(child.gameObject.name != gameObject.name)
				GameObject.Destroy(child.gameObject);
		}

		levels = Resources.LoadAll<Level>("Levels");
		float lbHeight = levelButtonPrefab.GetComponent<LayoutElement>().minHeight +
			GetComponent<VerticalLayoutGroup>().spacing;
		AdjustContentPaneHeight(levels.Length*lbHeight);
		for(int i = 0; i < levels.Length; i++) {
			GameObject levelGO = (GameObject)GameObject.Instantiate(levelButtonPrefab);
			
			RectTransform lbRect = levelGO.GetComponent<RectTransform>();
			lbRect.SetParent(transform);
			Button lb = levelGO.GetComponent<Button>();
			lb.interactable = ProgressionManager.LevelUnlocked(i);
			lb.GetComponentInChildren<Text>().text = levels[i].displayName;
			
			//create goal group
			GameObject goalGroup = (GameObject)GameObject.Instantiate(goalGroupPrefab);
			goalGroup.GetComponent<RectTransform>().sizeDelta += new Vector2(0,levels[i].possibleGoals.Length*lbHeight);
			goalGroup.GetComponent<RectTransform>().SetParent(transform);
			for(int j = 0; j < levels[i].possibleGoals.Length; j++) {
				//Create each goal button
				GameObject goalGO = (GameObject)GameObject.Instantiate(goalButtonPrefab);
				
				RectTransform gbRect = goalGO.GetComponent<RectTransform>();
				gbRect.SetParent(goalGroup.transform);
				Button gb = goalGO.GetComponent<Button>();
				gb.interactable = ProgressionManager.LevelUnlocked(i);
				gb.GetComponentInChildren<Text>().text = levels[i].possibleGoals[j].displayName;
				//this seems weird but we have to make new copies of these variables as their references
				//get passed to the delegate not the values
				int levelIndex = i;
				int goalIndex = j;
				gb.onClick.AddListener(() => {LaunchLevel(levelIndex,goalIndex);});

				//add the high scores
				List<float> scores = ChallengeHighScores.GetScores(levels[i],levels[i].possibleGoals[j]);
				GameObject scoreGroup = (GameObject)GameObject.Instantiate(highScoreGroupPrefab);
				scoreGroup.GetComponent<RectTransform>().SetParent(goalGroup.transform);
				for(int k = 0; k < scores.Count; k++) {
					GameObject scoreText = (GameObject)GameObject.Instantiate(highScoreTextPrefab);
					scoreText.GetComponent<RectTransform>().SetParent(scoreGroup.transform);
					scoreText.GetComponent<Text>().text = DifficultyToString(k) + ": " + scores[k];
				}
			}



			float goalGroupHeight = levels[i].possibleGoals.Length*
				(goalButtonPrefab.GetComponent<LayoutElement>().minHeight 
				 + goalGroup.GetComponent<VerticalLayoutGroup>().spacing);
			lb.onClick.AddListener(
				() => {ToggleGoalGroup(goalGroup,goalGroupHeight);});
			
			goalGroup.gameObject.SetActive(false);
		}
	}

	void LaunchLevel(int levelIndex, int goalIndex)
	{
		//PlayerPrefs.SetInt("Controller",controller);
		PlayerPrefs.SetInt("GoalIndex",goalIndex);
		PlayerPrefs.SetInt ("LevelIndex",levelIndex);
		PlayerPrefs.SetString("LevelName",levels[levelIndex].name);
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

	void ToggleGoalGroup(GameObject goalGroup, float groupHeight)
	{
		if(goalGroup.activeInHierarchy) {
			goalGroup.SetActive(false);
			goalGroup.GetComponent<RectTransform>().sizeDelta -= new Vector2(0,groupHeight);
			AdjustContentPaneHeight(-groupHeight);
		} else {
			goalGroup.SetActive(true);
			goalGroup.GetComponent<RectTransform>().sizeDelta += new Vector2(0,groupHeight);
			AdjustContentPaneHeight(groupHeight);
		}


	}

	void AdjustContentPaneHeight(float heightChange)
	{
		rectTrans.sizeDelta += new Vector2(0,heightChange);
		/*
		if(heightChange > 0)
			if(rectTrans.sizeDelta.y < heightChange)
				rectTrans.sizeDelta += new Vector2(0,heightChange-rectTrans.sizeDelta.y);
		else {
			rectTrans.sizeDelta += new Vector2(0,heightChange);
		}
		*/
	}



	void Update()
	{
		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.F1)) {
			ProgressionManager.UnlockAllTemporarily();
			RecreateLevelGUI();
		}
		#endif
	}
}

