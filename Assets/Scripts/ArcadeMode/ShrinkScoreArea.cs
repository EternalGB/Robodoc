using UnityEngine;
using System.Collections;

public class ShrinkScoreArea : LevelModifier
{

	public float shrinkMultiplier;

	public override void DoModification ()
	{
		GameObject[] areas = GameObject.FindGameObjectsWithTag("ScoreArea");
		GameObject scoreArea = areas[Random.Range(0,areas.Length)];
		scoreArea.transform.localScale *= shrinkMultiplier;
	}
		
}

