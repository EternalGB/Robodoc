using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsManager : MonoBehaviour
{

	public Slider masterVolume;
	public Slider musicVolume;
	public Slider sfxVolume;

	void Start()
	{
		Settings.Load();
		if(masterVolume != null)
			GetMasterVolume();
		if(musicVolume != null)
			GetMusicVolume();
		if(sfxVolume != null)
			GetSFXVolume();
	}

	public void ChangeSetting(Settings.SettingName setting, object value)
	{
		Settings.ChangeSetting(setting,value);
	}

	public void ChangeSetting(Settings.SettingName setting, Slider slider)
	{
		ChangeSetting(setting,slider.value);
	}

	public bool TryGetSetting(Settings.SettingName setting, out object value)
	{
		return Settings.TryGetSetting(setting,out value);
	}

	public bool TryGetSetting(Settings.SettingName setting, Slider slider)
	{
		object value;
		bool result = TryGetSetting(setting,out value);
		if(result) {
			slider.value = (float)value;
		}
		return result;
	}

	public void ChangeMasterVolume()
	{
		ChangeSetting(Settings.SettingName.MASTERVOL,masterVolume.value);
	}

	public void GetMasterVolume()
	{
		TryGetSetting(Settings.SettingName.MASTERVOL,masterVolume);
	}

	public void ChangeMusicVolume()
	{
		ChangeSetting(Settings.SettingName.MUSICVOL,musicVolume.value);
	}
	
	public void GetMusicVolume()
	{
		TryGetSetting(Settings.SettingName.MUSICVOL,musicVolume);
	}

	public void ChangeSFXVolume()
	{
		ChangeSetting(Settings.SettingName.SFXVOL,sfxVolume.value);
	}
	
	public void GetSFXVolume()
	{
		TryGetSetting(Settings.SettingName.SFXVOL,sfxVolume);
	}

	public void SaveSettings()
	{
		Settings.Save();
	}

	void OnDisable()
	{
		//Debug.Log ("Saving settings");
		SaveSettings();
	}

}

