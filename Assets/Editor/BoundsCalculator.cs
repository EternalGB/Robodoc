using UnityEngine;
using UnityEditor;
using System.Collections;

public class BoundsCalculator 
{

	[MenuItem("GameObject/Calculate 2D Bounds",false,0)]
	public static void Calculate2DBounds()
	{
		GameObject target = Selection.activeGameObject;
		Bounds bounds = Util.GetSmallest2DObjectBounds(target);
		GameObject boundObj = new GameObject();
		boundObj.transform.position = bounds.center;
		BoxCollider2D col = boundObj.AddComponent<BoxCollider2D>();
		col.size = bounds.size;
		boundObj.name = "Bounds";
		boundObj.transform.parent = target.transform;
	}

	[MenuItem("GameObject/Calculate 2D Bounds",true,0)]
	public static bool ValidateCalculateBounds()
	{
		Collider2D[] cols = Selection.activeGameObject.GetComponentsInChildren<Collider2D>();
		return cols.Length > 0;
	}

}

