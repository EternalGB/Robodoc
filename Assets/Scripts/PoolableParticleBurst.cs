using UnityEngine;
using System.Collections;

public class PoolableParticleBurst : PoolableObject
{
	
	bool hasPlayed;

	void OnEnable()
	{
		hasPlayed = false;
	}

	public void Play()
	{
		this.particleSystem.Play();
		hasPlayed = true;
	}

	void Update()
	{
		if(hasPlayed && particleSystem.isStopped) {
			Destroy();
		}
	}
	

}

