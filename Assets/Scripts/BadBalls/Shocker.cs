using UnityEngine;
using System.Collections;

public class Shocker : BadBall
{

	public float shockDuration;
	public Material shockMat;

	protected override void ApplyEffect (Transform target)
	{
		GameObject player = GameObject.Find("PlayerBall");
		if(player) {
			player.GetComponent<PlayerBall>().GetShocked(shockDuration,shockMat);
			Destroy();
		}
	}
	

}

