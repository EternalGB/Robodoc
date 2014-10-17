using UnityEngine;
using System.Collections;

public class DoubleColumn : CreditText
{
	
	public GUIStyle style2;

	public string text1;
	public string text2;

	public override void Draw (float width)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(text1,style, GUILayout.Width(width/2));
		GUILayout.Label(text2,style2, GUILayout.Width (width/2));
		GUILayout.EndHorizontal();
	}

}

