using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class TagBalls : TutorialEvent
{
	
	public GameObject messageBoxPrefab;
	public GameObject reticulePrefab;
	public BallMachine ballMachine;
	public string message;
	public int tagInterval;
	protected int numTagged = 0;
	
	List<GameObject> balls;
	Dictionary<GameObject,GameObject> messageBoxes;
	Dictionary<GameObject,GameObject> reticules;
	Dictionary<GameObject,Transform> targets;
	Vector3 offset;
	
	int spawnCount = 0;
	
	PlayerBall playerBall;
	bool completed = false;
	

	protected abstract bool BallValid(GameObject ball);

	protected abstract bool CheckCompleted();

	protected override void InitEvent ()
	{
		balls = new List<GameObject>();
		messageBoxes = new Dictionary<GameObject,GameObject>();
		reticules = new Dictionary<GameObject,GameObject>();
		targets = new Dictionary<GameObject, Transform>();
		offset = new Vector3(0,
		                     messageBoxPrefab.GetComponent<RectTransform>().rect.height,
		                     0);
		playerBall = GameObject.Find ("PlayerBall").GetComponent<PlayerBall>();
	}



	void BallSpawnHandler(GameObject ball)
	{
		if(BallValid(ball) && spawnCount%tagInterval == 0) {
			
			GameObject messageBox = (GameObject)Instantiate(messageBoxPrefab,ball.transform.position - offset,Quaternion.identity);
			Text textArea = messageBox.GetComponentInChildren<Text>();
			textArea.text = message;
			GameObject reticule = (GameObject)Instantiate(reticulePrefab,ball.transform.position,Quaternion.identity);
			//ballMachine.BallSpawned -= BallSpawnHandler;
			messageBoxes.Add (ball, messageBox);
			targets.Add(ball, ball.transform);
			reticules.Add(ball,reticule);
			balls.Add(ball);
			numTagged++;
		}
		spawnCount++;
	}
	
	void Update()
	{
		if(targets.Count > 0 && messageBoxes.Count > 0) {
			foreach(GameObject ball in balls) {
				if(!ball.activeInHierarchy) {
					DestroyImmediate(messageBoxes[ball]);
					DestroyImmediate(reticules[ball]);
					messageBoxes.Remove(ball);
					targets.Remove(ball);
					reticules.Remove(ball);
					//balls.Remove(ball);
				} else {
					messageBoxes[ball].transform.position = targets[ball].position - offset;
					reticules[ball].transform.position = targets[ball].position;
				}
			}
			balls.RemoveAll( (ball) => !ball.activeInHierarchy);
		}
		if(CheckCompleted())
			completed = true;
	}
	
	public override void Activate ()
	{
		ballMachine.BallSpawned += BallSpawnHandler;
	}
	
	
	
	public override void Deactivate ()
	{
		ballMachine.BallSpawned -= BallSpawnHandler;
	}
	
	public override bool Completed ()
	{
		return completed;
	}
}

