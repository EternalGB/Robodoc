using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndTreatmentDisabler : MonoBehaviour
{

	void OnEnable()
	{
		ChallengeGUI manager = GameObject.Find("ChallengeGUIManager").GetComponent<ChallengeGUI>();
		if(manager.goal != null && manager.goal.GetType().IsAssignableFrom(typeof(ScoreTarget)))
			GetComponent<Button>().interactable = false;
	}

}

