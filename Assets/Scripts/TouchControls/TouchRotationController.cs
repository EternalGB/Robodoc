using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchRotationController : TouchPadController
{

	
	public override Vector2 GetDirection()
	{
		if(Mathf.Abs(direction.x) > 0)
			direction.x = Mathf.Sign(direction.x);
		direction.y = 0;
		return direction;
	}
	


}

