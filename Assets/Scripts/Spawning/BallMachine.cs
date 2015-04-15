using UnityEngine;
using System.Collections.Generic;


public class BallMachine : MonoBehaviour
{

	public bool spawnOnStart = false;

	public List<GameObject> colourBalls;
	public List<GameObject> bonusBalls;
	public List<GameObject> badBalls;
	public float minInitSpeed,maxInitSpeed;
	public Vector2 goodRateMinMax, bonusRateMinMax, badRateMinMax, scoreMinMax;
	float goodPerSec,bonusPerSec,badPerSec;
	float goodSpawnScaler = 1, bonusSpawnScaler = 1, badSpawnScaler = 1;
	float speedScaler = 1;

	bool spawning = false;

	public CircleCollider2D playArea;


	public delegate void BallSpawnedHandler(GameObject ball);
	public event BallSpawnedHandler BallSpawned;

	void Awake()
	{
		if(playArea == null)
			playArea = GameObject.Find("PlayArea").GetComponent<CircleCollider2D>();
	}

	void Start()
	{
		UpdateDifficulty(0);
		if(spawnOnStart)
			StartSpawning();
		ScoreCalculator.PlayerScored += UpdateDifficulty;
	}

	public void StartSpawning()
	{
		Invoke("StartGoodSpawning",Random.Range(0,1/goodPerSec));
		Invoke("StartBadSpawning", Random.Range(0,1/badPerSec));
		Invoke("StartBonusSpawning", Random.Range(0,1/bonusPerSec));

		spawning = true;
	}

	void StartGoodSpawning()
	{
		StartCoroutine(Timers.Countdown<List<GameObject>,float>(1/goodPerSec,Spawn,colourBalls,1/goodPerSec));
	}

	void StartBadSpawning()
	{
		StartCoroutine(Timers.Countdown<List<GameObject>,float>(1/badPerSec,Spawn,badBalls,1/badPerSec));
	}

	void StartBonusSpawning()
	{
		StartCoroutine(Timers.Countdown<List<GameObject>,float>(1/bonusPerSec,Spawn,bonusBalls,1/bonusPerSec));
	}

	public void StopSpawning()
	{
		spawning = false;
	}

	public void AddColourBall(GameObject newBall)
	{
		colourBalls.Add(newBall);
	}

	public void AddBadBall(GameObject newBall)
	{
		badBalls.Add(newBall);
	}

	public int NumColourBalls()
	{
		return colourBalls.Count;
	}

	bool HaveColourBalls()
	{
		return colourBalls != null && colourBalls.Count > 0;
	}

	bool HaveBadBalls()
	{
		return badBalls != null && badBalls.Count > 0;
	}

	public void Spawn(List<GameObject> collection, float interval)
	{
		if(spawning) {
			GameObject ball = null;
			//Debug.Log ("Selecting ball");
			if(collection != null && collection.Count > 0)
				ball = Util.GetRandomElement<GameObject>(collection);
			
			if(ball != null) {
				//Debug.Log ("Spawning a " + ball.name);
				SpawnBall(ball);

			}
			//randomise the spawn a little bit
			float nextInterval = interval + Util.RandomSign()*Random.Range(0.1f,0.2f)*interval;
			StartCoroutine(Timers.Countdown<List<GameObject>,float>(nextInterval,Spawn,collection,interval));
		}
	}

	public GameObject SpawnBall(GameObject ballPrefab)
	{
		float radius = playArea.radius;
		Vector3 pos = PointOutside(Vector2.zero,1.05f*radius,1.2f*radius);
		//Debug.Log ("Spawning " + ballPrefab.name + " at " + pos);
		GameObject ball = PoolManager.Instance.GetPoolByRepresentative(ballPrefab).GetPooled();
		ball.transform.position = pos;
		ball.SetActive(true);
		Vector2 initVel;
		if(ball.layer == LayerMask.NameToLayer("BadBall") && GameObject.Find ("PlayerBall") != null) {
			initVel = (GameObject.Find("PlayerBall").transform.position - ball.transform.position).normalized*Random.Range(minInitSpeed,maxInitSpeed);
		} else {
			initVel = GetInitVelocity(pos,minInitSpeed,maxInitSpeed);
		}
		//always send the ball towards the screen-ish
		ball.GetComponent<Rigidbody2D>().velocity = speedScaler*initVel;
		//Debug.Log ("Set velocity to " + ball.GetComponent<Rigidbody2D> ().velocity);
		ball.GetComponent<Rigidbody2D>().angularVelocity = Mathf.Sign (ball.GetComponent<Rigidbody2D>().velocity.x)*ball.GetComponent<Rigidbody2D>().velocity.magnitude;
		//ball.rigidbody2D.velocity = Util.RandomVectorBetween(pos,screenTopLeft,screenBottomRight).normalized*Random.Range (minInitSpeed,maxInitSpeed);
		if(BallSpawned != null)
			BallSpawned(ball);
		return ball;
	}



	void UpdateDifficulty(float scoreIncrease)
	{

		goodPerSec = goodSpawnScaler*Mathf.Lerp(goodRateMinMax.x, goodRateMinMax.y, ScoreCalculator.Instance.score/(scoreMinMax.y - scoreMinMax.x));
		bonusPerSec = bonusSpawnScaler*Mathf.Lerp(bonusRateMinMax.x, bonusRateMinMax.y, ScoreCalculator.Instance.score/(scoreMinMax.y - scoreMinMax.x));
		badPerSec = badSpawnScaler*Mathf.Lerp(badRateMinMax.x, badRateMinMax.y, ScoreCalculator.Instance.score/(scoreMinMax.y - scoreMinMax.x));
		//Debug.Log ("Updating difficulty " + goodPerSec + " " + bonusPerSec + " " + badPerSec);
	}

	public void IncreaseGoodRateScaler(float scaler)
	{
		goodSpawnScaler += scaler;
		bonusSpawnScaler += scaler;
	}

	public void IncreaseBadRateScaler(float scaler)
	{
		badSpawnScaler += scaler;
	}

	public void IncreaseSpeedScaler(float scaler)
	{
		speedScaler += scaler;
	}


	Vector2 GetInitVelocity(Vector2 spawnPos, float minSpeed, float maxSpeed)
	{
		//pick a random point inside the play area
		Vector2 dest = Util.RandomPointInside(playArea);
		return (dest - spawnPos).normalized*Random.Range(minSpeed,maxSpeed);
	}

	Vector2 PointOutside(Vector2 topLeft, Vector2 bottomRight, float maxDist)
	{
		//Debug.Log ("Creating point just outside of " + topLeft + " " + bottomRight);
		Vector2 upLeft = Quaternion.AngleAxis(45,Vector3.forward)*Vector2.up;
		Vector2 downRight = -upLeft;
		Vector2 outerTopLeft = topLeft + upLeft*maxDist;
		Vector2 outerBottomRight = bottomRight + downRight*maxDist;
		//Debug.Log ("Outer limites are " + outerTopLeft + " " + outerBottomRight);
		Vector2 point = Vector2.zero;
		do {
			point = Util.RandomPointInside(outerTopLeft,outerBottomRight);
		} while(Util.PointInside(topLeft,bottomRight,point));
		return point;
	}

	Vector2 PointOutside(Vector2 center, float innerRadius, float outerRadius)
	{
		Vector2 point = center;
		while(Util.PointInside(center,innerRadius,point)) {
			point = Util.RandomPointInside(center,outerRadius);
		}
		return point;
	}

	void OnDisable()
	{
		ScoreCalculator.PlayerScored -= UpdateDifficulty;
	}

}

