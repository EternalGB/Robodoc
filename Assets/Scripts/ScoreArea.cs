using UnityEngine;
using System.Collections.Generic;

public class ScoreArea : MonoBehaviour
{

	public ScoreAreaAnimController anim;

	float scoreChargeStart, scoreChargeTime;
	public float scoreChargeDuration;




	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
			scoreChargeStart = Time.time;
	}
	
	void OnTriggerStay(Collider col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Player")) {
			anim.fill = (Time.time - scoreChargeStart)/scoreChargeDuration;
			if(Time.time - scoreChargeStart > scoreChargeDuration) {
				ScoreCalculator.Instance.ScoreStructure();
				//ListDirectChildren(col.transform);
				//Util.DestroyChildren(col.transform);
				scoreChargeStart = Time.time;
			}
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
			anim.fill = 0;
	}
	


}

