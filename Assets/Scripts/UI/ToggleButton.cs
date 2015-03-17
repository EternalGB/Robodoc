using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Toggle))]
public class ToggleButton : MonoBehaviour
{

	Toggle toggle;
	public Sprite on, off;
	Image image;

	void OnEnable()
	{
		toggle = GetComponent<Toggle>();
		toggle.onValueChanged.AddListener(ValueChanged);
		image = GetComponentInChildren<Image>();
	}

	void ValueChanged(bool value)
	{
		if(value)
			image.sprite = on;
		else
			image.sprite = off;
		image.SetNativeSize();
	}

	void OnDisable()
	{
		toggle.onValueChanged.RemoveListener(ValueChanged);
	}

}

