using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ChallengeProgression 
{

	static int levelUnlockedIndex;

	public static bool LevelUnlocked(int levelIndex)
	{
		//LoadProgression();
		return levelIndex <= levelUnlockedIndex;
	}
	
	public static void UpdateProgression(int levelIndex)
	{
		if(levelIndex == levelUnlockedIndex)
			levelUnlockedIndex++;
		SaveProgression();
	}

	public static void LoadProgression()
	{
		int levelUnlocked;
		if(!Util.TryLoadFromPlayerPrefs<int>("LevelUnlocked",out levelUnlocked)) {
			levelUnlockedIndex = 0;
		} else
			levelUnlockedIndex = levelUnlocked;

	}

	public static void SaveProgression()
	{
		Util.SaveToPlayerPrefs<int>("LevelUnlocked",levelUnlockedIndex);
	}

	public static void ClearProgression()
	{
		PlayerPrefs.DeleteKey("LevelUnlocked");
	}

	public static void UnlockAllTemporarily()
	{
		Level[] levels = Resources.LoadAll<Level>("Levels");
		levelUnlockedIndex = levels.Length;
	}

		
}

