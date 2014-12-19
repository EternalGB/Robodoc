using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TagBadBallsAmount : TagBallAmount
{

	protected override bool BallValid (GameObject ball)
	{
		return ball.GetComponent<BadBall>() != null;
	}

}

