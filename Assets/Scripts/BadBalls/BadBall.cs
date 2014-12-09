using UnityEngine;
using System.Collections;

public abstract class BadBall : PoolableScreenObj
{

	public AudioClip[] onHitSounds;

	public bool HasSound
	{
		get {return onHitSounds != null && onHitSounds.Length > 0;}
	}

	public AudioClip HitSound
	{
		get{ return Util.GetRandomElement(onHitSounds);}
	}

	public abstract void ApplyEffect(Transform target);

}

