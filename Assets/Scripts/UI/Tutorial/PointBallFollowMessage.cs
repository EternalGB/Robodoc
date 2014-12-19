using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointBallFollowMessage : TutorialEvent
{

	public GameObject messageBoxPrefab;
	public BallMachine ballMachine;
	public string message;

	GameObject messageBox;
	Transform target;
	Vector3 offset;

	protected override void InitEvent ()
	{
		messageBox = null;
		target = null;
	}

	void BallSpawnHandler(GameObject ball)
	{
		if(ball.GetComponent<PointBall>()) {
			Debug.Log ("starting follow message " + message);
			offset = new Vector3(0,
			                     messageBoxPrefab.GetComponent<RectTransform>().rect.height,
			                     0);
			messageBox = (GameObject)Instantiate(messageBoxPrefab,ball.transform.position - offset,Quaternion.identity);
			target = ball.transform;
			Debug.Log ("Target active=" + ball.activeInHierarchy);
			Text textArea = messageBox.GetComponentInChildren<Text>();
			textArea.text = message;
			ballMachine.BallSpawned -= BallSpawnHandler;
		}
	}

	void Update()
	{
		if(target != null && messageBox != null)
			messageBox.transform.position = target.position - offset;
	}

	public override void Activate ()
	{
		ballMachine.BallSpawned += BallSpawnHandler;
	}

	public override void Deactivate ()
	{
		Debug.Log ("Deleting follow message " + message);
		DestroyImmediate(messageBox);

	}

	public override bool Completed ()
	{
		if(target != null)
			return !target.gameObject.activeInHierarchy;
		else
			return false;
	}
	

}

