using UnityEngine;
using System.Collections.Generic;

public class RandomObstacleGenerator : MonoBehaviour
{

	public Collider2D spawnArea;
	public List<GameObject> obstacles;
	public int minClots, maxClots;
	public List<GameObject> paths;
	public float moveChance;
	public int minMoveSpeed, maxMoveSpeed;
	public float rotationChance;
	public int minRotSpeed, maxRotSpeed;

	int numAcross = 5;
	float padding = 40;

	void Start()
	{
		int numClots = Random.Range(minClots,maxClots);



		int tryLimit = numClots*numAcross;
		List<Vector2> positions = GeneratePositions(spawnArea.bounds, numClots/3);
		List<Bounds> obstaclesBounds = new List<Bounds>();

		int clotCount = 0;

		while(clotCount < numClots) {
			//choose an obstacle
			GameObject obstacle = (GameObject)Instantiate(Util.GetRandomElement(obstacles));
			obstacle.transform.rotation *= Util.RandomRotation(Vector3.forward,-90,90);

			//find a place with enough space for it
			Vector2 pos = Vector2.zero;
			int tries = 0;
			Bounds obstBounds = Util.GetSmallest2DObjectBounds(obstacle);
			Bounds testBounds = obstBounds;
			testBounds.extents += new Vector3(padding,padding);
			do {
				pos = Util.GetRandomElement(positions);
				testBounds.center = pos;
				tries++;

			} while(tries < tryLimit && Util.IntersectsAny(testBounds,obstaclesBounds));
			if(tries == tryLimit) {
				Destroy(obstacle);
				return;
			}

			obstacle.transform.position = pos;

			if(Random.value < moveChance) {
				WaypointFollower wf = obstacle.AddComponent<WaypointFollower>();
				wf.movementSpeed = Random.Range(minMoveSpeed,maxMoveSpeed);
				wf.waypoints = new List<Transform>();
				
				GameObject waypointParent = (GameObject)Instantiate(Util.GetRandomElement(paths));
				waypointParent.transform.rotation *= Util.RandomRotation(Vector3.forward,0,360);
				waypointParent.transform.position = pos;
				waypointParent.transform.parent = spawnArea.transform;
				
				Transform[] points = waypointParent.GetComponentsInChildren<Transform>(true);
				for(int p = 1; p < points.Length; p++) {
					wf.waypoints.Add(points[p]);
				}
			}

			if(Random.value < rotationChance) {
				WaypointFollower wf;
				if(!(wf = obstacle.GetComponent<WaypointFollower>()))
					wf = obstacle.AddComponent<WaypointFollower>();
				wf.rotSpeed = Random.Range(minRotSpeed,maxRotSpeed);
			}

			obstacle.transform.parent = spawnArea.transform;

			positions.Remove(pos);
			obstaclesBounds.Add(Util.GetSmallest2DObjectBounds(obstacle));

			int thisClots = obstacle.transform.childCount;
			clotCount += thisClots;
		}
	}

	List<Vector2> GeneratePositions(Bounds bounds, int numObs)
	{
		List<Vector2> positions = new List<Vector2>();
		//evenly space the positions 5 across and numObs down
		Vector2 spacing = new Vector2(bounds.size.x/(numAcross-1), bounds.size.y/(numObs-1));
		for(int i = 0; i < 5; i++) {
			for(int j = 0; j < numObs; j++) {
				Vector2 next = bounds.center + bounds.extents + new Vector3(-spacing.x*i,-spacing.y*j);
				positions.Add(next);
			}
		}
		return positions;
	}



}

