using UnityEngine;
using System.Collections;

public class AddWall : LevelModifier
{

	public GameObject wallPrefab;

	public override void DoModification ()
	{
		GameObject wall = (GameObject)GameObject.Instantiate(wallPrefab);
		Vector3 pos;
		float radius = wall.transform.localScale.y;
		do {
			pos = Util.RandomPointInside(GameObject.Find ("PlayArea").collider2D);
		} while(Util.ExistsWithinSphere(pos,radius,"Wall"));
		wall.transform.position = pos;
		wall.transform.rotation = Util.RandomRotation(Vector3.forward,0,360);
	}



}

