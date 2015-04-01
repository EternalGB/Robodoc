using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public abstract class BadBall : PoolableScreenObj
{

	public AudioClip[] onHitSounds;
	public AudioMixerGroup mixerGroup;

	public bool HasSound
	{
		get {return onHitSounds != null && onHitSounds.Length > 0;}
	}

	public AudioClip HitSound
	{
		get{ return Util.GetRandomElement(onHitSounds);}
	}

	public void Hit(Transform target)
	{
		if(HasSound)
			SoundEffectManager.Instance.PlayClipOnce(HitSound, mixerGroup, Vector3.zero,1,1);
		ApplyEffect(target);
	}

	protected abstract void ApplyEffect(Transform target);

}

