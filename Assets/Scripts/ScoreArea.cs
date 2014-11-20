using UnityEngine;
using System.Collections.Generic;

public class ScoreArea : MonoBehaviour
{

	public ScoreAreaAnimController anim;

	float scoreChargeStart, scoreChargeTime;
	public float scoreChargeDuration;




	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Player") && !col.GetComponent<PlayerBall>().glooped)
			scoreChargeStart = Time.time;
	}
	
	void OnTriggerStay2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Player")) {
			if(!col.GetComponent<PlayerBall>().glooped) {
				anim.fill = (Time.time - scoreChargeStart)/scoreChargeDuration;
				if(Time.time - scoreChargeStart > scoreChargeDuration) {
					ScoreCalculator.Instance.ScoreStructure();
					//ListDirectChildren(col.transform);
					//Util.DestroyChildren(col.transform);
					scoreChargeStart = Time.time;
				}
			} else {
				scoreChargeStart = Time.time;
				anim.fill = 0;
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
			anim.fill = 0;
	}
	


}

