using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoalRankDisplay : MonoBehaviour
{

	public RankDisplay rankDisplay;
	public Text nextTarget;

	Level level;
	Goal goal;
	int rank;

	public void Init(Level level, Goal goal)
	{
		this.level = level;
		this.goal = goal;
	}

	void Update()
	{
		if(level != null && goal != null) {
			rank = level.FindRank(goal.EvaluateSuccess());
			rankDisplay.SetRank(rank);
			if(rank != level.ranks.Length)
				nextTarget.text = goal.FormatSuccess(level.ranks[rank]);
			else
				nextTarget.text = "";
		}

	}

}

