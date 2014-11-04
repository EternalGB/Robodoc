using UnityEngine;
using System.Collections;

public class AddScoreArea : LevelModifier
{

	public GameObject scoreAreaPrefab;

	public override void DoModification ()
	{
		GameObject scoreArea = (GameObject)GameObject.Instantiate(scoreAreaPrefab);
		Vector3 pos;
		float radius = scoreArea.transform.localScale.x*scoreArea.GetComponent<SphereCollider>().radius;
		do {
			pos = Util.RandomPointInside(GameObject.Find ("PlayArea").collider2D);
		} while (Util.ExistsWithinSphere(pos,radius,"ScoreArea"));
		scoreArea.transform.position = pos;
	}

}

