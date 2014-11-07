using UnityEngine;
using System.Collections;

public class EjectedBall : PoolableScreenObj
{

	public override void Destroy ()
	{
		Util.DestroyChildrenWithComponent<AttachedBall>(transform);
		base.Destroy ();
	}
}

