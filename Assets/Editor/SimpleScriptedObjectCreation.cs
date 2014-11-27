using UnityEngine;
using UnityEditor;
using System.Collections;

public class SimpleScriptedObjectCreation
{

	public static void CreateGameObject<T>() where T : MonoBehaviour
	{
		GameObject go = new GameObject();
		go.AddComponent<T>();
		go.transform.position = Vector3.zero;
		
		if(Selection.activeGameObject != null)
			go.transform.parent = Selection.activeGameObject.transform;
		Selection.activeGameObject = go;
	}
}

