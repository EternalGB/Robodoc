using UnityEngine;
using System.Collections;

public class ScoreAreaAnimController : MonoBehaviour
{

	public float maxSize;
	public float fill;
	Vector3 scale;

	void Start()
	{
		scale = new Vector3(maxSize,maxSize,maxSize);
	}

	void Update()
	{
		transform.localScale = scale*fill;

		//fill = Mathf.Clamp(fill - deflateRate*Time.deltaTime,0,1);
	}
		
}

