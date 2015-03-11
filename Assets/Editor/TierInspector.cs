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


			Rect levelRect = new Rect(propertyRect.x,propertyRect.y,240,propertyRect.height);
			Rect unlockedRect = new Rect(propertyRect.x + 240,propertyRect.y,60,propertyRect.height);
			Rect pointRect = new Rect(propertyRect.x + 300,propertyRect.y,propertyRect.width - 300,propertyRect.height); 




			EditorGUI.PropertyField(levelRect,levels.serializedProperty.GetArrayElementAtIndex(index),GUIContent.none);
			SerializedProperty level = levels.serializedProperty.GetArrayElementAtIndex(index);
			EditorGUI.BeginChangeCheck();
			if(tier.levels[index] != null) {
				//EditorGUI.PropertyField(unlockedRect,level.serializedObject.FindProperty("unlocked"),GUIContent.none);
				//EditorGUI.PropertyField(pointRect,level.serializedObject.FindProperty("pointsNeededToUnlock"),GUIContent.none);
				tier.levels[index].progress.unlocked = EditorGUI.Toggle(unlockedRect,tier.levels[index].progress.unlocked);
				tier.levels[index].pointsNeededToUnlock = EditorGUI.IntField(pointRect,tier.levels[index].pointsNeededToUnlock);
			}

			if(EditorGUI.EndChangeCheck()) {
				EditorUtility.SetDirty(tier.levels[index]);
				//AssetDatabase.SaveAssets();
			}
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

