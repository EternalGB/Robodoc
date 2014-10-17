using UnityEngine;
using System.Collections;

public class LongestChain : TimeLimitGoal
{

	public override float EvaluateSuccess ()
	{
		return ScoreCalculator.Instance.longestChain;
	}

	public override void DisplayProgress (GUIStyle textStyle, GUIStyle predictionStyle)
	{
		GUILayout.Label("Longest Chain",textStyle);
		GUILayout.Label(ScoreCalculator.Instance.longestChain.ToString(), textStyle);
		GUILayout.Label("Next Chain",textStyle);
		GUILayout.Label(ScoreCalculator.Instance.nextLongestChain.ToString(), predictionStyle);
	}
		
	public override void DisplaySuccess (GUIStyle textStyle)
	{
		GUILayout.Label ("Longest Chain",textStyle);
		GUILayout.Label (EvaluateSuccess().ToString(),textStyle);
	}

	public override string FormatSuccess (float score)
	{
		return score.ToString();
	}
}

