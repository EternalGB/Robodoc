using UnityEngine;
using System.Collections;

public class ScoringBallChild : PoolableObject
{

	public GameObject particleBurstPrefab;

	public override void Destroy()
	{
		PoolableTrackingParticleBurst particles = PoolManager.Instance.
			GetPoolByRepresentative(particleBurstPrefab).GetPooled().GetComponent<PoolableTrackingParticleBurst>();
		particles.transform.position = transform.position;
		particles.gameObject.SetActive(true);
		//particles.particleSystem.startColor = ScoringBall.GetParticleColor(GetComponent<SpriteRenderer>().sprite);
		particles.SetTarget(GameObject.Find ("PlayerBall").transform);
		particles.Play();
		base.Destroy();
	}

}

