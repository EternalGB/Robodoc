using UnityEngine;
using System.Collections.Generic;

public class LevelTiersList : ScriptableObject
{

	public List<Tier> tiers;

	const string key = "ChallengeLevels";

	void OnEnable()
	{
		LoadProgress();
	}

	public void SaveProgress()
	{
		if(tiers != null) {
			//Util.SaveToPlayerPrefs<List<Tier>>(key,tiers);
			foreach(Tier tier in tiers) {
				tier.SaveProgress();
			}

		}
	}

	public void LoadProgress()
	{
		/*
		List<Tier> loadedTiers;
		if(Util.TryLoadFromPlayerPrefs<List<Tier>>(key, out loadedTiers)) {
			tiers = loadedTiers;
		} else {
			//init new tier list
			tiers = new List<Tier>();
		}
		*/
		if(tiers != null) {
			foreach(Tier tier in tiers) {
				tier.LoadProgress();
			}
			
		}
	}



}

