using UnityEngine;
using System.Collections.Generic;

public class ArcadeGUI : GameGUI
{

	public ArcadeProgression progression;

	protected override void InitGame ()
	{
		//Time.timeScale = 1;
		//GameObject.Instantiate(difficultyPrefabs[difficulty]);
		GameObject.Find("BGMusic").GetComponent<AudioSource>().Play();
		GameObject.FindWithTag("BallMachine").GetComponent<BallMachine>().StartSpawning();
		pb.BallCollect += BallCollectHandler;

		StartCountdown();
	}

	protected override void DoGameEnd ()
	{
		ArcadeStats.HighScore = goal.EvaluateSuccess();
		ArcadeStats.LongestChain = ScoreCalculator.Instance.longestChain;

		StartCoroutine(progression.UpdateProgression());
	}

	void BallCollectHandler(GameObject player, GameObject playerPart, GameObject ball)
	{
		if(ball.GetComponent<BadBall>()) {
			ArcadeStats.BadBallHit(ball.GetComponent<BadBall>());
		}
	}


}

