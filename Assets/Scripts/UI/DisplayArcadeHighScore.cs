using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class DisplayArcadeHighScore : MonoBehaviour
{

	Text text;
	
	void Start()
	{
		text = GetComponent<Text>();
		text.text = ArcadeStats.HighScore.ToString();
	}
	
}

