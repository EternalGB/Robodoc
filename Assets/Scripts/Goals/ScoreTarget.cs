using UnityEngine;
using System.Collections;

public class ScoreTarget : ChallengeGoal
{

	public float targetScore;
	float startTime;

	void OnEnable()
	{
		startTime = Time.timeSinceLevelLoad;
		timeRemaining = 0;
	}

	protected override void UpdateTime ()
	{
		timeRemaining += Time.deltaTime;
	}

	public override bool Completed ()
	{
		return ScoreCalculator.Instance.score >= targetScore;
	}

	public override float EvaluateSuccess ()
	{
		return Time.timeSinceLevelLoad - startTime;
	}

	public override string FormatSuccess (float score)
	{
		return Util.FormatTime(score);
	}

	public override float GetRank ()
	{
		if(EvaluateSuccess() <= gold)
			return 3;
		else if(EvaluateSuccess() <= silver)
			return 2;
		else if(EvaluateSuccess() <= bronze)
			return 1;
		else
			return 0;
	}

}

