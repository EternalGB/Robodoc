using UnityEngine;
using System.Collections;

public class BiggestCombo : TimeLimitGoal
{

	public override float EvaluateSuccess ()
	{
		return ScoreCalculator.Instance.biggestScore;
	}

	public override void DisplayProgress (GUIStyle textStyle, GUIStyle predictionStyle)
	{
		GUILayout.Label("Biggest Score",textStyle);
		GUILayout.Label(ScoreCalculator.Instance.displayBiggestScore.ToString(), textStyle);
		GUILayout.Label("Next Score",textStyle);
		GUILayout.Label(ScoreCalculator.Instance.displayNextScore.ToString(), predictionStyle);
	}

	public override void DisplaySuccess (GUIStyle textStyle)
	{
		GUILayout.Label ("Biggest Single Score",textStyle);
		GUILayout.Label (EvaluateSuccess().ToString(),textStyle);
	}

	public override string FormatSuccess (float score)
	{
		return score.ToString();
	}
}

