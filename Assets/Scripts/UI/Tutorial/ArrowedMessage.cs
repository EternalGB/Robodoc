using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ArrowedMessage : TutorialEvent
{

	public GameObject arrowedMessagePrefab;
	public Vector2 offsetDir;
	public Transform target;
	public string message;

	GameObject arrowedMessage;
	Vector3 offset;

	public override void Activate ()
	{
		if(target != null) {
			//create and position the arrow box so that it's outside the target and pointing at it
			Vector3 targetOffset = Vector3.zero;
			if(target.GetComponent<Collider2D>()) {
				Bounds bounds = target.GetComponent<Collider2D>().bounds;
				targetOffset = new Vector3(offsetDir.x*bounds.size.x/2,
				                           offsetDir.y*bounds.size.y/2,
				                           0);
			} else if(target.GetComponent<RectTransform>()) {
				Rect rect = target.GetComponent<RectTransform>().rect;
				targetOffset = new Vector3(offsetDir.x*rect.width/2,
				                           offsetDir.y*rect.height/2,
				                           0);
			}
			offset = new Vector3(offsetDir.x*arrowedMessagePrefab.GetComponent<RectTransform>().rect.width/2,
			                             offsetDir.y*arrowedMessagePrefab.GetComponent<RectTransform>().rect.height/2,
			                             0) + targetOffset;
			arrowedMessage = (GameObject)GameObject.Instantiate(arrowedMessagePrefab,target.position + offset,Quaternion.identity);
			arrowedMessage.GetComponentInChildren<Text>().text = message;
		}
	}

	void Update()
	{
		if(arrowedMessage != null && target != null)
			arrowedMessage.transform.position = target.position + offset;
	}

	public override void Deactivate ()
	{
		if(arrowedMessage != null)
			DestroyImmediate(arrowedMessage);
	}

}

