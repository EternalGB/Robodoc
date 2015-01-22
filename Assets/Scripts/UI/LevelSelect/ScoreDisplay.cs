using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour
{

	public Text text;

	public void SetScore(string score)
	{
		text.text = score;
	}
		
}

