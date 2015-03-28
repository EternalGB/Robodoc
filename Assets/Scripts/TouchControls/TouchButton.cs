using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{


	int lastPointerId = -1;
	bool touched = false;
	bool pressed = false;

	public void OnPointerDown(PointerEventData eventData)
	{
		if(!touched) {
			lastPointerId = eventData.pointerId;
			touched = true;
			pressed = true;
		}
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if(lastPointerId == eventData.pointerId) {
			lastPointerId = -1;
			touched = false;
		}
	}

	void FixedUpdate()
	{
		if(touched && pressed)
			pressed = false;
	}

	public bool Pressed()
	{
		return pressed;
	}

	public bool IsDown ()
	{
		return touched;
	}
		
	
}

