using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{

	public float volumeModifier = 1;

	//have to call this on awake also otherwise there's a tiny moment where the volume is different
	void Awake()
	{
		GetComponent<AudioSource>().volume = volumeModifier*GetVolume();
	}

	void Update()
	{
		GetComponent<AudioSource>().volume = volumeModifier*GetVolume();
	}

	static float GetVolume()
	{
		object master;
		object music;
		float fmaster = 1;
		float fmusic = 1;
		if(Settings.TryGetSetting(Settings.SettingName.MASTERVOL,out master))
			fmaster = (float)master;
		if(Settings.TryGetSetting(Settings.SettingName.MUSICVOL,out music))
			fmusic = (float)music;
		return fmaster*fmusic;
	}

}

