using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System;

[CustomEditor(typeof(Tier))]
public class TierInspector : Editor
{

	static Type[] unlockReqTypes = new Type[]{typeof(AlwaysUnlocked),typeof(RankPointRequirement)};
	static string[] unlockReqNames;
	
	ReorderableList levels;
	int selectedUnlockReq;

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


		//setup unlock req stuff
		unlockReqNames = new string[unlockReqTypes.Length];

		for(int i = 0; i < unlockReqTypes.Length; i++) {
			unlockReqNames[i] = unlockReqTypes[i].Name;
		}
		if(tier.unlockReq == null) {
			Debug.Log ("Null unlock req at OnEnable");
			selectedUnlockReq = 0;
			//tier.unlockReq = (UnlockRequirement)Activator.CreateInstance(unlockReqTypes[0]);
			tier.unlockReq = (UnlockRequirement)CreateInstance(unlockReqTypes[0]);
		} else {
			for(int i = 0; i < unlockReqTypes.Length; i++) {
				if(tier.unlockReq.GetType().Equals(unlockReqTypes[i]))
					selectedUnlockReq = i;
			}
		}

	}
	
	public override void OnInspectorGUI()
	{
		Tier tier = (Tier)target;
		//DrawDefaultInspector();

		serializedObject.Update();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("displayName"));
		tier.progress.unlocked = GUILayout.Toggle(tier.progress.unlocked,"Unlocked");

		levels.DoLayoutList();

		int newIndex = EditorGUILayout.Popup("Unlock Requirement ",selectedUnlockReq,unlockReqNames);
		if(newIndex != selectedUnlockReq) {
			//clean up the current object
			DestroyImmediate(tier.unlockReq);
			tier.unlockReq = (UnlockRequirement)CreateInstance(unlockReqTypes[newIndex]);
		}
		selectedUnlockReq = newIndex;


		if(tier.unlockReq != null) {
			tier.unlockReq.DrawInInspector();
		} else
			Debug.Log ("Null unlock req");

		serializedObject.ApplyModifiedProperties();
	}


	
}

