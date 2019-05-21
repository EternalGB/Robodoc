using UnityEngine;
using System.Collections;

public class PoolableTrackingParticleBurst : PoolableParticleBurst
{

	Transform target;
	ParticleSystem.Particle[] particles;
	int numAlive;
	public float followSpeed;
	public float shrinkDist, destroyDist;
	public AudioClip particleKillSound;

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public void Play()
	{
		base.Play();
		particles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().maxParticles];
		//Debug.Log ("Got " + numAlive + " particles");
	}

	void Update()
	{
		base.Update();
		if(target != null && hasPlayed) {
			numAlive = GetComponent<ParticleSystem>().GetParticles(particles);
			for(int i = 0; i < numAlive; i++) {
				particles[i].velocity = ((Vector2)target.position - (Vector2)particles[i].position).normalized*followSpeed;
				particles[i].angularVelocity = Mathf.Max(particles[i].velocity.x,particles[i].velocity.y)*100;
				float dist = Vector2.Distance(particles[i].position,target.position);
				if(dist <= shrinkDist) {
					particles[i].size *= (dist-destroyDist)/(shrinkDist-destroyDist);
					if(dist <= destroyDist) {
						particles[i].lifetime = -1;
						if(particleKillSound != null) {
							SoundEffectManager.Instance.PlayClipOnce(particleKillSound,Vector3.zero,1,Random.Range(0.9f,1.1f));
							target.SendMessage("GetParticle");
						}
					}
				}
			}
			GetComponent<ParticleSystem>().SetParticles(particles,numAlive);
		}
	}

}

