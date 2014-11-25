using UnityEngine;
using System.Collections;

public class HighScore : Goal
{

	public override float EvaluateSuccess ()
	{
		return ScoreCalculator.Instance.score;
	}

	public override string FormatSuccess (float score)
	{
		return score.ToString();
	}
}

