using UnityEngine;
using System.Collections;

public class TagGoodBallsUntilCollect : TagBallsUntilCollect
{

	protected override bool BallValid (GameObject ball)
	{
		return ball.GetComponent<PointBall>() != null;
	}

}

