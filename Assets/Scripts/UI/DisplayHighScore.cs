using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class DisplayHighScore : MonoBehaviour
{

	public Level level;
	public Goal goal;
	public int difficulty;
	Text text;

	void Start()
	{
		text = GetComponent<Text>();
		text.text = HighScores.GetScore(level,goal,difficulty).ToString();
	}
		
}

