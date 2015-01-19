using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour
{

	public Text text;

	public void SetScore(float score)
	{
		text.text = score.ToString();
	}
		
}

