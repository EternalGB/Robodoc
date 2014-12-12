using UnityEngine;
using System.Collections.Generic;

public class AttachedBall : PoolableObject 
{

	public enum BallTypeNames
	{
		Green,LightBlue,Pink,Red,Yellow,BonusBall
	}
	

	public GameObject ejectedBallPrefab;
	public float pointValue = 1;
	Vector3 origScale;
	Material origMat;

	public BallTypeNames type;
	public BallStatus status;

	List<Material> matQueue;

	void OnEnable()
	{
		//make sure it's not bringing any friends
		//Util.DestroyChildrenWithComponent<AttachedBall>(transform);
		status = BallStatus.NONE;
		origScale = transform.localScale;
		origMat = GetComponent<SpriteRenderer>().sharedMaterial;
		matQueue = new List<Material>();
		matQueue.Add(origMat);
	}

	public void AddStatus(BallStatus newStatus, Material newMat)
	{
		if(newMat != null) {
			renderer.sharedMaterial = newMat;
			matQueue.Add(newMat);
		}
		FlagsHelper.Set<BallStatus>(ref status, newStatus);
		CheckStatus();
	}
	
	public void RemoveStatus(BallStatus newStatus, Material toRemove)
	{
		FlagsHelper.Unset<BallStatus>(ref status, newStatus);
		if(toRemove != null && matQueue.Remove(toRemove)) {
			CheckStatus();
		}
	}

	public void ClearStatus()
	{
		status = BallStatus.NONE;
		matQueue = new List<Material>();
		matQueue.Add(origMat);
		CheckStatus();
	}

	void CheckStatus()
	{
		if(matQueue.Count > 0)
			renderer.sharedMaterial = matQueue[matQueue.Count-1];
	}

	public void SetType(string typeName)
	{
		type = (BallTypeNames)System.Enum.Parse(typeof(BallTypeNames),typeName);
	}

	public override void Destroy ()
	{
		ResetBall();
		base.Destroy ();
	}

	public void ResetBall()
	{
		transform.parent = null;
		ClearStatus();
	}

	public void SetPointValue(float value)
	{
		pointValue = value;
	}

	public void GetInfected(Material infectionMat, float spreadInterval)
	{
		GetComponent<SpriteRenderer>().sharedMaterial = infectionMat;
		AddStatus(BallStatus.INFECTED,infectionMat);
		SetPointValue(0);
		ScoreCalculator.Instance.SetScorePrediction();
		if(transform.parent != null)
			StartCoroutine(Timers.Countdown<Material,float>(spreadInterval,SpreadInfection,infectionMat,spreadInterval));
	}

	void SpreadInfection(Material infectionMat, float spreadInterval)
	{
		AttachedBall ball;

		if(ball = transform.parent.GetComponent<AttachedBall>()) {
			ball.GetInfected(infectionMat,spreadInterval);
		} else if(transform.parent.name == "PlayerBall") {
			GameObject ejected = PoolManager.Instance.GetPoolByRepresentative(ejectedBallPrefab).GetPooled();
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			ejected.transform.position = pos;
			ejected.transform.rotation = rot;
			ejected.GetComponent<SpriteRenderer>().sprite = transform.GetComponent<SpriteRenderer>().sprite;
			foreach(Transform c in transform)
				c.parent = ejected.transform;
			AddStatusAllAttachedBalls(ejected.transform,BallStatus.EJECTED,ejected.GetComponent<SpriteRenderer>().sharedMaterial);
			ejected.SetActive(true);
			ejected.rigidbody2D.velocity = (pos - transform.parent.position).normalized*50;
			Destroy();
		}
		ScoreCalculator.Instance.SetScorePrediction();
	}


	public static void AddStatusAllAttachedBalls(Transform t, BallStatus newStatus, Material mat)
	{
		AttachedBall ab;
		if(ab = t.GetComponent<AttachedBall>()) {
			ab.AddStatus(newStatus,mat);
		}
		foreach(Transform child in t)
			AddStatusAllAttachedBalls(child,newStatus,mat);
	}
	
	public static void AddStatusAllAttachedBallsTemp(Transform t, BallStatus newStatus, Material mat, float duration)
	{
		AttachedBall ab;
		if(ab = t.GetComponent<AttachedBall>()) {
			//we make copies so the delegate we create keeps its own state
			//Material oldMat = t.GetComponent<SpriteRenderer>().sharedMaterial;
			//Transform trans = t;
			ab.AddStatus(newStatus,mat);
			ab.StartCoroutine(Timers.Countdown(duration,() => {
				ab.RemoveStatus(newStatus,mat);
			}));
		}
		foreach(Transform child in t)
			AddStatusAllAttachedBallsTemp(child,newStatus,mat,duration);
		
	}

}
