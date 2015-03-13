using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeEndDisplay : MonoBehaviour
{

	public AnimatedRankDisplay rankDisplay;
	public Text scoreDisplay;

	public void EndDisplay(float score, int rank, ChallengeGoal goal)
	{
		rankDisplay.SetRank(rank);
		scoreDisplay.text = goal.FormatSuccess(score);
	}
}

