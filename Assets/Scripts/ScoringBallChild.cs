using UnityEngine;
using System.Collections;

public class ScoringBallChild : PoolableObject
{

	public GameObject particleBurstPrefab;

	public override void Destroy()
	{
		PoolableParticleBurst particles = PoolManager.Instance.
			GetPoolByRepresentative(particleBurstPrefab).GetPooled().GetComponent<PoolableParticleBurst>();
		particles.transform.position = transform.position;
		particles.gameObject.SetActive(true);
		particles.particleSystem.startColor = ScoringBall.GetParticleColor(GetComponent<SpriteRenderer>().sprite);
		particles.Play();
		base.Destroy();
	}

}

