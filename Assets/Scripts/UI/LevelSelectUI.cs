using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectUI : MonoBehaviour
{
	
	public GameObject levelButtonPrefab;
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
			lb.interactable = ChallengeProgression.LevelUnlocked(i);
			lb.GetComponentInChildren<Text>().text = levels[i].displayName;

			//TODO launch level
			//lb.onClick.AddListener();
		}
	}

	void LaunchLevel(int levelIndex)
	{
		PlayerPrefs.SetInt ("LevelIndex",levelIndex);
		PlayerPrefs.SetString("LevelName",levels[levelIndex].name);
		PlayerPrefs.Save();
		Application.LoadLevel(levels[levelIndex].sceneName);
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
			ChallengeProgression.UnlockAllTemporarily();
			RecreateLevelGUI();
		}
		#endif
	}
}

