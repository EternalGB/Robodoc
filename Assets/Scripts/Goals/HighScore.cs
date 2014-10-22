using UnityEngine;
using System.Collections;

public class HighScore : TimeLimitGoal
{

	public override float EvaluateSuccess ()
	{
		return ScoreCalculator.Instance.score;
	}

	public override void DisplayProgress (GUIStyle textStyle, GUIStyle predictionStyle)
	{
		GUILayout.Label("Score",textStyle);
		GUILayout.Label(ScoreCalculator.Instance.displayScore.ToString(), textStyle);
		GUILayout.Label("Next Score",textStyle);
		GUILayout.Label(ScoreCalculator.Instance.displayNextScore.ToString(), predictionStyle);
	}

	public override void DisplaySuccess (GUIStyle textStyle)
	{
		GUILayout.Label ("Score",textStyle);
		GUILayout.Label (EvaluateSuccess().ToString(),textStyle);
	}

	public override string FormatSuccess (float score)
	{
		return score.ToString();
	}
}

