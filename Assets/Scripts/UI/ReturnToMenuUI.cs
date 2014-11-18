using UnityEngine;
using System.Collections;

public class ReturnToMenuUI : MonoBehaviour
{

	void Update()
	{
		if(Input.GetKey(KeyCode.Escape) || Input.GetAxisRaw("Pause") == 1) {
			Application.LoadLevel("MainMenu");
		}

	}

}

