using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreMessage : GoalTutorialEvent
{

	public Text textArea;
	public TextAsset message;

	protected override void InitEvent ()
	{
		ScoreCalculator.PlayerScored += HandleScoreEvent;
	}

	public override void Activate ()
	{
		textArea.text = message.text;
	}

	public override void Deactivate ()
	{
		textArea.text = "";
		ScoreCalculator.PlayerScored -= HandleScoreEvent;
	}

	void HandleScoreEvent(float score)
	{
		Debug.Log (name + " receiving score event");
		if(score > 0)
			SetCompleted();
	}


}

