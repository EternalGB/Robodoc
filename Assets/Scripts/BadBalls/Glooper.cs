using UnityEngine;
using System.Collections;

public class Glooper : BadBall
{
	public float gloopDuration;
	public Material gloopMaterial;

	protected override void ApplyEffect (Transform target)
	{
		GameObject player = GameObject.Find("PlayerBall");
		if(player) {
			player.GetComponent<PlayerBall>().GetGlooped(gloopDuration,gloopMaterial);
			Destroy();
		}
	}

}

