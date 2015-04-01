using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderSettingGetter : MonoBehaviour
{

	public Settings.SettingName setting;
	Slider slider;

	void OnEnable()
	{
		slider = GetComponent<Slider>();
		slider.value = Settings.GetFloat(setting,0);
	}

}

