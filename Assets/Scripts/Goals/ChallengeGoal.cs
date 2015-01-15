using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class ChallengeGoal : Goal
{

	public float bronze,silver,gold;

	public abstract float GetRank();

}

