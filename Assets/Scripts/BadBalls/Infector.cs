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
			Debug.Log ("Infector Ball Applying Effect");
			ball.GetInfected(infectionMat,spreadInterval);
			Destroy();
		}
	}

	
	
}

