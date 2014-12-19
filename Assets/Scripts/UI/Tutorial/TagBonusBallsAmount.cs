using UnityEngine;
using System.Collections;

public class TagBonusBallsAmount : TagBallAmount
{

	protected override bool BallValid (GameObject ball)
	{
		return ball.name == "BonusBall";
	}

}

