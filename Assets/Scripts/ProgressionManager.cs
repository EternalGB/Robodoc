using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ProgressionManager 
{

	//Dictionary<string,bool> levelUnlocked;
	//Dictionary<string,bool> goalUnlocked;

	static int levelUnlockedIndex;
	static List<int> goalUnlockedIndexes;

	public static bool LevelUnlocked(int levelIndex)
	{
		//LoadProgression();
		return levelIndex <= levelUnlockedIndex;
	}

	public static bool GoalUnlocked(int levelIndex, int goalIndex)
	{
		//LoadProgression();
		return goalIndex <= goalUnlockedIndexes[levelIndex];
	}

	public static void UpdateProgression(int levelIndex, int goalIndex)
	{
		if(levelIndex == levelUnlockedIndex)
			levelUnlockedIndex++;
		if(goalUnlockedIndexes[levelIndex] == goalIndex)
			goalUnlockedIndexes[levelIndex]++;
		SaveProgression();
	}

	public static void LoadProgression()
	{
		int levelUnlocked;
		List<int> goalsUnlocked;
		if(!Util.TryLoadFromPlayerPrefs<int>("LevelUnlocked",out levelUnlocked)) {
			levelUnlockedIndex = 0;
		} else
			levelUnlockedIndex = levelUnlocked;
		if(!Util.TryLoadFromPlayerPrefs<List<int>>("GoalsUnlocked",out goalsUnlocked)) {
			Level[] levels = Resources.LoadAll<Level>("Levels");
			goalUnlockedIndexes = new List<int>();
			for(int i = 0; i < levels.Length; i++) {
				goalUnlockedIndexes.Add(0);
			}
		} else 
			goalUnlockedIndexes = goalsUnlocked;

	}

	public static void SaveProgression()
	{
		Util.SaveToPlayerPrefs<int>("LevelUnlocked",levelUnlockedIndex);
		Util.SaveToPlayerPrefs<List<int>>("GoalsUnlocked",goalUnlockedIndexes);
	}

	public static void ClearProgression()
	{
		PlayerPrefs.DeleteKey("LevelUnlocked");
		PlayerPrefs.DeleteKey("GoalsUnlocked");
	}

		
}

