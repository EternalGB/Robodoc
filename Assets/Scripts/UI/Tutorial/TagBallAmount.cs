using UnityEngine;
using System.Collections;

public abstract class TagBallAmount : TagBalls
{

	public int amountToTag;

	protected override bool CheckCompleted ()
	{
		return numTagged >= amountToTag;
	}

}

