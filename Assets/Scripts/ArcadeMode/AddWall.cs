using UnityEngine;
using System.Collections;

public class AddWall : LevelModifier
{

	public GameObject wallPrefab;
	int tryLimit = 10;

	public override void DoModification ()
	{
		GameObject wall = (GameObject)GameObject.Instantiate(wallPrefab);
		wall.transform.localScale *= Random.Range(1f,3f);
		Vector3 pos;
		float radius = wall.transform.localScale.y;
		int tries = 0;
		do {
			pos = Util.RandomPointInside(GameObject.Find ("PlayArea").collider2D);
			tries++;
			if(tries >= tryLimit)
				return;
		} while(Util.ExistsWithinSphere(pos,radius,"Wall"));
		wall.transform.position = pos;
		wall.transform.rotation = Util.RandomRotation(Vector3.forward,0,360);
	}



}

