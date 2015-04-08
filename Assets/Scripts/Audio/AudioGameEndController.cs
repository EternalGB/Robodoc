using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioGameEndController : MonoBehaviour
{
	
	GameGUI manager;
	AudioSource audioSource;

	float volume;
	[Range(0,1)]
	public float fadeRate = 0.33f;
	bool startFade = false;
	float lerpTimer = 0;
	
	void Start ()
	{
		manager = GameObject.FindGameObjectWithTag("GameGUI").GetComponent<GameGUI>();
		manager.GameEnd += HandleGameEnd;
		
		audioSource = GetComponent<AudioSource>();
	}
	
	void Update()
	{
		if(startFade) {
			lerpTimer = Mathf.Clamp(lerpTimer + fadeRate*Time.unscaledDeltaTime, 0, 1);
			audioSource.volume = Mathf.Lerp (audioSource.volume, 0, lerpTimer);
		}
	}

	void HandleGameEnd()
	{
		startFade = true;
	}
}

