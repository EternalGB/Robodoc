using UnityEngine;
using System.Collections;

public class ReturnToMenuUI : MonoBehaviour
{

	void Update()
	{
		if(Input.GetKey(KeyCode.Escape) || Input.GetAxisRaw("Pause") == 1 || Input.GetMouseButtonDown(0)
		   || Input.GetMouseButtonDown(1)) {
			Application.LoadLevel("MainMenu");
		}

	}

}

