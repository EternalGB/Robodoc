using UnityEngine;
using System.Collections;

public class Tutorial : Goal
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

