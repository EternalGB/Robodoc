using UnityEngine;
using System.Collections.Generic;

public class WaypointFollower : MonoBehaviour
{

	public float movementSpeed;
	public float rotSpeed;
	public List<Transform> waypoints;
	int wpIndex = 0;
	


	void Start()
	{
		if(waypoints != null && waypoints.Count > 0) {
			transform.position = waypoints[0].position;
		}
	}
	
	void Update()
	{
		if(waypoints != null && waypoints.Count > 0) {
			Vector3 nextMove;
			nextMove = (waypoints[wpIndex].position - transform.position).normalized*movementSpeed*Time.deltaTime;
			//always touch the waypoint
			Vector3.ClampMagnitude(nextMove,Vector3.Distance(transform.position,waypoints[wpIndex].position));
			transform.position += nextMove;
			if(Vector3.Distance(transform.position,waypoints[wpIndex].position) < 5f)
				wpIndex = (wpIndex+1)%waypoints.Count;
			transform.rotation *= Quaternion.AngleAxis(rotSpeed*Time.deltaTime,Vector3.forward);
		}
	}

}

