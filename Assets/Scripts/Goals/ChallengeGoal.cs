using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class ChallengeGoal : Goal
{

	public abstract int ScoreComparitor(float score1, float score2);

}

