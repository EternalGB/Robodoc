using UnityEngine;
using System.Collections;

public class Infector : BadBall
{

	public Material infectionMat;
	public float spreadInterval;


	public override void ApplyEffect (Transform target)
	{
		AttachedBall ball;
		if(ball = target.GetComponent<AttachedBall>()) {
			ball.GetInfected(infectionMat,spreadInterval);
			Destroy();
		}
	}

	
	
}

