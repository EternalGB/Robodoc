using UnityEngine;
using System.Collections.Generic;

public class Credits : MonoBehaviour
{

	public GUISkin skin;
	public TextAsset creditsFile;
	List<CreditText> credits;

	Vector3 scale;
	float origWidth = 1920;
	float origHeight = 1080;

	public float lineHeight;
	float boxHeight;
	float creditPos;

	public float scrollSpeed;

	void Start()
	{
		creditPos = origHeight;
		credits = CreditParser.ParseCreditsFile(creditsFile,skin);
	}

	void Update()
	{
		creditPos -= scrollSpeed*Time.deltaTime;
		if(Input.anyKeyDown)
			Application.LoadLevel("MainMenu");
	}

	void OnGUI()
	{
		boxHeight = credits.Count*lineHeight;


		scale.x = Screen.width/origWidth;
		scale.y = Screen.height/origHeight;
		scale.z = 1;
		Matrix4x4 lastMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero,Quaternion.identity,scale);
		GUISkin unityDef = GUI.skin;
		GUI.skin = skin;

		GUILayout.BeginArea(new Rect(new Rect(560,creditPos,800,boxHeight)));

		foreach(CreditText text in credits) {
			text.Draw(800);
		}

		GUILayout.EndArea();

		GUI.skin = unityDef;
		GUI.matrix = lastMat;
	}

}

