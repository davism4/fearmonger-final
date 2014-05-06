using UnityEngine;
using System.Collections;

public class Person2 : MonoBehaviour {

	protected Animator anim;

	public bool IS_FACING_RIGHT=false;
	public bool IS_FACING_LEFT { get { return !IS_FACING_RIGHT; } }

	protected GUITexture healthBar;
	[HideInInspector] public bool CanMove=true; // manual movement

	protected int sanity, sanityMax;
	public float sanityPercent {
		get {
			return (100f*(float)sanity)/sanityMax;
		}
	}
	[HideInInspector] public Room room;
//	Game2 game;
	SpriteRenderer spriteRenderer;
	protected GUIText text;
	protected bool isHurt=false, isMessage=false, isMoving=false, isFleeing=false;
	public float hurtCooldown=0f, messageCooldown=0f, walkCooldown=0f, admireCooldown=0f;
	private const float hurtTimeMax=1.5f, messageTimeMax=1f, moveTimeMax=2.5f, waitTimeMax=0.5f;
	public bool isLeaving=false;
//	private bool isPossessed=false;

	protected int fearDropMin, fearDropMax, moneyDropMin, moneyDropMax;

	// Determined by subclass
	protected float speed;
	protected float speedNormal=3f, speedFast=7f, admireCooldownMax=99f, admireCooldownMin=99f;

	
	// PUBLIC FUNCTIONS

	public void DisplayHP(){
		isMessage=true;
		messageCooldown=messageTimeMax;
		text.text=sanityPercent.ToString ()+"%";
		Debug.Log(text.text);
	}
	
	public virtual void Scare(int damage){
		if (!isHurt){
			sanity -= damage;
			isHurt=true;
			hurtCooldown=hurtTimeMax;
			speed=speedFast;
			// Maybe show some text:
			if (UnityEngine.Random.value<0.35f){
				if (UnityEngine.Random.value<0.45f)
					text.text=ShockPhrases.Phrase ();
				else
					text.text="!";
			} else {
				text.text = "-"+damage;
			}
			int j = Mathf.Min (UnityEngine.Random.Range (fearDropMin,fearDropMax),damage);
			if (room!=null && room.game.pickupFear!=null && j>0){
				for (int k=0;k<j;k++){
					// Use a circular/polygonal pattern
					Instantiate(room.game.pickupFear,transform.position +
					            0.7f*(new Vector3(Mathf.Cos(k*Mathf.PI/j),Mathf.Sin(k*Mathf.PI/j),-2f)),Quaternion.identity);
				}
			}
			//Debug.Log(text.text);
			isMessage=true;
			messageCooldown=messageTimeMax;
		}
	}
	
	private void DropMoney(){
		if (room!=null && room.game.pickupCoin!=null){
			int i = UnityEngine.Random.Range (moneyDropMin,moneyDropMax);
			if (i>0){
				for (int j=0;j<i;j++){
					// use a circular/polygonal pattern
					Instantiate(room.game.pickupCoin,transform.position +
					            0.7f*(new Vector3(Mathf.Cos(j*Mathf.PI/i),Mathf.Sin(j*Mathf.PI/i),-2f)),Quaternion.identity);
				}
			}
			admireCooldown = UnityEngine.Random.Range (admireCooldownMin,admireCooldownMax);
		}
	}

	// PROTECTED/PRIVATE FUNCTIONS

	protected virtual void Start () {
		speed = speedNormal;
		sanity=sanityMax; 
		spriteRenderer=GetComponent<SpriteRenderer>();
		IS_FACING_RIGHT=true;
		if (UnityEngine.Random.value<0.6f){
			StartMoving(); // sometimes they start moving, sometimes they don't
		}
		healthBar=transform.GetChild (0).GetComponent<GUITexture>();
		text=transform.GetChild (0).GetComponent<GUIText>();
//		text = transform.GetComponent<GUIText>();
		text.text="";

		anim = transform.GetComponent<Animator> ();
		if (!anim.enabled)
			anim=null;
	}

	public void SetRoom(Room r){
		admireCooldownMax -= r.quality;
		admireCooldownMin -= r.quality;
		if (admireCooldownMax<12f)
			admireCooldownMax=12f;
		if (admireCooldownMin<8f)
			admireCooldownMin=8f;
	}

		// Update is called once per frame
	protected virtual void Update () {
		if (isHurt){
			spriteRenderer.color=Color.red;
		} else {
			spriteRenderer.color=Color.white;
		}
		if (isFleeing || isLeaving){
			UpdateLeaving ();
		} else {
			UpdateNormal ();
		}
		if (isMessage){
			if (messageCooldown>0) {
				messageCooldown -= Time.deltaTime;
			} else {
				messageCooldown=messageTimeMax;
				text.text="";
				isMessage=false;
			}

		}

		if (anim!=null){
			anim.SetBool ("walkRight", IS_FACING_RIGHT);
			anim.SetBool ("walkLeft", IS_FACING_LEFT);
		}
	}

	// Not leaving the room
	float dt, dx;
	protected virtual void UpdateNormal(){
		dt = Time.deltaTime;
		if (isHurt){
			if (hurtCooldown>0){ // recovering from hit
				hurtCooldown -= dt;
			} else {
				isHurt=false; // vulnerable again
				speed = speedNormal;
				StartMoving ();
			}
		} else {
			if (admireCooldown<=0f){ // people drop cash on intervals, which is offset by room quality
				DropMoney ();
			} else {
				admireCooldown -= Time.deltaTime;
			}
		}
		if (CanMove){
			if (isMoving){
				dx = speed*dt;
				if (IS_FACING_RIGHT){
					if (transform.position.x+dx < GameVars.WallRight){
						rigidbody2D.velocity = new Vector2(speed,rigidbody2D.velocity.y);
					}
				} else if (IS_FACING_LEFT) {
					if (transform.position.x-dx > GameVars.WallLeft){
						rigidbody2D.velocity = new Vector2(-speed,rigidbody2D.velocity.y);
					}
				}
				if (walkCooldown>0){ // moving -> keep moving
					walkCooldown -= dt;
				} else {
					if (!isHurt)
						StopMoving (); // moving -> pause
					if (UnityEngine.Random.value<=0.2f){ // moving -> pause -> turn around
						IS_FACING_RIGHT=!IS_FACING_RIGHT;
					}
				}
			} else {
				if (walkCooldown>0){ // pause -> stay paused
					walkCooldown -= dt;
				} else {
					if (transform.position.x>GameVars.WallRightSoft){
						IS_FACING_RIGHT=false;
					} else if (transform.position.x<GameVars.WallLeftSoft){
						IS_FACING_RIGHT=true;
					}
					StartMoving (); // pause -> move
				}
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other){
		if (other.transform.CompareTag("SideWall")){
			if (rigidbody2D.velocity.x>0){
				IS_FACING_RIGHT=false;
			} else {
				IS_FACING_RIGHT=true;
			}
			/*
			if (transform.position.x>GameVars.WallRightSoft){
				IS_FACING_RIGHT=false;
			} else { // must have bumped into the left wall
				IS_FACING_RIGHT=true;
			}*/
		}
	}

	// When sanity is 0 - exit is on the left side of the room
	protected virtual void UpdateLeaving(){
		if (isHurt){
			if (hurtCooldown>0){ // recovering from hit
				hurtCooldown -= dt;
			} else {
				isHurt=false; // vulnerable again
				speed = speedNormal;
			}
		}
		if (isFleeing){
			speed=speedFast;
		} else { // isLeaving must be true
			speed = speedNormal;
		}
		if (IS_FACING_RIGHT){
			IS_FACING_RIGHT=false;/*
			if (transform.position.x < GameVars.WallRight){
				rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
				//transform.position += new Vector3(dx,0,0);
			} else {
				Exit ();
			}*/
		} else {
			if (transform.position.x > GameVars.WallLeft){
				rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
				//transform.position -= new Vector3(speed*dt,0,0);
			} else {
				Exit ();
			}
		}
	}
	
	// Interact with furniture
	public virtual void Interact(Furniture f){
		if (f is Lamp){
			if (!isHurt && !((Lamp)f).isOn) {
				StopMoving();
				((Lamp)f).Flip (this);
				if (sanity<sanityMax && !(f is Lamp_Scary)){
					sanity++;
				}
			}
		}
	}

	// PRIVATE FUNCTIONS

	protected virtual void Exit(){
		room.PlayDoorSound();
		DestroyImmediate(gameObject);
	}

	// Isn't moving yet -> start moving in a random direction
	private void StartMoving(){
		isMoving=true;
		walkCooldown=UnityEngine.Random.Range (moveTimeMax/2, moveTimeMax);
		if (IS_FACING_RIGHT){
			rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
		} else {
			rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
		}
	}

	// Go to waiting
	protected void StopMoving(){
		isMoving=false;
		rigidbody2D.velocity = new Vector2(0,rigidbody2D.velocity.y);
		walkCooldown=UnityEngine.Random.Range (waitTimeMax/2,waitTimeMax);
	}


}
