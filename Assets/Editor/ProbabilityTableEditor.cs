using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ProbabilityTable))]
public class ProbabilityTableEditor : Editor
{

	public override void OnInspectorGUI ()
	{
		ProbabilityTable wpt = (ProbabilityTable)target;

		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("Items");
		float tmpProb = 0;
		for(int i = 0; i < wpt.Size; i++) {
			tmpProb = wpt.probabilities[i];
			EditorGUILayout.BeginHorizontal();
			wpt.items[i] = EditorGUILayout.ObjectField(wpt.items[i],typeof(Object),true,GUILayout.Width(200));
			tmpProb = EditorGUILayout.Slider(tmpProb,0,1);//EditorGUILayout.FloatField(tmpProb,GUILayout.Width(100));
			if(GUI.changed)
				wpt.SetProbability(i,tmpProb);
			if(GUILayout.Button("Remove"))
				wpt.RemoveEntry(i);
			EditorGUILayout.EndHorizontal();
		}

		if(GUILayout.Button("Add Entry")) {
			wpt.AddItem(null);
		}

		if(GUILayout.Button ("Reset Probabilities")) {
			wpt.ResetProbabilities();
		}
		EditorGUILayout.EndVertical();
		EditorGUIUtility.LookLikeControls();

	}

}

