using UnityEngine;
using System.Collections.Generic;

public class SoundEffectManager : MonoBehaviour
{

	public GameObject playerPrefab;
	ObjectPool playerPool;

	public List<AudioClip> loadedClips;
	private Dictionary<string,AudioClip> soundLibrary;

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
		soundLibrary = new Dictionary<string, AudioClip>();
		foreach(AudioClip ac in loadedClips)
			soundLibrary.Add(ac.name,ac);
	}

	private AudioSource GetAvailablePlayer()
	{
		GameObject player = playerPool.GetPooled();
		player.SetActive(true);
		AudioSource asrc = player.audio;
		return asrc;
	}

	public static void PlayClipOnce(AudioClip clip, Vector3 position, float volume)
	{
		AudioSource.PlayClipAtPoint(clip,position,volume);
	}

	public void PlayClipOnce(AudioClip clip, Vector3 position, float volume, float pitch)
	{
		AudioSource asrc = GetAvailablePlayer();
		asrc.clip = clip;
		asrc.volume = volume;
		asrc.pitch = pitch;
		asrc.Play();
	}

	public AudioSource PlayClipLoop(AudioClip clip, Vector3 position)
	{
		AudioSource asrc = GetAvailablePlayer();
		asrc.clip = clip;
		asrc.loop = true;
		asrc.Play();
		return asrc;
	}

	public void PlayClipOnce(string clipName, Vector3 position, float volume, float pitch)
	{
		AudioClip clip;
		if(soundLibrary.TryGetValue(clipName,out clip)) {
			PlayClipOnce(clip,position,volume,pitch);
		} else  {
			throw new KeyNotFoundException("No clip called " + clipName + " in library");
		}

	}

}

