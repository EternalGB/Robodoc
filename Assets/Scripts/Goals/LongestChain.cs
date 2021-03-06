using UnityEngine;
using System.Collections;

public class LongestChain : ChallengeGoal
{

	public override float EvaluateSuccess ()
	{
		return ScoreCalculator.Instance.longestChain;
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

