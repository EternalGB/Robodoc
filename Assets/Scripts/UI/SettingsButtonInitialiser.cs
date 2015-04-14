using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsButtonInitialiser : MonoBehaviour
{
	
	void Awake ()
	{
		GameGUI manager = GameObject.FindGameObjectWithTag("GameGUI").GetComponent<GameGUI>();
		Button button = GetComponent<Button>();
		button.onClick.AddListener(
			() => manager.ShowSettings()
		);
	}
	

}

