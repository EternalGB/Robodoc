using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public abstract class TouchStickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

	protected Vector2 input;
	//public Vector2 screenPosition;
	RectTransform trans;
	public RectTransform stick;
	public float maxStickMovement;
	protected float radius;
	int lastPointerId = -1;
	bool touched = false;


	void Start()
	{
		trans = GetComponent<RectTransform>();
		radius = trans.sizeDelta.x/2;
	}
	
	void Update()
	{
		stick.anchoredPosition = maxStickMovement*radius*input;
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log (name + " pointer down");
		if(!touched) {
			lastPointerId = eventData.pointerId;
			PrepareInput(eventData);
			touched = true;
		}
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		if(lastPointerId == eventData.pointerId) {
			PrepareInput(eventData);
		}
	}

	void PrepareInput(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(trans,eventData.position,Camera.main, out input);
	}

	public abstract Vector2 GetInput();
	
	public void OnPointerUp (PointerEventData eventData)
	{
		if(lastPointerId == eventData.pointerId) {
			input = Vector2.zero;
			lastPointerId = -1;
			touched = false;
		}
	}
	


}

