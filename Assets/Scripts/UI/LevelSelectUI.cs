using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectUI : MonoBehaviour
{
	
	public GameObject levelButtonPrefab;
	public GameObject goalButtonPrefab;
	public GameObject goalGroupPrefab;
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
			

			GameObject goalGroup = (GameObject)GameObject.Instantiate(goalGroupPrefab);
			goalGroup.GetComponent<RectTransform>().sizeDelta += new Vector2(0,levels[i].possibleGoals.Length*lbHeight);
			goalGroup.GetComponent<RectTransform>().SetParent(transform);
			for(int j = 0; j < levels[i].possibleGoals.Length; j++) {
				GameObject goalGO = (GameObject)GameObject.Instantiate(goalButtonPrefab);
				//goalGO.transform.parent = goalGroup.transform;
				
				RectTransform gbRect = goalGO.GetComponent<RectTransform>();
				gbRect.SetParent(goalGroup.transform);
				Button gb = goalGO.GetComponent<Button>();
				gb.interactable = ProgressionManager.LevelUnlocked(i);
				gb.GetComponentInChildren<Text>().text = levels[i].possibleGoals[j].displayName;
				
			}

			float gbHeight = levels[i].possibleGoals.Length*
				(goalButtonPrefab.GetComponent<LayoutElement>().minHeight 
				 + goalGroup.GetComponent<VerticalLayoutGroup>().spacing);
			lb.onClick.AddListener(
				() => {ToggleGoalGroup(goalGroup,gbHeight);});
			
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

	void ToggleGoalGroup(GameObject goalGroup, float goalButtonHeight)
	{
		if(goalGroup.activeInHierarchy) {
			goalGroup.SetActive(false);
			goalGroup.GetComponent<RectTransform>().sizeDelta -= new Vector2(0,goalButtonHeight);
			AdjustContentPaneHeight(-goalButtonHeight);
		} else {
			goalGroup.SetActive(true);
			goalGroup.GetComponent<RectTransform>().sizeDelta += new Vector2(0,goalButtonHeight);
			AdjustContentPaneHeight(goalButtonHeight);
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

