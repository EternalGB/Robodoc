using UnityEngine;
using System.Collections;

public class PoolableScreenObj : PoolableObject
{

	float offset = 1f;
	Bounds allowableArea;

	void OnEnable()
	{
		allowableArea = new Bounds(new Vector3(0.5f,0.5f,0),new Vector3(1+offset,1+offset,1));
	}

	void Update()
	{

		Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
		viewPos.z = 0;
		if(!allowableArea.Contains(viewPos))
			Destroy();
	}

}

