using UnityEngine;
using System.Collections;

public class Delinker : BadBall
{

	public Material ejectedMat;

	protected override void ApplyEffect (Transform target)
	{
		if(!(target.gameObject.layer == LayerMask.NameToLayer("Player"))) {
			//Debug.Log ("Delinker Ball Applying Effect - Target: " + target.GetInstanceID());
			target.parent = transform;
			AttachedBall.AddStatusAllAttachedBalls(target,BallStatus.EJECTED,ejectedMat);
		}
	}

	public override void Destroy ()
	{
		Util.DestroyChildrenWithComponent<AttachedBall>(transform);
		base.Destroy ();
	}
	

}

