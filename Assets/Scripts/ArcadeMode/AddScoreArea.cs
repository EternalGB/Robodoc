using UnityEngine;
using System.Collections;

public class AddScoreArea : LevelModifier
{

	public GameObject scoreAreaPrefab;
	int tryLimit = 10;

	public override void DoModification ()
	{
		GameObject scoreArea = (GameObject)GameObject.Instantiate(scoreAreaPrefab);
		Collider2D allowableArea = GameObject.Find ("PlayArea").GetComponent<Collider2D>();
		Vector3 pos;
		float radius = scoreArea.transform.localScale.x*scoreArea.GetComponent<CircleCollider2D>().radius;
		int tries = 0;
		do {
			pos = Util.RandomPointInside(allowableArea);
			tries++;
			//bail out if we try too many times
			if(tries >= tryLimit)
				return;
		} while (Util.ExistsWithinSphere(pos,radius,"ScoreArea"));
		scoreArea.transform.position = pos;
	}

}

