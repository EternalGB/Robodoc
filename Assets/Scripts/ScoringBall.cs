using UnityEngine;
using System.Collections;

public class ScoringBall : PoolableObject
{

	public float lifeTime;
	public float speed;
	public Vector3 travelDir;
	public float rotSpeed;
	//public Vector3 rotCenter;
	

	void Update()
	{
		transform.position += travelDir*speed*Time.deltaTime;
		transform.Rotate(Vector3.forward,rotSpeed*Time.deltaTime);
		//transform.RotateAround(rotCenter,Vector3.forward,rotSpeed*Time.deltaTime);
	}
			
	void OnDisable()
	{
		//CancelInvoke("Destroy");
		Util.DestroyChildren(transform);
	}

	public void Arm()
	{
		StartCoroutine(Timers.Countdown(lifeTime,Destroy));
	}
}

