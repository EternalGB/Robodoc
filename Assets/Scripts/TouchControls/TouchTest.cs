using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TouchTest : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

	public List<DisplayTouch> touches;
	public List<PointerEventData> datas;
	RectTransform trans;

	void Start()
	{
		trans = GetComponent<RectTransform>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		DisplayTouch touch = new DisplayTouch(eventData.pointerId, eventData.position);
		if(!touches.Contains(touch)) {
			touches.Add(touch);
			Debug.Log ("New touch: " + eventData.pointerId);


			Debug.Log ("Num touches now " + touches.Count);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		//Debug.Log ("Drag for " + eventData.pointerId);
		DisplayTouch touch = new DisplayTouch(eventData.pointerId, eventData.position);
		if(touches.Contains(touch))
			touches[touches.IndexOf(touch)].position = eventData.position;
		//Debug.DrawLine(Vector3.zero,GetLocalPos(eventData.position),Color.red,0.1f);
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		DisplayTouch touch = new DisplayTouch(eventData.pointerId, eventData.position);
		if(touches.Contains(touch))
			touches.Remove(touch);
	}

	Vector2 GetLocalPos(PointerEventData eventData)
	{
		Vector2 input;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(trans,eventData.position,Camera.main, out input);
		return input;
	}

	[System.Serializable]
	public class DisplayTouch
	{
		public int id;
		public Vector2 position;

		public DisplayTouch(int id, Vector2 position)
		{
			this.id = id;
			this.position = position;
		}

		public override bool Equals (object obj)
		{
			DisplayTouch other = obj as DisplayTouch;
			if(other != null)
				return id == other.id;
			else
				return false;
		}

		public override int GetHashCode ()
		{
			return id;
		}
	}
}

