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
	}

	void HandleScoreEvent(float score)
	{
		SetCompleted();
	}


}

