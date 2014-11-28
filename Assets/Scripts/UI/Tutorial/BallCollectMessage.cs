using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BallCollectMessage : GoalTutorialEvent
{

	public Text textArea;
	public TextAsset message;

	PlayerBall playerBall;
	
	protected override void InitEvent ()
	{
		playerBall = GameObject.Find ("PlayerBall").GetComponent<PlayerBall>();

	}
	
	void BallCollectionHandler(GameObject player, GameObject playerPart, GameObject ball)
	{
		SetCompleted();
	}

	public override void Activate ()
	{
		playerBall.BallCollect += BallCollectionHandler;
		textArea.text = message.text;
	}

	public override void Deactivate ()
	{
		textArea.text = "";
		playerBall.BallCollect -= BallCollectionHandler;
	}
	
}

