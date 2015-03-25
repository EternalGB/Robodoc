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

	public override void ResetGoal ()
	{
		lerpTimer = 0;
		timeRemaining = 0;
		//startTime = 0;
	}

	public override int ScoreComparitor (float score1, float score2)
	{
		return (int)Mathf.Sign(score2 - score1);
	}
	

}

