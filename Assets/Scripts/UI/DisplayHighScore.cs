using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class DisplayHighScore : MonoBehaviour
{

	public Level level;
	Text text;

	void Start()
	{
		text = GetComponent<Text>();
		text.text = level.progress.score.ToString();
	}
		
}

