using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PoolableSoundPlayer : PoolableObject
{

	public override void Destroy ()
	{
		audio.Stop();
		audio.clip = null;
		audio.loop = false;
		audio.pitch = 1;

		base.Destroy ();
	}

	void Update()
	{
		if(!audio.isPlaying) {
			Destroy();
		}
	}

}

