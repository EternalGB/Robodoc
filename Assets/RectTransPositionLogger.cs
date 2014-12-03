using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RectTransPositionLogger : MonoBehaviour 
{

	public Vector3 position;
	RectTransform rect;

	void Start()
	{
		rect = GetComponent<RectTransform>();
	}

	void Update()
	{
		position = Util.ScreenPointToWorld(Camera.main.WorldToScreenPoint(rect.position),0);
	}

}
