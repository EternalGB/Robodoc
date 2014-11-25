using UnityEngine;
using System.Collections;

public class LongestChain : Goal
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

