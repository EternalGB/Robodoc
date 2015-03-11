using UnityEngine;
using System.Collections.Generic;

public class WorldSpaceScoreAreaLevelConstructor : MonoBehaviour
{

	public Collider2D area;
	public GameObject scoreAreaPrefab;
	public List<GameObject> waypointGroups;

	public int minScoreAreas, maxScoreAreas;
	public float movingChance;
	public float minMoveSpeed, maxMoveSpeed;

	void Start()
	{
		int numScoreAreas = Random.Range(minScoreAreas,maxScoreAreas);
		float spacing = area.bounds.size.y/(numScoreAreas-1);
		for(int i = 0; i < numScoreAreas; i++) {
			Vector2 pos = GetRandomPosition(area.bounds, -i*spacing);
			GameObject scoreArea = (GameObject)Instantiate(scoreAreaPrefab,pos,Quaternion.identity);
			if(Random.value < movingChance) {
				WaypointFollower wf = scoreArea.AddComponent<WaypointFollower>();
				wf.movementSpeed = Random.Range(minMoveSpeed,maxMoveSpeed);
				wf.waypoints = new List<Transform>();

				GameObject waypointParent = (GameObject)Instantiate(Util.GetRandomElement(waypointGroups));
				waypointParent.transform.rotation *= Util.RandomRotation(Vector3.forward,0,360);
				waypointParent.transform.position = pos;
				waypointParent.transform.parent = area.transform;

				Transform[] points = waypointParent.GetComponentsInChildren<Transform>(true);
				for(int p = 1; p < points.Length; p++) {
					wf.waypoints.Add(points[p]);
				}
			}
			scoreArea.transform.parent = area.transform;
		}
	}

	Vector2 GetRandomPosition(Bounds bounds, float y)
	{
		Vector2 pos = new Vector2();
		pos.x = bounds.center.x + Random.Range(-bounds.extents.x,bounds.extents.x);
		pos.y = (bounds.center.y + bounds.extents.y) + y;
		return pos;
	}

}

