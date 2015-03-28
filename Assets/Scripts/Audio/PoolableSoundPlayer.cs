using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PoolableSoundPlayer : PoolableObject
{

	public override void Destroy ()
	{
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().clip = null;
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().pitch = 1;

		base.Destroy ();
	}

	void Update()
	{
		if(!GetComponent<AudioSource>().isPlaying) {
			Destroy();
		}
	}

}

