using UnityEngine;
using System.Collections;

public class RankStar : MonoBehaviour
{


	public bool animationFinished = false;
	public AudioClip prefinishedSound, finishedSound;

	public void SetFinished()
	{
		animationFinished = true;
	}

	public void PlayEndSound()
	{
		SoundEffectManager.Instance.PlayClipOnce(finishedSound,Vector3.zero,1,1);
	}

	public void PlayPreSound()
	{
		SoundEffectManager.Instance.PlayClipOnce(prefinishedSound,Vector3.zero,1,1);
	}

}

