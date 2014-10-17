using UnityEngine;
using System.Collections;

public class WaypointPathDrawer : MonoBehaviour
{

	Transform[] points;

	void OnDrawGizmos()
	{
		points = GetComponentsInChildren<Transform>();
		if(points != null && points.Length > 2) {
			for(int i = 1; i < points.Length-1; i++) {
				Gizmos.DrawLine (points[i].position,points[i+1].position);
			}
			Gizmos.DrawLine(points[points.Length-1].position,points[1].position);
		}
	}

}

