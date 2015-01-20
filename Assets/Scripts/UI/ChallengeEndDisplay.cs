using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeEndDisplay : MonoBehaviour
{

	public RankDisplay rankDisplay;
	public Text scoreDisplay;

	public void EndDisplay(Level level)
	{
		rankDisplay.SetRank(level.progress.rank);
		scoreDisplay.text = level.progress.score.ToString ();
	}
}

