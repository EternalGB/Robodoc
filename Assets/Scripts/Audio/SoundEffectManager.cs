using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SoundEffectManager : MonoBehaviour
{

	public GameObject playerPrefab;
	ObjectPool playerPool;



	public static SoundEffectManager Instance
	{
		get; private set;
	}
	
	void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		
		Instance = this;
	}

	void Start()
	{
		playerPool = PoolManager.Instance.GetPoolByRepresentative(playerPrefab);
	}

	private AudioSource GetAvailablePlayer()
	{
		GameObject player = playerPool.GetPooled();
		player.SetActive(true);
		AudioSource asrc = player.GetComponent<AudioSource>();
		return asrc;
	}

	public void PlayClipOnce(AudioClip clip, AudioMixerGroup mixerGroup, Vector3 position, float volume, float pitch)
	{
		AudioSource asrc = GetAvailablePlayer();
		asrc.clip = clip;
		asrc.volume = volume;
		asrc.pitch = pitch;
		asrc.outputAudioMixerGroup = mixerGroup;
		asrc.Play();
	}
	


}

