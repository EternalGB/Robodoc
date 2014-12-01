using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ArcadeStatsViewer))]
public class ArcadeStatsViewerInspector : Editor
{

	public override void OnInspectorGUI()
	{
		ArcadeStatsViewer asv = (ArcadeStatsViewer)target;
		GUILayout.BeginVertical();
		for(int i = 0; i < asv.names.Count; i++) {
			GUILayout.BeginHorizontal();
			GUILayout.Label(asv.names[i],GUILayout.Width(200));
			GUILayout.Label(asv.values[i].ToString());
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

}

