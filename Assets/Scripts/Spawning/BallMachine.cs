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
	//top left, top right, bottom right, bottom left
	//List<Vector3> screenCorners;

	public Collider2D playArea;

	List<GameObject> goodBalls;

	void Start()
	{
		/*
		screenCorners = new List<Vector3>();
		screenCorners.Add(Util.ScreenPointToWorld(new Vector3(0,Camera.main.pixelHeight), 0));
		screenCorners.Add(Util.ScreenPointToWorld(new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight), 0));
		screenCorners.Add(Util.ScreenPointToWorld(new Vector3(Camera.main.pixelWidth,0), 0));
		screenCorners.Add(Util.ScreenPointToWorld(new Vector3(0,0), 0));
		*/
		if(playArea == null)
			playArea = GameObject.Find("PlayArea").GetComponent<Collider2D>();
		goodBalls = new List<GameObject>();
		goodBalls.AddRange(colourBalls);
		goodBalls.AddRange(bonusBalls);
		InvokeRepeating("SpawnBall",1/ballsPerSec,1/ballsPerSec);
	}
	

	void SpawnBall()
	{
		//refresh our spawn area if the screen size has changed
		/*
		screenCorners[0] = (Util.ScreenPointToWorld(new Vector3(0,Camera.main.pixelHeight), 0));
		screenCorners[1] = (Util.ScreenPointToWorld(new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight), 0));
		screenCorners[2] = (Util.ScreenPointToWorld(new Vector3(Camera.main.pixelWidth,0), 0));
		screenCorners[3] = (Util.ScreenPointToWorld(new Vector3(0,0), 0));

		Util.DebugDrawConnected(screenCorners,Color.white,2);
		*/
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
	}

	void UpdateDifficulty(float scoreIncrease)
	{
		ballsPerSec = Mathf.Clamp (ballsPerSec + 1f*scoreIncrease/1000,0,20);
		badBallChance = Mathf.Clamp (badBallChance + 0.05f*scoreIncrease/1000,0,0.5f);
	}

	Vector2 GetInitVelocity(Vector2 spawnPos, float minSpeed, float maxSpeed)
	{
		//pick whichever of two pairs of opposing corners has the widest angle between the vectors made with the spawn pos as the origin
		/*
		Vector3 v1 = screenCorners[0];
		Vector3 v2 = screenCorners[2];
		if(Util.AngleBetween(spawnPos,screenCorners[0],screenCorners[2]) < Util.AngleBetween(spawnPos,screenCorners[1],screenCorners[3])) {
			v1 = screenCorners[1];
			v2 = screenCorners[3];
		}
		float angle = Util.AngleBetween(spawnPos,v1,v2);
		Vector3 middleVec = (v1 - spawnPos).normalized + (v2 - spawnPos).normalized;
		middleVec.Normalize();
		Vector3 travelDir = Quaternion.AngleAxis(Random.Range (-angle/2,angle/2),Vector3.forward)*middleVec;
		Debug.DrawLine(spawnPos,v1,Color.green,5);
		Debug.DrawLine(spawnPos,v2,Color.green,5);
		Debug.DrawLine(spawnPos,spawnPos + travelDir*10,Color.red,5);
		return travelDir*Random.Range (minSpeed,maxSpeed);
		*/

		//pick a random point inside the screen
		/*
		Vector2 dest = Util.RandomPointInside(screenCorners[0],screenCorners[2]);
		return (dest - spawnPos).normalized*Random.Range(minSpeed,maxSpeed);
		*/

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

