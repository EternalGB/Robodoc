using UnityEngine;
using System.Collections;


public class SpawnRectangle : SpawnArea
{

	public float width,height;
	Vector3 topLeft,topRight,bottomLeft,bottomRight;

	void OnEnable()
	{
		topLeft = new Vector3(transform.position.x - width/2, transform.position.y + height/2);
		topRight = new Vector3(transform.position.x + width/2, transform.position.y + height/2);
		bottomLeft = new Vector3(transform.position.x - width/2, transform.position.y - height/2);
		bottomRight = new Vector3(transform.position.x + width/2, transform.position.y - height/2);
	}

	public override Vector3 GetPoint ()
	{
		return transform.position + new Vector3(Random.Range (-width/2,width/2),Random.Range(-height/2,height/2));
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine (topLeft,topRight);
		Gizmos.DrawLine (topRight,bottomRight);
		Gizmos.DrawLine (bottomRight,bottomLeft);
		Gizmos.DrawLine (bottomLeft,topLeft);
	}
		
}

