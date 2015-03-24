using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.UI;
using System.Collections;

public class TouchMovementController : TouchPadController
{

	public override Vector2 GetDirection()
	{
		direction.Normalize();
		return direction;
	}
	
}

