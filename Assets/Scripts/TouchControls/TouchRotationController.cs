using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchRotationController : TouchPadController
{

	
	public override Vector2 GetDirection()
	{
		direction.y = 0;
		return direction;
	}
	


}

