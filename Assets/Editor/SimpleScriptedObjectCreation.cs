using UnityEngine;
using UnityEditor;
using System.Collections;

public class SimpleScriptedObjectCreation
{

	[MenuItem("GameObject/Tutorial/Timed Message")]
	public static void CreateTimedMessage()
	{
		CreateGameObject<TimedMessage>();
	}

	[MenuItem("CONTEXT/GameObject/Tutorial/Timed Message",false,0)]
	public static void ContextCreateTimedMessage()
	{
		CreateTimedMessage();
	}

	[MenuItem("GameObject/Tutorial/Timed Ball Spawn")]
	public static void CreateTimedBallSpawn()
	{
		CreateGameObject<TimedBallSpawn>();
	}
	
	[MenuItem("CONTEXT/GameObject/Tutorial/Timed Ball Spawn",false,0)]
	public static void ContextCreateTimedBallSpawn()
	{
		CreateTimedBallSpawn();
	}


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

