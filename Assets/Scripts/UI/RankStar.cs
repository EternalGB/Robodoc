using UnityEngine;
using System.Collections;

public class RankStar : MonoBehaviour
{


	public bool animationFinished = false;
	public AudioClip finishedSound;

	public void SetFinished()
	{
		animationFinished = true;
		SoundEffectManager.PlayClipOnce(finishedSound,Vector3.zero,1);
	}

}

