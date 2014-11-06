using UnityEngine;
using System.Collections;

public class AddWall : LevelModifier
{

	public GameObject wallPrefab;
	int tryLimit = 10;

	public override void DoModification ()
	{
		GameObject wall = (GameObject)GameObject.Instantiate(wallPrefab);
		wall.transform.localScale += new Vector3(0,Random.Range(0f,15f),0);
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

