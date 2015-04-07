using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class RandomClipSelector : MonoBehaviour
{

	public List<AudioClip> clips;

	void Awake()
	{
		GetComponent<AudioSource>().clip = GetRandomClip();
	}

	public AudioClip GetRandomClip()
	{
		return Util.GetRandomElement(clips);
	}

}

