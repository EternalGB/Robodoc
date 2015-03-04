using UnityEngine;
using System.Collections;

public class PoolableParticleBurst : PoolableObject
{
	
	protected bool hasPlayed;

	protected void OnEnable()
	{
		hasPlayed = false;
	}

	public void Play()
	{
		this.particleSystem.Play();
		hasPlayed = true;
	}

	protected void Update()
	{
		if(hasPlayed && particleSystem.isStopped) {
			Destroy();
		}
	}
	

}

