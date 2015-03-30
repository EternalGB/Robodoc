using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public abstract class TouchPadController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	
	public Vector2 direction;
	protected Vector2 origin;
	public float radius;
	public float deadzone;
	public RectTransform originPoint;
	RectTransform radiusPt;
	int lastPointerId = -1;
	bool touched = false;
	RectTransform trans;
	
	void Awake()
	{
		direction = Vector2.zero;
		touched = false;
		trans = GetComponent<RectTransform>();
		radiusPt = transform.FindChild("RadiusPt").GetComponent<RectTransform>();
	}

	void Update()
	{
		radius = Util.ScreenPointToWorld(Camera.main.WorldToScreenPoint(radiusPt.position - trans.position),0).x;
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if(!touched) {
			lastPointerId = eventData.pointerId;
			origin = eventData.position;
			touched = true;
			if(originPoint != null) {
				originPoint.gameObject.SetActive(true);
				Vector3 pt;
				RectTransformUtility.ScreenPointToWorldPointInRectangle(trans,origin,Camera.main, out pt);
				originPoint.position = pt;
			}
		}
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		if(lastPointerId == eventData.pointerId) {
			Vector2 currPos = eventData.position;
			Vector2 dir = currPos - origin;
			direction = dir;
			if(direction.magnitude <= deadzone*radius)
				direction = Vector2.zero;
			else
				direction = Vector2.ClampMagnitude(direction/radius,1);

		}
	}	
	
	public abstract Vector2 GetDirection();
	
	public void OnPointerUp (PointerEventData eventData)
	{
		if(lastPointerId == eventData.pointerId) {
			direction = Vector2.zero;
			lastPointerId = -1;
			touched = false;
			if(originPoint != null) {
				originPoint.gameObject.SetActive(false);
			}
		}
	}
	
	
	
}

