using UnityEngine;
using System.Collections;

public class BGController : MonoBehaviour
{

	float scrollPercentage;
	Vector2 startPos;
	public Vector2 maxScroll;
	Vector2 scroll;

	void Awake()
	{
		startPos = transform.position;
	}

	void Update()
	{
		transform.position = startPos + scrollPercentage*maxScroll;
	}

	public void UpdatePos(float percentage)
	{
		scrollPercentage = Mathf.Clamp(percentage,0,1);
	}

}

