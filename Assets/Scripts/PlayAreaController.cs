using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class PlayAreaController : MonoBehaviour
{

	CircleCollider2D circleCollider;
	public RectTransform radiusPoint;

	void Start()
	{
		circleCollider = GetComponent<CircleCollider2D>();
	}

	void Update()
	{
		//keeps the play area the same size as the viewable area on the screen
		circleCollider.radius = Util.ScreenPointToWorld(Camera.main.WorldToScreenPoint(radiusPoint.position),0).x;
	}
		
}

