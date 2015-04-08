using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioPauseController : MonoBehaviour
{

	GameGUI manager;
	AudioSource audio;

	void Start ()
	{
		manager = GameObject.FindGameObjectWithTag("GameGUI").GetComponent<GameGUI>();
		manager.PauseToggled += PauseHandler;

		audio = GetComponent<AudioSource>();

	}

	void PauseHandler(bool paused)
	{
		if(paused) {
			audio.Pause();
		} else {
			audio.UnPause();
		}
	}

	void OnDisable()
	{
		manager.PauseToggled -= PauseHandler;
	}

}

