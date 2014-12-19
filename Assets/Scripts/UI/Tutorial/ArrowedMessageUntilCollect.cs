using UnityEngine;
using System.Collections;

public class ArrowedMessageUntilCollect : ArrowedMessage
{
	

	bool completed = false;

	protected override void InitEvent ()
	{
		ScoreCalculator.PlayerScored += HandlePlayerScored;
	}

	void HandlePlayerScored (float scoreIncrease)
	{
		completed = true;
		ScoreCalculator.PlayerScored -= HandlePlayerScored;
	}

	public override bool Completed ()
	{
		return completed;
	}


}

