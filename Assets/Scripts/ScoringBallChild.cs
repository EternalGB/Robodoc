using UnityEngine;
using System.Collections;

public class ScoringBallChild : PoolableObject
{

	public GameObject trackingParticleBurst;
	public GameObject particleBurst;

	public override void Destroy()
	{
		PoolableTrackingParticleBurst particles = PoolManager.Instance.
			GetPoolByRepresentative(trackingParticleBurst).GetPooled().GetComponent<PoolableTrackingParticleBurst>();
		particles.transform.position = transform.position;
		particles.gameObject.SetActive(true);
		//particles.particleSystem.startColor = ScoringBall.GetParticleColor(GetComponent<SpriteRenderer>().sprite);
		particles.SetTarget(GameObject.Find ("PlayerBall").transform);
		particles.Play();

		PoolableParticleBurst pBurst = PoolManager.Instance.GetPoolByRepresentative(particleBurst).GetPooled().GetComponent<PoolableParticleBurst>();
		pBurst.transform.position = transform.position;
		pBurst.gameObject.SetActive(true);
		pBurst.Play();

		base.Destroy();
	}

}

