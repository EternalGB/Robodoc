using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Level : ScriptableObject
{

	public string displayName;
	public string sceneName;
	public ChallengeGoal goal;
	public BallMachine ballMachinePrefab;
	public float rank;
	public bool unlocked;

}

