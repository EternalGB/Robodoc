using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class SettingsManager : MonoBehaviour
{

	public AudioMixer masterMixer;
	float master;
	float music;
	float fx;

	void Start()
	{

		Settings.Load();
		master = Settings.GetFloat(Settings.SettingName.MASTERVOL, 0);
		music = Settings.GetFloat(Settings.SettingName.MUSICVOL, 0);
		fx = Settings.GetFloat(Settings.SettingName.SFXVOL, 0);
		SetMasterVolume(master);
		SetMusicVolume(music);
		SetFXVolume(fx);
	}

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

	public void ChangeSetting(Settings.SettingName setting, object value)
	{
		Settings.ChangeSetting(setting,value);
	}
	
	public void SaveSettings()
	{
		masterMixer.GetFloat("masterVol", out master);
		masterMixer.GetFloat("musicVol", out music);
		masterMixer.GetFloat("fxVol", out fx);
		ChangeSetting(Settings.SettingName.MASTERVOL, master);
		ChangeSetting(Settings.SettingName.MUSICVOL, music);
		ChangeSetting(Settings.SettingName.SFXVOL, fx);
		Settings.Save();

	}

	void OnDisable()
	{
		//Debug.Log ("Saving settings");
		SaveSettings();
	}

}

