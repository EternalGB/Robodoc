using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioPauseController : MonoBehaviour
{

	GameGUI manager;
	AudioSource audioSource;

	void Start ()
	{
		manager = GameObject.FindGameObjectWithTag("GameGUI").GetComponent<GameGUI>();
		manager.PauseToggled += PauseHandler;

		audioSource = GetComponent<AudioSource>();

	}

	void PauseHandler(bool paused)
	{
		if(paused) {
			audioSource.Pause();
		} else {
			audioSource.UnPause();
		}
	}

	void OnDisable()
	{
		manager.PauseToggled -= PauseHandler;
	}

}

