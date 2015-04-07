using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class ChallengeLevelMusicSelector : MonoBehaviour
{
	
	public List<AudioClip> clips2Min, clips5Min;
	
	public void SelectMusic(Goal goal)
	{
		if(goal.initTime > 120)
			GetComponent<AudioSource>().clip = Util.GetRandomElement(clips5Min);
		else
			GetComponent<AudioSource>().clip = Util.GetRandomElement(clips2Min);
		
	}

	
}

