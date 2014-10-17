using UnityEngine;
using System.Collections;

public class EmptySpace : CreditText
{

	public EmptySpace()
	{
		style = new GUIStyle();
		style.margin = new RectOffset(0,0,20,20);
	}

	public override void Draw (float width)
	{
		GUILayout.Box ("",style);
	}

}

