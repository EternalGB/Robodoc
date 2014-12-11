using UnityEngine;
using System.Collections;

public class AttachedBall : PoolableObject 
{

	public enum BallTypeNames
	{
		Green,LightBlue,Pink,Red,Yellow,BonusBall,Infected
	}



	public GameObject ejectedBallPrefab;
	public float pointValue = 1;
	Vector3 origScale;
	Material origMat;

	public BallTypeNames type;
	public BallStatus status;

	void Start()
	{
		origScale = transform.localScale;
		origMat = GetComponent<SpriteRenderer>().sharedMaterial;
	}

	public void AddStatus(BallStatus newStatus, Material newMat)
	{
		if(newMat != null)
			renderer.sharedMaterial = newMat;
		FlagsHelper.Set<BallStatus>(ref status, newStatus);
		CheckStatus();
	}
	
	public void RemoveStatus(BallStatus newStatus)
	{
		FlagsHelper.Unset<BallStatus>(ref status, newStatus);
		CheckStatus();
	}
	
	void CheckStatus()
	{
		if(status == BallStatus.NONE)
			renderer.sharedMaterial = origMat;
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
		//transform.localScale = origScale;
		GetComponent<SpriteRenderer>().sharedMaterial = origMat;
	}

	public void SetPointValue(float value)
	{
		pointValue = value;
	}

	public void GetInfected(Material infectionMat, float spreadInterval)
	{
		GetComponent<SpriteRenderer>().sharedMaterial = infectionMat;
		type = BallTypeNames.Infected;
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
			foreach(Transform c in transform)
				c.parent = ejected.transform;
			Util.SetMaterialAllAttachedBalls(ejected.transform,ejected.GetComponent<SpriteRenderer>().sharedMaterial);
			ejected.SetActive(true);
			ejected.rigidbody2D.velocity = (pos - transform.parent.position).normalized*20;
			Destroy();
		}
		ScoreCalculator.Instance.SetScorePrediction();
	}


}
