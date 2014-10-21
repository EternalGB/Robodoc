using UnityEngine;
using System.Collections;

public class LevelManager
{

	static int activeGoalIndex;
	static Level activeLevel;

	public static void SetActiveLevel(string name)
	{
		activeLevel = Resources.Load<Level>("Levels/" + PlayerPrefs.GetString(name,"01-Circle"));
	}

	public static Level GetActiveLevel()
	{
		return activeLevel;
	}

	public static void SetActiveGoal(int index)
	{
		activeGoalIndex = index;
	}

	public static Goal GetActiveGoal()
	{
		if(activeLevel != null)
			return activeLevel.possibleGoals[activeGoalIndex];
		else
			return null;
	}

	public static int GetActiveGoalIndex()
	{
		return activeGoalIndex;
	}


}

