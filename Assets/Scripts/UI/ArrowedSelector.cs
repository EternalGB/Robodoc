using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ArrowedSelector : MonoBehaviour
{

	public int index;
	public List<string> texts;
	public Text displayText;
	public Button previousButton;
	public Button nextButton;


	public void ChangeText(int index)
	{
		if(index >= 0 && index < texts.Count) {
			//Debug.Log ("Changing index to " + index + ", text = " + texts[index]);
			this.index = index;
			displayText.text = texts[index];
			nextButton.interactable = true;
			previousButton.interactable = true;
			if(index == texts.Count-1)
				nextButton.interactable = false;
			if(index == 0)
				previousButton.interactable = false;
		}
	}

	public void NextText()
	{
		index = Mathf.Clamp(index+1,0,texts.Count-1);
		ChangeText(index);
	}

	public void PreviousText()
	{
		index = Mathf.Clamp(index-1,0,texts.Count-1);
		ChangeText(index);
	}
}

