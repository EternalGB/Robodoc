using UnityEngine;
using System.Collections.Generic;

public class LevelTiersList : ScriptableObject
{

	public List<Tier> tiers;

	const string key = "ChallengeLevels";

	public void Save()
	{
		if(tiers != null)
			Util.SaveToPlayerPrefs<List<Tier>>(key,tiers);
	}

	public void Load()
	{
		List<Tier> loadedTiers;
		if(Util.TryLoadFromPlayerPrefs<List<Tier>>(key, out loadedTiers)) {
			tiers = loadedTiers;
		} else {
			//init new tier list
			tiers = new List<Tier>();
		}
	}



}

