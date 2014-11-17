using UnityEngine;
using System.Collections;

public class Arcade : Goal
{

	public float initRatio;
	public float timeToScoreRatio;
	public float ratioDegradeRate;


	void OnEnable()
	{
		base.OnEnable();
		timeToScoreRatio = initRatio;
		ScoreCalculator.PlayerScored += AddTime;
	}

	void OnDisable()
	{
		//dunno if I have to do this but just in case...
		ScoreCalculator.PlayerScored -= AddTime;
	}

	void AddTime(float scoreIncrease)
	{
		lerpTimer = 0;
		timeRemaining += Mathf.Ceil(scoreIncrease/timeToScoreRatio);
		timeToScoreRatio += scoreIncrease/ratioDegradeRate;
	}
	
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

