using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;

public class AssetCreation 
{

	[MenuItem("Assets/Create/Level")]
	public static void CreateLevel()
	{
		CreateAsset<Level>();
	}

	[MenuItem("Assets/Create/Goal/High Score")]
	public static void CreateHighScore()
	{
		CreateAsset<HighScore>();
	}

	[MenuItem("Assets/Create/Goal/Longest Chain")]
	public static void CreateLongestChain()
	{
		CreateAsset<LongestChain>();
	}

	[MenuItem("Assets/Create/Goal/Score Target")]
	public static void CreateScoreTarget()
	{
		CreateAsset<ScoreTarget>();
	}

	[MenuItem("Assets/Create/Goal/Biggest Combo")]
	public static void CreateBiggestCombo()
	{
		CreateAsset<BiggestCombo>();
	}


	public static void CreateAsset<T> () where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
		
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}

	[MenuItem("Assets/Clone Scriptable Obj")]
	public static void CloneScriptableObj()
	{
		CopyAsset();
	}

	[MenuItem("Assets/Clone Scriptable Obj",true)]
	public static bool ValidateCloneScriptableObj()
	{
		return HasTypeInHierarchy(Selection.activeObject.GetType(),typeof(ScriptableObject));
	}

	static bool HasTypeInHierarchy(Type type, Type searchType)
	{
		while(type.BaseType != null) {
			if(type.Equals(searchType))
				return true;
			type = type.BaseType;
		}
		return false;
	}

	public static void CopyAsset()
	{
		string path = AssetDatabase.GetAssetPath(Selection.activeObject);
		string filename = Path.GetFileNameWithoutExtension(path);
		string newPath = path.Replace(filename,filename + " - Copy");
		AssetDatabase.CopyAsset(path,newPath);
		//AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
	}
}
