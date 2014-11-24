using UnityEngine;
using System.Collections;

public class ScoringBallChild : PoolableObject
{

	public GameObject particleBurstPrefab;
	public AttachedBall.BallTypeNames type;

	public override void Destroy()
	{
		PoolableParticleBurst particles = PoolManager.Instance.
			GetPoolByRepresentative(particleBurstPrefab).GetPooled().GetComponent<PoolableParticleBurst>();
		particles.transform.position = transform.position;
		particles.gameObject.SetActive(true);
		particles.particleSystem.startColor = Util.GetRandomPixel(GetComponent<SpriteRenderer>().sprite.texture);
		particles.Play();
		base.Destroy();
	}

}

