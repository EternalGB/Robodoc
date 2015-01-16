using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(LevelTiersList))]
public class LevelTiersListInspector : Editor
{

	ReorderableList tiers;

	void OnEnable()
	{
		tiers = new ReorderableList(serializedObject,serializedObject.FindProperty("tiers"),
		                             true,true,true,true);
		//SerializedProperty first = tiers.serializedProperty.GetArrayElementAtIndex(0);
		//Debug.Log (first.CountInProperty());
		LevelTiersList lm = (LevelTiersList)target;
		tiers.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

			Rect propertyRect = new Rect(rect);
			propertyRect.height = EditorGUIUtility.singleLineHeight;

			Rect unlockedRect = new Rect(propertyRect.x,propertyRect.y,60,propertyRect.height);
			Rect tierRect = new Rect(propertyRect.x + 60,propertyRect.y,propertyRect.width-60,propertyRect.height);

			EditorGUI.PropertyField(tierRect,tiers.serializedProperty.GetArrayElementAtIndex(index),GUIContent.none);
			if(lm.tiers[index] != null)
				lm.tiers[index].progress.unlocked = EditorGUI.Toggle(unlockedRect,lm.tiers[index].progress.unlocked);

		};
	}

	public override void OnInspectorGUI()
	{
		LevelTiersList lm = (LevelTiersList)target;
		//DrawDefaultInspector();


		serializedObject.Update();
		tiers.DoLayoutList();
		serializedObject.ApplyModifiedProperties();

		EditorGUILayout.BeginVertical();
		if(GUILayout.Button("Save Progress"))
			lm.SaveProgress();
		if(GUILayout.Button("Load Progress"))
			lm.LoadProgress();
		EditorGUILayout.EndVertical();
	}

}

