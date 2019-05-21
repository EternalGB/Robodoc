using UnityEngine;
using System.Collections.Generic;

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

	BallStatus status = BallStatus.NONE;
	
	bool bombsEnabled = true;

	public delegate void BallCollectHandler(GameObject player, GameObject playerPart, GameObject ball);
	public event BallCollectHandler BallCollect;


	Material origMat;
	List<Material> matQueue;

	Settings.ControlType movControls;

	public Transform halo;
	public float haloIncrement, haloDecrement;

	void Start()
	{
		ballPool = PoolManager.Instance.GetPoolByRepresentative(ballPrefab);
		origMat = GetComponent<SpriteRenderer>().sharedMaterial;
		matQueue = new List<Material>();
		matQueue.Add(origMat);
	}

	void Update()
	{
		//to stop new colliders changing the center of mass
		GetComponent<Rigidbody2D>().centerOfMass = Vector2.zero;

		if(!FlagsHelper.IsSet<BallStatus>(status,BallStatus.FROZEN)) {
			object contr;
			if(!Settings.TryGetSetting(Settings.SettingName.MOVCONTROLS, out contr)) {
				movControls = Settings.ControlType.MOUSE;
				Debug.Log ("Couldn't load move controls");
			} else {
				movControls = (Settings.ControlType)contr;
			}

			if(movControls == Settings.ControlType.MOUSE) {
				//move towards the mouse
				
				mousePos = Util.MouseToWorldPos(0);
				if(Vector2.Distance(mousePos,transform.position) >= 2)
					GetComponent<Rigidbody2D>().velocity = (mousePos - transform.position).normalized*speed;
				else {
					GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					transform.position = mousePos;
				}
			} else {
				//use the keyboard
				GetComponent<Rigidbody2D>().velocity = new Vector3(Input.GetAxis("Horizontal")*speed,Input.GetAxis("Vertical")*speed);
			}
		}


		if(bombsEnabled && Input.GetButtonDown("Bomb")) {
			FireBomb();
		}

		//spin left or right
		if(!FlagsHelper.IsSet<BallStatus>(status,BallStatus.SHOCKED)) {
			rotAngle = Mathf.Repeat(rotAngle + Input.GetAxisRaw("Spin")*rotSpeed*Time.deltaTime,360);
			transform.rotation = Quaternion.AngleAxis(rotAngle,Vector3.forward);
			//rigidbody2D.angularVelocity += new Vector3(0,0,rigidbody2D.velocity.x + rigidbody2D.velocity.y)*-0.1f;
		}

		//reduce the size of the halo
		halo.localScale = new Vector3(Mathf.Clamp(halo.localScale.x - haloDecrement*Time.deltaTime,0,1),Mathf.Clamp(halo.localScale.x - haloDecrement*Time.deltaTime,0,1));

		Debug.DrawLine(transform.position,(Vector2)transform.position+GetComponent<Rigidbody2D>().velocity,Color.blue,0f);


	}

	void FixedUpdate()
	{
		if(!playArea.OverlapPoint(transform.position)) {
			if( Vector2.Distance(playArea.transform.position,(Vector2)transform.position + GetComponent<Rigidbody2D>().velocity) >
			   Vector2.Distance(playArea.transform.position,transform.position)) {
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;//rigidbody2D.velocity - (Vector2)transform.position.normalized*rigidbody2D.velocity.magnitude;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(!FlagsHelper.IsSet<BallStatus>(status,BallStatus.GLOOPED)) {
			ContactPoint2D[] cps = col.contacts;
			Transform parent = GetCorrectParent(cps);
			GameObject playerPart = parent.gameObject;
			GameObject collidingBall = col.gameObject;
			if(BallCollect != null)
				BallCollect(this.gameObject,playerPart,collidingBall);
			if(col.gameObject.layer == LayerMask.NameToLayer("GoodBall")) {
				PointBall pb;
				if(pb = col.gameObject.GetComponent<PointBall>()) {
					if(pb.HasSound)
						SoundEffectManager.PlayClipOnce(pb.CollectSound,Vector3.zero,1);
					Vector3 pos = pb.transform.position;
					Quaternion rot = pb.transform.rotation;
					Vector3 scale = pb.transform.localScale;
					Material mat = pb.GetComponent<SpriteRenderer>().sharedMaterial;
					Sprite sprite = pb.GetComponent<SpriteRenderer>().sprite;
					pb.Destroy();
					AttachedBall ball = ballPool.GetPooled().GetComponent<AttachedBall>();

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

					//if we current have any number of statuses, apply them to the new ball
					if(status != BallStatus.NONE) {
						//apply all current status to new ball
						ball.status = status;
						//include all materials (except the original material)
						for(int i = 1; i < matQueue.Count; i++) {
							ball.matQueue.Add(matQueue[i]);
						}
						ball.CheckStatus();
					}
					//Debug.Log ("Creating Attached Ball at " + pos + " parent: " + ball.transform.parent.GetInstanceID());
					
					ScoreCalculator.Instance.ComboUp();
					//recalculate score
					ScoreCalculator.Instance.SetScorePrediction();

				}
			} else if(col.gameObject.layer == LayerMask.NameToLayer("BadBall")) {
				//Debug.Log ("BadBall Collision");
				BadBall bb = col.gameObject.GetComponent<BadBall>();
				bb.ApplyEffect(cps[0].otherCollider.transform);
				if(bb.HasSound) 
					SoundEffectManager.PlayClipOnce(bb.HitSound,Vector3.zero,1);
				
				//recalculate score
				ScoreCalculator.Instance.SetScorePrediction();
			}

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
				if(ball.GetComponent<Rigidbody2D>()) {
					ball.GetComponent<Rigidbody2D>().velocity += (Vector2)ball.transform.position.normalized*bombForce;
				}
			}
			numBombs--;
		}
	}

	public void GetFrozen(float duration, Material frozenMat)
	{
		if(!FlagsHelper.IsSet<BallStatus>(status,BallStatus.FROZEN)) {
			GetComponent<Renderer>().sharedMaterial = frozenMat;
			AddStatus(BallStatus.FROZEN,frozenMat);
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			GetComponent<Rigidbody2D>().isKinematic = true;
			AttachedBall.AddStatusAllAttachedBalls(transform,BallStatus.FROZEN,frozenMat);
			StartCoroutine(Timers.Countdown(duration,() => {
				RemoveStatus(BallStatus.FROZEN,frozenMat);
				GetComponent<Rigidbody2D>().isKinematic = false;
			}));
		}
	}

	public void GetShocked(float duration, Material shockedMat)
	{
		if(!FlagsHelper.IsSet<BallStatus>(status,BallStatus.SHOCKED)) {
			GetComponent<Renderer>().sharedMaterial = shockedMat;
			AddStatus(BallStatus.SHOCKED,shockedMat);
			AttachedBall.AddStatusAllAttachedBalls(transform,BallStatus.SHOCKED,shockedMat);
			StartCoroutine(Timers.Countdown(duration,() => RemoveStatus(BallStatus.SHOCKED,shockedMat)));
		}
	}

	public void GetGlooped(float duration, Material gloopMat)
	{
		if(!FlagsHelper.IsSet<BallStatus>(status,BallStatus.GLOOPED)) {
			GetComponent<Renderer>().sharedMaterial = gloopMat;
			AddStatus(BallStatus.GLOOPED,gloopMat);
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
			AttachedBall.AddStatusAllAttachedBalls(transform,BallStatus.GLOOPED,gloopMat);
			StartCoroutine(Timers.Countdown(duration,() => RemoveStatus(BallStatus.GLOOPED,gloopMat)));
		}
	}

	public void AddStatus(BallStatus newStatus, Material newMat)
	{
		if(newMat != null) {
			GetComponent<Renderer>().sharedMaterial = newMat;
			matQueue.Add(newMat);
		}
		FlagsHelper.Set<BallStatus>(ref status, newStatus);
		CheckStatus();
	}
	
	public void RemoveStatus(BallStatus newStatus, Material toRemove)
	{
		FlagsHelper.Unset<BallStatus>(ref status, newStatus);
		if(toRemove != null && matQueue.Remove(toRemove)) {
			CheckStatus();
		}
		AttachedBall.RemoveStatusAllAttachedBalls(transform,newStatus,toRemove);
	}
	
	void CheckStatus()
	{
		if(matQueue.Count > 0)
			GetComponent<Renderer>().sharedMaterial = matQueue[matQueue.Count-1];
	}

	public bool HasStatus(BallStatus checkStatus)
	{
		return FlagsHelper.IsSet(status,checkStatus);
	}

	public void DisableBombs()
	{
		bombsEnabled = false;
	}

	public void EnableBombs()
	{
		bombsEnabled = true;
	}

	public void GetParticle()
	{
		halo.localScale = Vector3.ClampMagnitude(halo.localScale + new Vector3(haloIncrement,haloIncrement),1);
	}

}

