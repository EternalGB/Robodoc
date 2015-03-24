using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchRotationButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	Vector2 screenPos;
	public float input;
	RectTransform trans;
	int lastPointerId = -1;
	bool touched = false;
	public Image left, right;
	
	void Start()
	{
		trans = GetComponent<RectTransform>();
	}
	


	public void OnPointerDown(PointerEventData eventData)
	{
		PrepareInput(eventData);
		if(screenPos.x < 0) {
			input = -1;
		} else {
			input = 1;
		}
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		PrepareInput(eventData);
		input = 0;
	}

	void PrepareInput(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(trans,eventData.position,Camera.main, out screenPos);
	}

}

