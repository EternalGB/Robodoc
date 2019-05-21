using UnityEngine;
using System.Collections;

public class AddWall : LevelModifier
{

	public GameObject wallPrefab;
	int tryLimit = 10;

	public override void DoModification ()
	{
		GameObject wall = (GameObject)GameObject.Instantiate(wallPrefab);
		Vector3 pos;
		float radius = 40;
		int tries = 0;
		do {
			pos = Util.RandomPointInside(GameObject.Find ("PlayArea").GetComponent<Collider2D>());
			tries++;
			if(tries >= tryLimit)
				return;
		} while(Util.ExistsWithinSphere(pos,radius,"Wall"));
		wall.transform.position = pos;
		wall.transform.rotation = Util.RandomRotation(Vector3.forward,0,360);
	}




}

