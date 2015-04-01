using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioMixManager : MonoBehaviour
{

	public AudioMixer masterMixer;

	public void SetMasterVolume(float value)
	{
		masterMixer.SetFloat("masterVol",value);
	}

	public void SetMusicVolume(float value)
	{
		masterMixer.SetFloat ("musicVol",value);
	}

	public void SetFXVolume(float value)
	{
		masterMixer.SetFloat("fxVol",value);
	}

}

