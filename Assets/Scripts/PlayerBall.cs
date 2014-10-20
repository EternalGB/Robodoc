using UnityEngine;
using System.Collections;

public class PlayerBall : MonoBehaviour
{

	public GameObject ballPrefab;
	ObjectPool ballPool;
	public float speed, rotSpeed;
	Vector3 mousePos;
	public int numBombs = 3;
	float bombForce = 10000;
	

	public Collider2D playArea;
	public float edgeOffset = 20;
	float rotAngle = 0;

	void Start()
	{
		ballPool = PoolManager.Instance.GetPoolByRepresentative(ballPrefab);
	}

	void Update()
	{

		if(PlayerPrefs.GetInt("Controller") == 1) {
			//move towards the mouse
			mousePos = Util.MouseToWorldPos(0);
			rigidbody.velocity = Vector3.ClampMagnitude((mousePos - transform.position)*speed,speed);
		} else {
			//use the keyboard
			rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal")*speed,Input.GetAxis("Vertical")*speed);
		}

		if(Input.GetButtonDown("Bomb")) {
			FireBomb();
		}

		//spin left or right
		rotAngle = Mathf.Repeat(rotAngle + Input.GetAxisRaw("Spin")*rotSpeed*Time.deltaTime,360);
		transform.rotation = Quaternion.AngleAxis(rotAngle,Vector3.forward);
		//rigidbody.angularVelocity += new Vector3(0,0,rigidbody.velocity.x + rigidbody.velocity.y)*-0.1f;




	}

	void FixedUpdate()
	{
		if(!playArea.OverlapPoint(transform.position)) {
			if( Vector2.Distance(playArea.transform.position,transform.position + rigidbody.velocity) >
			   Vector2.Distance(playArea.transform.position,transform.position)) {
				rigidbody.velocity = rigidbody.velocity - transform.position.normalized*rigidbody.velocity.magnitude;
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("GoodBall")) {
			PointBall pb;
			if(pb = col.gameObject.GetComponent<PointBall>()) {
				ContactPoint[] cps = col.contacts;
				Vector3 pos = pb.transform.position;
				Quaternion rot = pb.transform.rotation;
				Vector3 scale = pb.transform.localScale;
				Material mat = pb.renderer.sharedMaterial;
				pb.Destroy();
				AttachedBall ball = ballPool.GetPooled().GetComponent<AttachedBall>();
				//make sure it's not bringing any friends
				Util.DestroyChildren(ball.transform);
				ball.transform.position = pos;
				ball.transform.rotation = rot;
				ball.transform.localScale = scale;
				ball.renderer.sharedMaterial = mat;
				ball.gameObject.SetActive(true);
				Transform parent = GetCorrectParent(cps);
				ball.transform.parent = parent;
				if(parent == null)
					Debug.LogError("No correct parent could be found");
				ball.SetPointValue(pb.pointValue);

				//Debug.Log ("Creating Attached Ball at " + pos + " parent: " + ball.transform.parent.GetInstanceID());

				ScoreCalculator.Instance.ComboUp();
				//recalculate score
				ScoreCalculator.Instance.SetScorePrediction();
				
				int depth = FindDepth(ball.transform);
				//Debug.Log ("Depth of " + ball.transform.GetInstanceID() + " " + depth);
				if(depth > 0) {
					SoundEffectManager.Instance.PlayClipOnce("BallGet",Vector3.zero,1,1 + depth/2f);
				}
			}
		} else if(col.gameObject.layer == LayerMask.NameToLayer("BadBall")) {
			ContactPoint[] cps = col.contacts;
			col.gameObject.GetComponent<BadBall>().ApplyEffect(cps[0].thisCollider.transform);
			//recalculate score
			ScoreCalculator.Instance.SetScorePrediction();
			SoundEffectManager.Instance.PlayClipOnce("BadHit",Vector3.zero,1,1);
		}
	}

	//returns the first transform that is the player or an attached ball
	Transform GetCorrectParent(ContactPoint[] cps)
	{

		foreach(ContactPoint cp in cps) {
			if(cp.thisCollider.GetComponent<AttachedBall>() || cp.thisCollider.GetComponent<PlayerBall>())
				return cp.thisCollider.transform;
			if(cp.otherCollider.GetComponent<AttachedBall>() || cp.otherCollider.GetComponent<PlayerBall>())
				return cp.otherCollider.transform;
		}
		return null;
	}

	int FindDepth(Transform child)
	{
		//Debug.Log ("Finding depth of " + child.GetInstanceID());
		Transform cursor = child;
		int depth = 0;
		while(cursor.parent != null) {
			//Debug.Log ("Non-null parent " + cursor.parent.GetInstanceID());
			if(cursor.renderer.sharedMaterial.name == "AnyColour" ||
			   cursor.parent.renderer.sharedMaterial.name == "PlayerBall" ||
			   cursor.parent.renderer.sharedMaterial.name == "AnyColour" || 
			   cursor.parent.renderer.sharedMaterial.Equals(cursor.renderer.sharedMaterial)) {
				//Debug.Log ("Materials match");
				depth++;
			} else
				depth = 0;
			cursor = cursor.parent;
		}
		return depth;

		/*
		foreach(Transform c in parent) {
			if(c.GetInstanceID() == child.GetInstanceID())
				return depth+1;
			else {
				if(c.renderer.sharedMaterial.name == "AnyColour" || child.renderer.sharedMaterial.name == "AnyColour" ||
				   c.renderer.sharedMaterial.Equals(child.renderer.sharedMaterial)) {
					return FindDepth(child,c,depth+1);
				} else {
					return FindDepth(child,c,0);
				}
			}
		}
		return -1;
		*/
	}

	void FireBomb()
	{
		SoundEffectManager.Instance.PlayClipOnce("Bomb",Vector3.zero,1,1);
		GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
		foreach(GameObject ball in balls) {
			if(ball.rigidbody) {
				ball.rigidbody.AddExplosionForce(bombForce,Vector3.zero,200);
			}
		}
		numBombs--;
	}
}

