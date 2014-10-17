using UnityEngine;
using System.Collections;

public class SingleColumn : CreditText
{

	public string text;

	public override void Draw (float width)
	{
		GUILayout.Label(text,style);
	}

}

