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
		Invoke("SpawnBall",1/ballsPerSec);
		ScoreCalculator.PlayerScored += UpdateDifficulty;
	}
	
	public void AddGoodBall(GameObject newBall)
	{
		goodBalls.Add(newBall);
	}

	public void AddBadBall(GameObject newBall)
	{
		badBalls.Add(newBall);
	}

	void SpawnBall()
	{

		GameObject ball;
		if(Random.value < badBallChance) {
			ball = PoolManager.Instance.GetPoolByRepresentative(Util.GetRandomElement(badBalls)).GetPooled();
		} else {
			ball = PoolManager.Instance.GetPoolByRepresentative(Util.GetRandomElement(goodBalls)).GetPooled();
		}
		Vector3 pos = PointOutside(Vector2.zero,playArea.transform.localScale.x/2,playArea.transform.localScale.x*1.25f);
		ball.transform.position = pos;
		ball.SetActive(true);
		//always send the ball towards the screen-ish
		ball.rigidbody.velocity = GetInitVelocity(pos,minInitSpeed,maxInitSpeed);	
		//ball.rigidbody.velocity = Util.RandomVectorBetween(pos,screenTopLeft,screenBottomRight).normalized*Random.Range (minInitSpeed,maxInitSpeed);
		Invoke("SpawnBall",1/ballsPerSec);
	}



	void UpdateDifficulty(float scoreIncrease)
	{
		ballsPerSec = Mathf.Clamp (ballsPerSec + 1f*scoreIncrease/1000,0,20);
		badBallChance = Mathf.Clamp (badBallChance + 0.05f*scoreIncrease/1000,0,0.5f);
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

