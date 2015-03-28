using UnityEngine;
using System.Collections.Generic;

public class ColorTest : MonoBehaviour
{

	public List<GameObject> quads;

	void Start()
	{
		ChangeColors();
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.Space))
			ChangeColors();
	}


	void ChangeColors()
	{
		Palette palette = new Palette(quads.Count,0.99f,0.99f);
		for(int i = 0; i < quads.Count; i++) {
			quads[i].GetComponent<Renderer>().material.color = palette.GetColor(i);
		}
	}

}

