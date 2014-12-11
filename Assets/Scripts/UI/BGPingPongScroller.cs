using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BGController))]
public class BGPingPongScroller : MonoBehaviour
{

	BGController bgc;
	float scroll;
	public float scrollRate;

	void Start()
	{
		bgc = GetComponent<BGController>();
		scroll = 0;
	}

	void Update()
	{
		scroll = scroll + scrollRate*Time.deltaTime;
		bgc.UpdatePos(Mathf.PingPong (scroll,1));
	}

}

