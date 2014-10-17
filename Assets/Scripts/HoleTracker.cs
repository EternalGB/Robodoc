﻿using UnityEngine;
using System.Collections;

public class HoleTracker : MonoBehaviour 
{
	public Transform hole;
	float radius;

	void Update()
	{
		radius = hole.localScale.x/2;
		renderer.material.SetVector("_ObjPos",new Vector4(hole.position.x,hole.position.y,hole.position.z,0));
		renderer.material.SetFloat("_Radius",radius);
	}

}
