using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class RankStar : MonoBehaviour
{


	public bool animationFinished = false;
	public AudioClip prefinishedSound, finishedSound;
	public AudioMixerGroup mixerGroup;

	public void SetFinished()
	{
		animationFinished = true;
	}

	public void PlayEndSound()
	{
		SoundEffectManager.Instance.PlayClipOnce(finishedSound,mixerGroup, Vector3.zero,1,1);
	}

	public void PlayPreSound()
	{
		SoundEffectManager.Instance.PlayClipOnce(prefinishedSound, mixerGroup, Vector3.zero,1,1);
	}

}

