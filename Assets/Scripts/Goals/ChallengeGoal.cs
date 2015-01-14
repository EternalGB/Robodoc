using UnityEngine;
using System.Collections;

public abstract class ChallengeGoal : Goal
{

	public float bronze,silver,gold;

	public abstract float GetRank();

}

