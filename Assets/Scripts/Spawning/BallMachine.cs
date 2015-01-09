using UnityEngine;
using System.Collections.Generic;

public class BallMachine : MonoBehaviour
{

	public bool spawnOnStart = false;

	public List<GameObject> colourBalls;
	public List<GameObject> bonusBalls;
	public List<GameObject> badBalls;
	public float minInitSpeed,maxInitSpeed;
	public AnimationCurve goodRateCurve,bonusRateCurve,badRateCurve;
	float goodPerSec,bonusPerSec,badPerSec;

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
		if(spawnOnStart)
			StartSpawning();
		UpdateDifficulty(0);
		ScoreCalculator.PlayerScored += UpdateDifficulty;
	}

	public void StartSpawning()
	{
		StartCoroutine(Timers.Countdown<List<GameObject>,float>(1/goodPerSec,Spawn,colourBalls,1/goodPerSec));
		StartCoroutine(Timers.Countdown<List<GameObject>,float>(1/badPerSec,Spawn,badBalls,1/badPerSec));
		StartCoroutine(Timers.Countdown<List<GameObject>,float>(1/bonusPerSec,Spawn,bonusBalls,1/bonusPerSec));
		spawning = true;
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
			if(collection != null && collection.Count > 0)
				ball = Util.GetRandomElement<GameObject>(collection);
			
			if(ball != null) 
				SpawnBall(ball);
			//randomise the spawn a little bit
			float nextInterval = interval + Util.RandomSign()*Random.Range(0.1f,0.2f)*interval;
			StartCoroutine(Timers.Countdown<List<GameObject>,float>(nextInterval,Spawn,collection,interval));
		}
	}

	public GameObject SpawnBall(GameObject ballPrefab)
	{

		float radius = playArea.radius;
		Vector3 pos = PointOutside(Vector2.zero,1.05f*radius,1.2f*radius);

		GameObject ball = PoolManager.Instance.GetPoolByRepresentative(ballPrefab).GetPooled();
		ball.transform.position = pos;
		ball.SetActive(true);
		//always send the ball towards the screen-ish
		ball.rigidbody2D.velocity = GetInitVelocity(pos,minInitSpeed,maxInitSpeed);	
		ball.rigidbody2D.angularVelocity = Mathf.Sign (ball.rigidbody2D.velocity.x)*ball.rigidbody2D.velocity.magnitude;
		//ball.rigidbody2D.velocity = Util.RandomVectorBetween(pos,screenTopLeft,screenBottomRight).normalized*Random.Range (minInitSpeed,maxInitSpeed);
		if(BallSpawned != null)
			BallSpawned(ball);
		return ball;
	}



	void UpdateDifficulty(float scoreIncrease)
	{
		goodPerSec = goodRateCurve.Evaluate(ScoreCalculator.Instance.score);
		bonusPerSec = bonusRateCurve.Evaluate(ScoreCalculator.Instance.score);
		badPerSec = badRateCurve.Evaluate(ScoreCalculator.Instance.score);
		//goodPerSec = Mathf.Clamp (goodPerSec + goodRateIncr*scoreIncrease/pointsPerIncr,0,goodRateMax);
		//bonusPerSec = Mathf.Clamp (bonusPerSec + bonusRateIncr*scoreIncrease/pointsPerIncr,0,bonusRateMax);
		//badPerSec = Mathf.Clamp (badPerSec + badRateIncr*scoreIncrease/pointsPerIncr,0,badRateMax);
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

