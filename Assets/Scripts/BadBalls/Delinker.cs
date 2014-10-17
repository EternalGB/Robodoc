using UnityEngine;
using System.Collections;

public class Delinker : BadBall
{

	public Material ejectedMat;

	public override void ApplyEffect (Transform target)
	{
		if(!(target.gameObject.layer == LayerMask.NameToLayer("Player"))) {
			target.parent = transform;
			Util.SetMaterialAllChildren(target,ejectedMat);
		}
	}

	public override void Destroy ()
	{
		Util.DestroyChildren(transform);
		base.Destroy ();
	}
	

}

