using UnityEngine;
using System.Collections;

public class PlayerBall : MonoBehaviour
{

	public GameObject ballPrefab;
	ObjectPool ballPool;
	public float speed, rotSpeed;
	Vector3 mousePos;
	public int numBombs = 3;
	float bombForce = 1000;
	

	public Collider2D playArea;
	//public float edgeOffset = 20;
	float rotAngle = 0;

	bool movFreeze = false;
	bool rotFreeze = false;
	public bool glooped = true;

	public delegate void BallCollectHandler(GameObject player, GameObject playerPart, GameObject ball);
	public event BallCollectHandler BallCollect;

	void Start()
	{
		ballPool = PoolManager.Instance.GetPoolByRepresentative(ballPrefab);
	}

	void Update()
	{
		//to stop new colliders changing the center of mass
		rigidbody2D.centerOfMass = Vector2.zero;

		if(!movFreeze ){
			if(PlayerPrefs.GetInt("Controller",1) == 1) {
				//move towards the mouse
				
				mousePos = Util.MouseToWorldPos(0);
				if(Vector2.Distance(mousePos,transform.position) >= 2)
					rigidbody2D.velocity = (mousePos - transform.position).normalized*speed;
				else
					rigidbody2D.velocity = Vector2.zero;
			} else {
				//use the keyboard
				rigidbody2D.velocity = new Vector3(Input.GetAxis("Horizontal")*speed,Input.GetAxis("Vertical")*speed);
			}
		}


		if(Input.GetButtonDown("Bomb")) {
			FireBomb();
		}

		//spin left or right
		if(!rotFreeze) {
			rotAngle = Mathf.Repeat(rotAngle + Input.GetAxisRaw("Spin")*rotSpeed*Time.deltaTime,360);
			transform.rotation = Quaternion.AngleAxis(rotAngle,Vector3.forward);
			//rigidbody2D.angularVelocity += new Vector3(0,0,rigidbody2D.velocity.x + rigidbody2D.velocity.y)*-0.1f;
		}


		Debug.DrawLine(transform.position,(Vector2)transform.position+rigidbody2D.velocity,Color.blue,0f);


	}

	void FixedUpdate()
	{
		if(!playArea.OverlapPoint(transform.position)) {
			if( Vector2.Distance(playArea.transform.position,(Vector2)transform.position + rigidbody2D.velocity) >
			   Vector2.Distance(playArea.transform.position,transform.position)) {
				rigidbody2D.velocity = Vector2.zero;//rigidbody2D.velocity - (Vector2)transform.position.normalized*rigidbody2D.velocity.magnitude;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(!glooped) {
			ContactPoint2D[] cps = col.contacts;
			Transform parent = GetCorrectParent(cps);
			GameObject playerPart = parent.gameObject;
			GameObject collidingBall = col.gameObject;
			if(col.gameObject.layer == LayerMask.NameToLayer("GoodBall")) {
				PointBall pb;
				if(pb = col.gameObject.GetComponent<PointBall>()) {

					Vector3 pos = pb.transform.position;
					Quaternion rot = pb.transform.rotation;
					Vector3 scale = pb.transform.localScale;
					Material mat = pb.GetComponent<SpriteRenderer>().sharedMaterial;
					Sprite sprite = pb.GetComponent<SpriteRenderer>().sprite;
					pb.Destroy();
					AttachedBall ball = ballPool.GetPooled().GetComponent<AttachedBall>();
					//make sure it's not bringing any friends
					Util.DestroyChildrenWithComponent<AttachedBall>(ball.transform);
					ball.transform.position = pos;
					ball.transform.rotation = rot;
					ball.GetComponent<SpriteRenderer>().sprite = sprite;
					//ball.transform.localScale = scale;
					ball.GetComponent<SpriteRenderer>().sharedMaterial = mat;
					ball.gameObject.SetActive(true);

					ball.transform.parent = parent;
					if(parent == null)
						Debug.LogError("No correct parent could be found");
					ball.SetPointValue(pb.pointValue);
					ball.SetType(pb.gameObject.name);
					
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
				//Debug.Log ("BadBall Collision");
				col.gameObject.GetComponent<BadBall>().ApplyEffect(cps[0].otherCollider.transform);
				//recalculate score
				ScoreCalculator.Instance.SetScorePrediction();
				SoundEffectManager.Instance.PlayClipOnce("BadHit",Vector3.zero,1,1);
			}
			BallCollect(gameObject,playerPart,collidingBall);
		}
	}

	//returns the first transform that is the player or an attached ball
	Transform GetCorrectParent(ContactPoint2D[] cps)
	{

		foreach(ContactPoint2D cp in cps) {
			if(cp.collider.GetComponent<AttachedBall>() || cp.collider.GetComponent<PlayerBall>())
				return cp.collider.transform;
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
			if(ScoreCalculator.ComboValid(cursor.parent,cursor)) {
				//Debug.Log ("Materials match");
				depth++;
			} else
				depth = 0;
			cursor = cursor.parent;
		}
		return depth;
	}

	void FireBomb()
	{
		if(numBombs > 0) {
			SoundEffectManager.Instance.PlayClipOnce("Bomb",Vector3.zero,1,1);
			GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
			foreach(GameObject ball in balls) {
				if(ball.rigidbody2D) {
					ball.rigidbody2D.velocity += (Vector2)ball.transform.position.normalized*bombForce;
				}
			}
			numBombs--;
		}
	}

	public void GetFrozen(float duration, Material frozenMat)
	{
		if(!movFreeze) {
			movFreeze = true;
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.isKinematic = true;
			Util.SetMaterialAllAttachedBallsTemp(transform,frozenMat,duration);
			StartCoroutine(Timers.Countdown(duration,() => {
				movFreeze = false;
				rigidbody2D.isKinematic = false;
			}));
		}
	}

	public void GetShocked(float duration, Material shockedMat)
	{
		if(!rotFreeze) {
			rotFreeze = true;
			Util.SetMaterialAllAttachedBallsTemp(transform,shockedMat,duration);
			StartCoroutine(Timers.Countdown(duration,() => {rotFreeze = false;}));
		}
	}

	public void GetGlooped(float duration, Material gloopMaterial)
	{
		if(!glooped) {
			glooped = true;
			//we make copies so the delegate we create keeps its own state
			/*
			Material oldMat = GetComponent<SpriteRenderer>().sharedMaterial;
			Transform trans = transform;
			GetComponent<SpriteRenderer>().sharedMaterial = gloopMaterial;
			StartCoroutine(Timers.Countdown(duration,() => {
				Debug.Log ("Resetting mat to " + oldMat.name + " " + oldMat.GetInstanceID());
				trans.GetComponent<SpriteRenderer>().sharedMaterial = oldMat;
			}));
			*/
			Util.SetMaterialAllAttachedBallsTemp(transform,gloopMaterial,duration);
			StartCoroutine(Timers.Countdown(duration,() => glooped = false));

		}
	}




}

