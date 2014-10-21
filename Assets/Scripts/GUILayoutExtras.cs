using UnityEngine;
using System.Collections;

public class GUILayoutExtras
{

	public static int ArrowedSelector(int selection, string[] texts, GUIStyle[] buttonStyles, GUIStyle labelStyle)
	{
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("",buttonStyles[0]))
			selection = Mathf.Clamp(selection-1,0,texts.Length-1);
		GUILayout.Label(texts[selection],labelStyle);
		if(GUILayout.Button("",buttonStyles[1]))
			selection = Mathf.Clamp(selection+1,0,texts.Length-1);
		GUILayout.EndHorizontal();
		return selection;
	}

}

