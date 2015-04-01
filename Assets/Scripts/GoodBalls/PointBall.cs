using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class PointBall : PoolableScreenObj
{

	public float pointValue = 1;
	public AudioClip[] onCollectSounds;
	public AudioMixerGroup mixerGroup;

	public bool HasSound
	{
		get {return onCollectSounds != null && onCollectSounds.Length > 0;}
	}

	public AudioClip CollectSound
	{
		get{ return Util.GetRandomElement(onCollectSounds);}
	}

	public void Hit(Transform target)
	{
		if(HasSound)
			SoundEffectManager.Instance.PlayClipOnce(CollectSound, mixerGroup, Vector3.zero,1,1);
	}
	

}

