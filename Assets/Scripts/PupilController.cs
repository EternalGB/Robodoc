using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PupilController : MonoBehaviour 
{

	public float eyeRadius;
	public Transform eyeCenter;
	public Transform target;

	void OnEnable()
	{
		target = GameObject.Find("PlayerBall").transform;
	}

	void Update()
	{
		if(eyeCenter != null && target != null)
			transform.position = eyeCenter.position + (target.position - transform.position).normalized*eyeRadius;
	}

}
