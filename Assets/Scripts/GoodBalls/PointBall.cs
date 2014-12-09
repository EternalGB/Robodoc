using UnityEngine;
using System.Collections;

public class PointBall : PoolableScreenObj
{

	public float pointValue = 1;
	public AudioClip[] onCollectSounds;

	public bool HasSound
	{
		get {return onCollectSounds != null && onCollectSounds.Length > 0;}
	}

	public AudioClip CollectSound
	{
		get{ return Util.GetRandomElement(onCollectSounds);}
	}

}

