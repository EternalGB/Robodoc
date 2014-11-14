using UnityEngine;
using System.Collections;

public class FaceVelocity2D : MonoBehaviour
{

	public Rigidbody2D targetRB;
	public Animator anim;
	Vector3 dir;
	public float speed;

	void Update()
	{
		dir = targetRB.velocity;
		if(anim != null) {
			anim.SetFloat("Speed",dir.magnitude);
			speed = dir.magnitude;
		}
		dir.Normalize();
		float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

	}

}

