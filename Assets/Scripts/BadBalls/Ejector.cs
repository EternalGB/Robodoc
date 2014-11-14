using UnityEngine;
using System.Collections.Generic;

public class Ejector : BadBall
{
	public GameObject ejectedBallPrefab;
	public float ejectionForce;

	public override void ApplyEffect (Transform target)
	{
		if(target.gameObject.name == "PlayerBall") {
			EjectChildren(target);
		}
	}

	void EjectChildren(Transform parent)
	{


		List<Transform> toEject = new List<Transform>();
		foreach(Transform child in parent) {
			if(child.GetComponent<AttachedBall>())
				toEject.Add(child);
		}
		//have to do two passes because we need to set parent to null
		//and this modifies what we're enumerating through
		foreach(Transform child in toEject) {
			GameObject ejected = PoolManager.Instance.GetPoolByRepresentative(ejectedBallPrefab).GetPooled();
			Vector3 pos = child.position;
			Quaternion rot = child.rotation;
			ejected.transform.position = pos;
			ejected.transform.rotation = rot;
			ejected.GetComponent<SpriteRenderer>().sprite = child.GetComponent<SpriteRenderer>().sprite;
			foreach(Transform c in child)
				c.parent = ejected.transform;
			child.SendMessage("Destroy");

			Util.SetMaterialAllAttachedBalls(ejected.transform,ejected.GetComponent<SpriteRenderer>().sharedMaterial);
			ejected.SetActive(true);
			ejected.rigidbody2D.velocity = (pos - parent.position).normalized*ejectionForce;
		}
	}





}

