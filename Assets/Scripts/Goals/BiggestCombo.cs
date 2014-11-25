using UnityEngine;
using System.Collections;

public class BiggestCombo : Goal
{

	public override float EvaluateSuccess ()
	{
		return ScoreCalculator.Instance.biggestScore;
	}

	public override string FormatSuccess (float score)
	{
		return score.ToString();
	}
}

