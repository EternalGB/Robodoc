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




	void Update()
	{
		transform.position += travelDir*speed*Time.deltaTime;
		transform.Rotate(Vector3.forward,rotSpeed*Time.deltaTime);
		//transform.RotateAround(rotCenter,Vector3.forward,rotSpeed*Time.deltaTime);
	}
			
	public override void Destroy()
	{
		//CancelInvoke("Destroy");
		PoolableTrackingParticleBurst particles = PoolManager.Instance.
			GetPoolByRepresentative(particleBurstPrefab).GetPooled().GetComponent<PoolableTrackingParticleBurst>();
		particles.transform.position = transform.position;
		particles.gameObject.SetActive(true);
		particles.particleSystem.startColor = GetParticleColor(GetComponent<SpriteRenderer>().sprite);
		particles.SetTarget(GameObject.Find ("PlayerBall").transform);
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
		//Debug.Log ("Sprite " + sprite.name + " average colour: " + color.ToString());
		HSBColor hColor = new HSBColor(color);
		//hColor.s = 1;
		hColor.b = 1;
		//Debug.Log ("Sprite " + sprite.name + " hcolour: " + hColor.ToString());
		return hColor.ToColor();
	}

}

