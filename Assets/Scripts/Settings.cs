using UnityEngine;
using System.Collections.Generic;
using System;

public class Settings
{

	public enum SettingName
	{
		MASTERVOL,MUSICVOL,SFXVOL,MOVCONTROLS
	}

	public enum ControlType
	{
		KEYBOARD = 0,
		MOUSE = 1
	}

	public static Dictionary<SettingName,Type> settingTypes = new Dictionary<SettingName, Type>
	{
		{SettingName.MASTERVOL,typeof(float)},
		{SettingName.MUSICVOL,typeof(float)},
		{SettingName.SFXVOL,typeof(float)},
		{SettingName.MOVCONTROLS,typeof(ControlType)}
	};

	public static Dictionary<SettingName,object> settingValues;

	public static bool TryGetSetting(SettingName setting, out object value)
	{
		if(settingValues == null)
			Load();
		return settingValues.TryGetValue(setting,out value);
	}

	public static void ChangeSetting(SettingName setting, object value)
	{
		Type type;
		if(settingTypes.TryGetValue(setting,out type)) {
			if(type.Equals(value.GetType())) {
				if(settingValues == null)
					Load();
				if(settingValues.ContainsKey(setting))
					settingValues[setting] = value;
				else
					settingValues.Add(setting,value);
			} else {
				throw new ArgumentException("Wrong type, type of " + Enum.GetName(typeof(SettingName),setting) + " is " + type.ToString());
			}
		} else {
			throw new ArgumentException("That setting does not have a registered type");
		}
	}

	public static void Load()
	{
		settingValues = new Dictionary<SettingName, object>();
		foreach(SettingName setting in Enum.GetValues(typeof(SettingName))) {
			object value;
			if(Util.TryLoadFromPlayerPrefs("Settings" + Enum.GetName(typeof(SettingName),setting),out value)) {
				settingValues.Add(setting,value);
			} 
		}
	}

	public static void Save()
	{
		foreach(SettingName setting in settingValues.Keys) {
			//Debug.Log ("Saving " + Enum.GetName(typeof(SettingName),setting));
			SaveSetting(setting);
		}

	}

	public static void SaveSetting(SettingName setting)
	{
		if(settingValues == null)
			Load();
		object value;
		if(settingValues.TryGetValue(setting,out value)) {
			Util.SaveToPlayerPrefs("Settings" + Enum.GetName(typeof(SettingName),setting),value);
			//Debug.Log (Enum.GetName(typeof(SettingName),setting) + " saved as " + value.ToString());
		}
	}

}

