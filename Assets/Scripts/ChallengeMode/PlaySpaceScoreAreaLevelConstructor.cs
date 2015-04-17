using UnityEngine;
using System.Collections.Generic;

public class PlaySpaceScoreAreaLevelConstructor : MonoBehaviour
{

	public WaypointFollower wf;

	public List<GameObject> waypointGroups;
	public int numGroups;
	public float minMoveSpeed, maxMoveSpeed;

	void Start()
	{
		wf.movementSpeed = Random.Range(minMoveSpeed,maxMoveSpeed);
		wf.waypoints = new List<Transform>();

		for(int i = 0; i < numGroups; i++) {
			GameObject waypointParent = (GameObject)Instantiate(Util.GetRandomElement(waypointGroups));
			waypointParent.transform.rotation *= Util.RandomRotation(Vector3.forward,0,360);
			waypointParent.transform.position = Vector2.zero;
			Transform[] points = waypointParent.GetComponentsInChildren<Transform>(true);
			for(int p = 1; p < points.Length; p++) {
				wf.waypoints.Add(points[p]);
			}
		}

		

		wf.transform.position = wf.waypoints[0].position;
	}

}

