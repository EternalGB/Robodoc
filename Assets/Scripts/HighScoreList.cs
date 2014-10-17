using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class HighScoreList
{

	[SerializeField]
	public string name;
	[SerializeField]
	public List<float> highScores;

}

