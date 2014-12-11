using UnityEngine;
using System.Collections;

public class ScoringBall : PoolableObject
{
	public GameObject particleBurstPrefab;


	public float lifeTime;
	public float speed;
	public Vector3 travelDir;
	public float rotSpeed;
	//public Vector3 rotCenter;

	public AttachedBall.BallTypeNames type;



	void Update()
	{
		transform.position += travelDir*speed*Time.deltaTime;
		transform.Rotate(Vector3.forward,rotSpeed*Time.deltaTime);
		//transform.RotateAround(rotCenter,Vector3.forward,rotSpeed*Time.deltaTime);
	}
			
	public override void Destroy()
	{
		//CancelInvoke("Destroy");
		PoolableParticleBurst particles = PoolManager.Instance.
			GetPoolByRepresentative(particleBurstPrefab).GetPooled().GetComponent<PoolableParticleBurst>();
		particles.transform.position = transform.position;
		particles.gameObject.SetActive(true);
		particles.particleSystem.startColor = GetParticleColor(GetComponent<SpriteRenderer>().sprite);
		particles.Play();
		Util.DestroyChildrenWithComponent<ScoringBallChild>(transform);
		base.Destroy();
	}

	public void Arm()
	{
		StartCoroutine(Timers.Countdown(lifeTime,Destroy));
	}

	public static Color GetParticleColor(Sprite sprite)
	{
		Color color = Util.GetAverageColor(sprite.texture,2);
		HSBColor hColor = new HSBColor(color);
		hColor.s = 1;
		hColor.b = 1;
		Debug.Log ("Sample color " + hColor.ToString());
		return hColor.ToColor();
	}

}

