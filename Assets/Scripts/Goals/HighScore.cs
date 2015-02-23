using UnityEngine;
using System.Collections;

[System.Serializable]
public class HighScore : ChallengeGoal
{

	public override float EvaluateSuccess ()
	{
		return ScoreCalculator.Instance.score;
	}

	public override string FormatSuccess (float score)
	{
		return score.ToString();
	}

	public override int ScoreComparitor (float score1, float score2)
	{
		return (int)Mathf.Sign(score1 - score2);
	}



}

