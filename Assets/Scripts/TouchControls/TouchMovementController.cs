using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.UI;
using System.Collections;

public class TouchMovementController : TouchPadController
{

	public override Vector2 GetDirection()
	{
		return direction;
	}
	
}

