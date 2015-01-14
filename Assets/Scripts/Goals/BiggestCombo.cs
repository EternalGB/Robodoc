using UnityEngine;
using System.Collections;

public class BiggestCombo : ChallengeGoal
{

	public override float EvaluateSuccess ()
	{
		return ScoreCalculator.Instance.biggestScore;
	}

	public override string FormatSuccess (float score)
	{
		return score.ToString();
	}

	public override float GetRank ()
	{
		if(EvaluateSuccess() >= gold)
			return 3;
		else if(EvaluateSuccess() >= silver)
			return 2;
		else if(EvaluateSuccess() >= bronze)
			return 1;
		else
			return 0;
	}
}

