using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeGetter : MonoBehaviour
{

	GameGUI uiController;
	Text text;

	void Start()
	{
		uiController = GameObject.FindGameObjectWithTag("GameGUI").GetComponent<GameGUI>();
		text = GetComponent<Text>();
	}

	void Update()
	{
		text.text = MinutesSeconds(uiController.displayTime);
	}


	static string MinutesSeconds(float seconds)
	{
		System.TimeSpan ts = new System.TimeSpan(0,0,(int)seconds);
		string secs = ts.Seconds.ToString();
		if(ts.Seconds < 10)
			secs = "0" + secs;
		return ts.Minutes.ToString() + ":" + secs;
	}


}

