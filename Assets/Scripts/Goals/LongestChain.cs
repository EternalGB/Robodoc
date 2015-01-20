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


}

