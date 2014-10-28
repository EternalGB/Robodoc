using UnityEngine;
using System.Collections;

public class Endless : Goal
{

	public float initTime;
	public float timeRemaining;
	public float initRatio;
	public float timeToScoreRatio;
	public float ratioDegradeRate;
	float lerpTimer;
	float lerpSpeed = 0.25f;
	public float displayTime;

	void OnEnable()
	{
		lerpTimer = 0;
		timeRemaining = initTime;
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

	public override bool Completed ()
	{
		//kinda doing weird stuff here because Completed will always be called every update by GameGUI
		displayTime = Mathf.Floor(Mathf.Lerp (displayTime,timeRemaining,lerpTimer));
		lerpTimer = Mathf.Clamp (lerpTimer + lerpSpeed*Time.deltaTime,0,1f);
		timeRemaining -= Time.deltaTime;
		return timeRemaining <= 0;
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

