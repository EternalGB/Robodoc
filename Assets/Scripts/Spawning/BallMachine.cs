using UnityEngine;
using System.Collections.Generic;

public class BallMachine : MonoBehaviour
{

	public List<GameObject> colourBalls;
	public List<GameObject> bonusBalls;
	public List<GameObject> badBalls;
	public float ballsPerSec;
	public float minInitSpeed,maxInitSpeed;
	public float badBallChance;
	public float pointsPerIncr;
	public float ballRateIncr;
	public float badChanceIncr;

	public Collider2D playArea;

	List<GameObject> goodBalls;

	void Awake()
	{
		if(playArea == null)
			playArea = GameObject.Find("PlayArea").GetComponent<Collider2D>();
		goodBalls = new List<GameObject>();
	}

	void Start()
	{
		goodBalls.AddRange(colourBalls);
		goodBalls.AddRange(bonusBalls);

		ScoreCalculator.PlayerScored += UpdateDifficulty;
	}

	public void StartSpawning()
	{
		Invoke("Spawn",1/ballsPerSec);
	}

	public void StopSpawning()
	{
		CancelInvoke("Spawn");
	}

	public void AddGoodBall(GameObject newBall)
	{
		goodBalls.Add(newBall);
	}

	public void AddBadBall(GameObject newBall)
	{
		badBalls.Add(newBall);
	}

	public int NumGoodBalls()
	{
		return goodBalls.Count;
	}

	bool HaveGoodBalls()
	{
		return goodBalls != null && goodBalls.Count > 0;
	}

	bool HaveBadBalls()
	{
		return badBalls != null && badBalls.Count > 0;
	}

	public void Spawn()
	{
		GameObject ball = null;
		if(HaveGoodBalls() && HaveBadBalls()) {
			if(Random.value < badBallChance) {
				ball = PoolManager.Instance.GetPoolByRepresentative(Util.GetRandomElement(badBalls)).GetPooled();
			} else {
				ball = PoolManager.Instance.GetPoolByRepresentative(Util.GetRandomElement(goodBalls)).GetPooled();
			}
		} else if(HaveGoodBalls()) {
			ball = PoolManager.Instance.GetPoolByRepresentative(Util.GetRandomElement(goodBalls)).GetPooled();
		} else if(HaveBadBalls()) {
			ball = PoolManager.Instance.GetPoolByRepresentative(Util.GetRandomElement(badBalls)).GetPooled();
		}

		if(ball != null)
			SpawnBall(ball);
		Invoke("Spawn",1/ballsPerSec);
	}

	public GameObject SpawnBall(GameObject ball)
	{
		Vector3 pos = PointOutside(Vector2.zero,playArea.transform.localScale.x/2,playArea.transform.localScale.x*1.5f);
		ball.transform.position = pos;
		ball.SetActive(true);
		//always send the ball towards the screen-ish
		ball.rigidbody2D.velocity = GetInitVelocity(pos,minInitSpeed,maxInitSpeed);	
		ball.rigidbody2D.angularVelocity = Mathf.Sign (ball.rigidbody2D.velocity.x)*ball.rigidbody2D.velocity.magnitude;
		//ball.rigidbody2D.velocity = Util.RandomVectorBetween(pos,screenTopLeft,screenBottomRight).normalized*Random.Range (minInitSpeed,maxInitSpeed);
		return ball;
	}



	void UpdateDifficulty(float scoreIncrease)
	{
		ballsPerSec = Mathf.Clamp (ballsPerSec + ballRateIncr*scoreIncrease/pointsPerIncr,0,20);
		badBallChance = Mathf.Clamp (badBallChance + badChanceIncr*scoreIncrease/pointsPerIncr,0,0.5f);
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

}

