using UnityEngine;
using System.Collections;

public class Freezer : BadBall
{

	public override void ApplyEffect (Transform target)
	{
		throw new System.NotImplementedException ();
	}

	public override void Destroy ()
	{
		base.Destroy ();
	}

}

