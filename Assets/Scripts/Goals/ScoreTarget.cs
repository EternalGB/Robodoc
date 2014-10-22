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

	public override void DisplayProgress (GUIStyle textStyle, GUIStyle predictionStyle)
	{
		GUILayout.Label("Target Score",textStyle);
		GUILayout.Label(targetScore.ToString(), textStyle);
		GUILayout.Label("Score",textStyle);
		GUILayout.Label(ScoreCalculator.Instance.displayScore.ToString(), textStyle);
		GUILayout.Label("Next Score",textStyle);
		GUILayout.Label(ScoreCalculator.Instance.displayNextScore.ToString(), predictionStyle);
	}

	public override void DisplaySuccess (GUIStyle textStyle)
	{
		GUILayout.Label ("Time",textStyle);
		GUILayout.Label (FormatSuccess(EvaluateSuccess()),textStyle);
	}

	public override string FormatSuccess (float score)
	{
		return Util.FormatTime(score);
	}

}

