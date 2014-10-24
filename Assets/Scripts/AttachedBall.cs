using UnityEngine;
using System.Collections;

public class AttachedBall : PoolableObject 
{

	public GameObject ejectedBallPrefab;
	public float pointValue = 1;
	Vector3 origScale;
	Material origMat;


	void Start()
	{
		origScale = transform.localScale;
		origMat = renderer.sharedMaterial;
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
		renderer.sharedMaterial = origMat;
	}

	public void SetPointValue(float value)
	{
		pointValue = value;
	}

	public void GetInfected(Material infectionMat, float spreadInterval)
	{
		renderer.sharedMaterial = infectionMat;
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
			Util.SetMaterialAllChildren(ejected.transform,ejected.renderer.sharedMaterial);
			ejected.SetActive(true);
			ejected.rigidbody.velocity = (pos - transform.parent.position).normalized*20;
			Destroy();
		}
		ScoreCalculator.Instance.SetScorePrediction();
	}


}
