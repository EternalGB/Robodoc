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
				GameObject tierGO = (GameObject)Instantiate(tierButtonPrefab);
				Button tierButton = tierGO.GetComponent<Button>();

				//set progress
				tierButton.interactable = tier.progress.unlocked;
				if(tier.progress.unlocked) {
					tierGO.GetComponentInChildren<Text>().text = tier.displayName;
				} else {
					tierGO.GetComponentInChildren<Text>().text = "Unlocked at " + tier.pointsNeededToUnlock + " RP";
				}
				//add tier button to tierGroup
				tierGO.GetComponent<RectTransform>().SetParent(tierGroup,false);
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
						if(level.progress.unlocked) {
							levelGO.GetComponentInChildren<Text>().text = level.displayName;
						} else {
							levelGO.GetComponentInChildren<Text>().text = "Unlocked at " + level.pointsNeededToUnlock + " RP";
						}

						//set progress
						levelButton.interactable = level.progress.unlocked;
						levelButton.GetComponentInChildren<RankDisplay>().SetRank(level.progress.rank);
						levelButton.GetComponentInChildren<ScoreDisplay>().SetScore(level.goal.FormatSuccess(level.progress.score));
						//add button to levelGroup
						levelButton.GetComponent<RectTransform>().SetParent(levelGroup,false);
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

							Application.LoadLevel(thisLevel.sceneName);
						});
					}
				}
			}
		}
		//scrollbar.value = 1;
		//scrollbar.size = 1;
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
		string[] names = assetPath.Split(new char[]{'/'});
		bool recording = false;
		string result = "";
		for(int i = 0; i < names.Length; i++) {
			if(names[i] == "Resources") {
				recording = true;
				i++;
			}
			if(recording) {
				//if the last token remove any file extensions
				if(i == names.Length-1) {
					string[] tokens = names[i].Split( new char[]{'.'});
					for(int j = 0; j < tokens.Length; j++) {
						if(j != tokens.Length-1)
							result += tokens[j];
					}
				} else {
					result += names[i];
					result += '/';
				}
			}
		}
		return result;

	}
}

