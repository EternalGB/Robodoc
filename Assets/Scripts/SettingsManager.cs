using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsManager : MonoBehaviour
{

	void Start()
	{
		Settings.Load();
	}

	public void ChangeSetting(Settings.SettingName setting, object value)
	{
		Settings.ChangeSetting(setting,value);
	}

	public void ChangeSetting(Settings.SettingName setting, Slider slider)
	{
		ChangeSetting(setting,slider.value);
	}

	public void ChangeMasterVolume(Slider slider)
	{
		ChangeSetting(Settings.SettingName.MASTERVOL,slider.value);
	}


}

