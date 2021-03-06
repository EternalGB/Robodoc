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
	
	public override string FormatSuccess (float score)
	{
		return score.ToString();
	}
}

