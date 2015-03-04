using UnityEngine;
using System.Collections;

public class PoolableTrackingParticleBurst : PoolableParticleBurst
{

	Transform target;
	ParticleSystem.Particle[] particles;
	int numAlive;
	public float followSpeed;
	public float shrinkDist, destroyDist;

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public void Play()
	{
		base.Play();
		particles = new ParticleSystem.Particle[particleSystem.maxParticles];

		//Debug.Log ("Got " + numAlive + " particles");
	}

	void Update()
	{
		base.Update();
		if(target != null && hasPlayed) {
			numAlive = particleSystem.GetParticles(particles);
			for(int i = 0; i < numAlive; i++) {
				particles[i].velocity = ((Vector2)target.position - (Vector2)particles[i].position).normalized*followSpeed;
				particles[i].angularVelocity = Mathf.Max(particles[i].velocity.x,particles[i].velocity.y)*1000;
				float dist = Vector2.Distance(particles[i].position,target.position);
				if(dist <= shrinkDist) {
					particles[i].size *= (dist-destroyDist)/(shrinkDist-destroyDist);
					if(dist <= destroyDist) {
						particles[i].lifetime = -1;
					}
				}
			}
			particleSystem.SetParticles(particles,numAlive);
		}
	}

}
