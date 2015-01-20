using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectUI : MonoBehaviour
{

	public RectTransform tierGroup;
	public RectTransform levelGroup;
	public Scrollbar scrollbar;

	public GameObject tierButtonPrefab;
	public GameObject levelButtonPrefab;

	public int selectedTier;

	public LevelTiersList ltl;



	void Start()
	{
		RecreateLevelGUI();
	}

	void RecreateLevelGUI()
	{
		//delete any children
		foreach(RectTransform child in tierGroup)
		{
			if(child.gameObject.name != tierGroup.name)
				GameObject.Destroy(child.gameObject);
		}

		foreach(RectTransform child in levelGroup)
		{
			if(child.gameObject.name != levelGroup.name)
				GameObject.Destroy(child.gameObject);
		}

		//load progress
		ltl.LoadProgress();

		selectedTier = PlayerPrefs.GetInt("SelectedTier",0);

		if(ltl != null) {
			for(int i = 0; i < ltl.tiers.Count; i++) {
				Tier tier = ltl.tiers[i];
				//make tier button
				GameObject tierGO = (GameObject)GameObject.Instantiate(tierButtonPrefab);
				Button tierButton = tierGO.GetComponent<Button>();
				tierGO.GetComponentInChildren<Text>().text = tier.displayName;
				//set progress
				tierButton.interactable = tier.progress.unlocked;
				//add tier button to tierGroup
				tierGO.GetComponent<RectTransform>().SetParent(tierGroup);
				//link tier button callback - switch tab
				int tierNum = i;
				tierButton.onClick.AddListener( () => {
					PlayerPrefs.SetInt("SelectedTier",tierNum);
					RecreateLevelGUI();
				});

				//if the tier is the selected tier
				if(i == selectedTier) {
					tierButton.Select();
					//resize level group to fit all levels
					/*
					VerticalLayoutGroup levelGroupLayout = levelGroup.GetComponent<VerticalLayoutGroup>();
					float lbHeight = levelButtonPrefab.GetComponent<LayoutElement>().minHeight +
						levelGroupLayout.spacing;
					AdjustContentPaneHeight(levelGroup, tier.levels.Count*lbHeight + levelGroupLayout.padding.bottom + levelGroupLayout.padding.top);
					*/
					foreach(Level level in tier.levels) {
						//make level button
						GameObject levelGO = (GameObject)GameObject.Instantiate(levelButtonPrefab);
						Button levelButton = levelGO.GetComponent<Button>();
						levelGO.GetComponentInChildren<Text>().text = level.displayName;
						//set progress
						levelButton.interactable = level.progress.unlocked;
						levelButton.GetComponentInChildren<RankDisplay>().SetRank(level.progress.rank);
						levelButton.GetComponentInChildren<ScoreDisplay>().SetScore(level.progress.score);
						//add button to levelGroup
						levelButton.GetComponent<RectTransform>().SetParent(levelGroup);
						//link button callback - launch level
						Level thisLevel = level;
						levelButton.onClick.AddListener( () => {
							//pass references to the goal and ball machine
							string machinePath = AssetDatabase.GetAssetPath(thisLevel.ballMachinePrefab);
							string goalPath = AssetDatabase.GetAssetPath(thisLevel.goal);
							string levelPath = AssetDatabase.GetAssetPath(thisLevel);
							Util.SaveToPlayerPrefs<string>("MachinePath",TrimPathToResourcePath(machinePath));
							Util.SaveToPlayerPrefs<string>("GoalPath",TrimPathToResourcePath(goalPath));
							Util.SaveToPlayerPrefs<string>("LevelPath",TrimPathToResourcePath(levelPath));
							//actually launch the level
							//TODO
							//Application.LoadLevel(thisLevel.sceneName);
						});
					}
				}


			}
			/*
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
			*/
		}
		scrollbar.value = 1;
		//scrollbar.size = 1;
	}

	void LaunchLevel(int levelIndex)
	{

	}



	void AdjustContentPaneHeight(RectTransform rectTrans, float height)
	{
		//rectTrans.sizeDelta += new Vector2(0,heightChange);
		rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,height);

	}



	void Update()
	{
		/*
		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.F1)) {
			ChallengeProgression.UnlockAllTemporarily();
			RecreateLevelGUI();
		}
		#endif
		*/
	}

	//trim off anything from the from of the 
	static string TrimPathToResourcePath(string assetPath)
	{
		string[] names = assetPath.Split(new char[]{System.IO.Path.PathSeparator});
		bool recording = false;
		string result = "";
		for(int i = 0; i < names.Length; i++) {
			if(names[i] == "Resources")
				recording = true;
			if(recording) {
				result += names[i];
				if(i != names.Length-1)
					result += System.IO.Path.PathSeparator;
			}
		}
		return result;

	}
}

