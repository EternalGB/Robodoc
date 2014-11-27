using UnityEngine;
using System.Collections;

public class TimedHighlighting : TimedTutorialEvent
{

	public GameObject highlightPrefab;
	public Transform target;
	GameObject highlight;

	protected override void InitEvent ()
	{

	}

	protected override void StartEvent ()
	{
		if(target != null) {
			highlight = (GameObject)GameObject.Instantiate(highlightPrefab,target.position,target.rotation);
			highlight.transform.parent = target.transform;
		}
	}

	public override void Deactivate ()
	{
		GameObject.DestroyImmediate(highlight);
	}

}

