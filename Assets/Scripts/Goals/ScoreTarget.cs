using UnityEngine;
using System.Collections;

public class ScoreTarget : Goal
{

	public float targetScore;
	float startTime;

	void OnEnable()
	{
		startTime = Time.timeSinceLevelLoad;
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

}

