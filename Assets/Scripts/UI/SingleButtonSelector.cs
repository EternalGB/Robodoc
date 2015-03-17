using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class SingleButtonSelector : MonoBehaviour
{

	int index;
	public List<string> texts;
	public Text displayText;

	void OnEnable()
	{
		if(texts != null && texts.Count > 0) {
			displayText.text = texts[0];
		}
		GetComponent<Button>().onClick.AddListener(SelectNext);
	}

	public void SelectNext()
	{
		index = (index+1)%texts.Count;
		displayText.text = texts[index];
	}


	public void SetIndex(int index)
	{
		if(index >= 0 && index < texts.Count) {
			this.index = index;
			displayText.text = texts[index];
		}
	}

	public int GetIndex()
	{
		return index;
	}

	void OnDisable()
	{
		GetComponent<Button>().onClick.RemoveListener(SelectNext);
	}
}

