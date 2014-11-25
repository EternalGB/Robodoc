using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimedMessage : TimedTutorialEvent
{

	public Text textArea;
	public TextAsset message;

	protected override void InitEvent ()
	{

	}

	protected override void StartEvent ()
	{
		textArea.text = message.text;
	}

	public override void Deactivate ()
	{
		textArea.text = "";
	}

}

