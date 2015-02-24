using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System;

[CustomEditor(typeof(Tier))]
public class TierInspector : Editor
{
	

	ReorderableList levels;

	void OnEnable()
	{
		levels = new ReorderableList(serializedObject,serializedObject.FindProperty("levels"),
		                             true,true,true,true);
		Tier tier = (Tier)target;
		levels.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			Rect propertyRect = new Rect(rect);
			propertyRect.height = EditorGUIUtility.singleLineHeight;

			Rect unlockedRect = new Rect(propertyRect.x,propertyRect.y,60,propertyRect.height);
			Rect levelRect = new Rect(propertyRect.x + 60,propertyRect.y,propertyRect.width-60,propertyRect.height);

			EditorGUI.PropertyField(levelRect,levels.serializedProperty.GetArrayElementAtIndex(index),GUIContent.none);
			if(tier.levels[index] != null)
				tier.levels[index].progress.unlocked = EditorGUI.Toggle(unlockedRect,tier.levels[index].progress.unlocked);
		};



	}



	public override void OnInspectorGUI()
	{
		Tier tier = (Tier)target;
		//DrawDefaultInspector();

		serializedObject.Update();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("displayName"));
		tier.progress.unlocked = GUILayout.Toggle(tier.progress.unlocked,"Unlocked");

		levels.DoLayoutList();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("pointsNeededToUnlock"));

		EditorGUILayout.BeginVertical();
		if(GUILayout.Button("Save Progress"))
			tier.SaveProgress();
		if(GUILayout.Button("Load Progress"))
			tier.LoadProgress();
		EditorGUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();
	}



	
}

