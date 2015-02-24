using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoalRankDisplay : MonoBehaviour
{

	public RankDisplay rankDisplay;
	public Text nextTarget;

	Level level;
	ChallengeGoal goal;
	int rank;

	public void Init(Level level, ChallengeGoal goal)
	{
		this.level = level;
		this.goal = goal;
	}

	void Update()
	{
		if(level != null && goal != null) {
			rank = level.FindRank(goal.EvaluateSuccess());
			if(rankDisplay)
				rankDisplay.SetRank(rank);
			if(nextTarget) {
				if(goal.GetType() == typeof(ScoreTarget))
					nextTarget.text = (goal as ScoreTarget).targetScore.ToString();
				else {
					if(rank != level.ranks.Length)
						nextTarget.text = goal.FormatSuccess(level.ranks[rank]);
					else
						nextTarget.text = "";
				}
			}
		}

	}

}

