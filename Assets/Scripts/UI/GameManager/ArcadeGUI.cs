using UnityEngine;
using System.Collections.Generic;

public class ArcadeGUI : GameGUI
{

	protected override void InitGame ()
	{
		Time.timeScale = 1;
		//GameObject.Instantiate(difficultyPrefabs[difficulty]);
		GameObject.Find("BGMusic").GetComponent<AudioSource>().Play();
		GameObject.FindWithTag("BallMachine").GetComponent<BallMachine>().StartSpawning();
	}

	protected override void DoGameEnd ()
	{
		ArcadeStats.HighScore = goal.EvaluateSuccess();
	}



}
