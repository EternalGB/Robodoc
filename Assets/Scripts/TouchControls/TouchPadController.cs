using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public abstract class TouchPadController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	
	public Vector2 direction;
	protected Vector2 origin;
	int lastPointerId = -1;
	bool touched = false;
	
	void Awake()
	{
		direction = Vector2.zero;
		touched = false;
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if(!touched) {
			lastPointerId = eventData.pointerId;
			origin = eventData.position;
			touched = true;
		}
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		if(lastPointerId == eventData.pointerId) {
			Vector2 currPos = eventData.position;
			Vector2 dir = currPos - origin;
			direction = dir;
		}
	}	
	
	public abstract Vector2 GetDirection();
	
	public void OnPointerUp (PointerEventData eventData)
	{
		if(lastPointerId == eventData.pointerId) {
			direction = Vector2.zero;
			lastPointerId = -1;
			touched = false;
		}
	}
	
	
	
}

