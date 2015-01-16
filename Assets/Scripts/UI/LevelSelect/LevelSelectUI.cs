using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectUI : MonoBehaviour
{

	public RectTransform tierGroup;
	public RectTransform levelGroup;
	public Scrollbar scrollbar;

	public GameObject tierButtonPrefab;
	public GameObject levelButtonPrefab;


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

		if(ltl != null) {
			foreach(Tier tier in ltl.tiers) {
				//make tier button
				GameObject tierGO = (GameObject)GameObject.Instantiate(tierButtonPrefab);
				Button tierButton = tierGO.GetComponent<Button>();
				tierGO.GetComponentInChildren<Text>().text = tier.name;
				//set progress
				tierButton.interactable = tier.progress.unlocked;
				//add tier button to tierGroup
				tierGO.GetComponent<RectTransform>().SetParent(tierGroup);
				//link tier button callback - switch tab
				tierButton.onClick.AddListener( () => {});

				//resize level group to fit all levels
				VerticalLayoutGroup levelGroupLayout = levelGroup.GetComponent<VerticalLayoutGroup>();
				float lbHeight = levelButtonPrefab.GetComponent<LayoutElement>().minHeight +
					levelGroupLayout.spacing;
				AdjustContentPaneHeight(levelGroup, tier.levels.Count*lbHeight + levelGroupLayout.padding.bottom + levelGroupLayout.padding.top);

				foreach(Level level in tier.levels) {
					//make level button
					GameObject levelGO = (GameObject)GameObject.Instantiate(levelButtonPrefab);
					Button levelButton = levelGO.GetComponent<Button>();
					levelGO.GetComponentInChildren<Text>().text = level.displayName;
					//set progress
					levelButton.interactable = level.progress.unlocked;
					levelButton.GetComponentInChildren<RankDisplay>().SetRank(level.progress.rank);
					//add button to levelGroup
					levelButton.GetComponent<RectTransform>().SetParent(levelGroup);
					//link button callback - launch level
					levelButton.onClick.AddListener( () => {});
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
}

