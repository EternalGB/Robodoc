using UnityEngine;
using System.Collections;

public class EjectedBall : PoolableScreenObj
{

	public override void Destroy ()
	{
		Util.DestroyChildren(transform);
		base.Destroy ();
	}
}

