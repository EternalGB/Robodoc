using UnityEngine;
using System.Collections;

public class Freezer : BadBall
{

	public float freezeDuration;
	public Material frozenMat;

	public override void ApplyEffect (Transform target)
	{
		GameObject player = GameObject.Find("PlayerBall");
		if(player) {
			player.GetComponent<PlayerBall>().GetFrozen(freezeDuration,frozenMat);
			Destroy();
		}
	}


}

