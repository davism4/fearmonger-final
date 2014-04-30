using UnityEngine;
using System.Collections;

public class Person2 : MonoBehaviour {

	public bool IS_FACING_RIGHT=false;
	public bool IS_FACING_LEFT { get { return !IS_FACING_RIGHT; } }

	protected GUITexture healthBar;
	[HideInInspector] public bool CanMove=true; // manual movement

	int sanity;
	int sanityMax=20;
	public float sanityPercent {
		get {
			return (100f*(float)sanity)/sanityMax;
		}
	}
	[HideInInspector] public Room room;
	public AudioClip doorSound;
//	Game2 game;
	SpriteRenderer spriteRenderer;
	protected GUIText text;
	private bool isMessage=false, isMoving=false, isFleeing=false;
	public float hurtCooldown=0f, messageCooldown=0f, walkCooldown=0f, admireCooldown=0f;
	private float moveTimeMax=2.5f, waitTimeMax=0.5f;
	private const float hurtTimeMax=1.5f, messageTimeMax=1f;
	protected bool isHurt=false;
//	private bool isPossessed=false;

	protected int fearDropMin=0, fearDropMax=0, moneyDropMin=0, moneyDropMax=0;

	protected float speed;
	protected float speedNormal=3f, speedFast=7f, admireCooldownMax=99f, admireCooldownMin=99f;
//	private UnityEngine.Random random;
	private GameObject pickupCoin, pickupFear;
	
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
			// (Drop fear?)
			//game.AddXP (iTemp1);
			float i = UnityEngine.Random.value; // Determine text response
			if (i<0.35f){ // only show text sometimes
				text.text=ShockPhrases.Phrase ();
			} else if (i<0.5f){
				text.text="!";
			} else {
				text.text = "-"+damage;
			}
			int j = Mathf.Min (UnityEngine.Random.Range (fearDropMin,fearDropMax),damage);
			if (j>0){
				for (int k=0;k<j;k++){
					Instantiate(pickupFear,transform.position +
					            0.7f*(new Vector3(Mathf.Cos(k*Mathf.PI/j),Mathf.Sin(k*Mathf.PI/j),-2f)),Quaternion.identity);
					//Instantiate (pickupFear,transform.position+new Vector3(0.7f*iTemp3,0.7f*iTemp3,-2f),Quaternion.identity);
				}
			}
			//Debug.Log(text.text);
			isMessage=true;
			messageCooldown=messageTimeMax;
		}
	}
	
	private void DropMoney(){
		int i = UnityEngine.Random.Range (moneyDropMin,moneyDropMax);
		if (i>0){
			for (int j=0;j<i;j++){
				Instantiate(pickupCoin,transform.position +
				            0.7f*(new Vector3(Mathf.Cos(j*Mathf.PI/i),Mathf.Sin(j*Mathf.PI/i),-2f)),Quaternion.identity);
				//Instantiate (pickupFear,transform.position+new Vector3(0.7f*iTemp3,0.7f*iTemp3,-2f),Quaternion.identity);
			}
		}
		admireCooldown = UnityEngine.Random.Range (admireCooldownMin,admireCooldownMax);
	}

	// PROTECTED/PRIVATE FUNCTIONS

	protected virtual void Start () {
		speed = speedNormal;
		sanity=sanityMax; 
		spriteRenderer=GetComponent<SpriteRenderer>();
		/*iTemp1 = UnityEngine.Random.Range (1,4);
		if (iTemp1==1){
			IS_FACING_RIGHT=false;
			StartMoving(); // sometimes they start moving, sometimes they don't
		} else if(iTemp1==2){	
			IS_FACING_RIGHT=true; // sometimes they start moving, sometimes they don't
			StartMoving();
		} else if (iTemp1==3){
			IS_FACING_RIGHT=false;
			StopMoving ();
		} else {
			IS_FACING_RIGHT=true;
			StopMoving ();
		}*/
		IS_FACING_RIGHT=true;
		if (UnityEngine.Random.value>0.5f){
			StartMoving ();
		}
		pickupCoin=Resources.Load<GameObject>("Prefabs/PickupCoin");
		pickupFear=Resources.Load<GameObject>("Prefabs/PickupFear");
		healthBar=transform.GetChild (0).GetComponent<GUITexture>();
		text=transform.GetChild (0).GetComponent<GUIText>();
//		text = transform.GetComponent<GUIText>();
		text.text="";
		//game = GameObject.Find ("Main Game").GetComponent<Game2>();
		//random = new UnityEngine.Random();
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
		if (isFleeing){
			UpdateFleeing ();
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
	}

	// Not being scared from the hotel
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
			}
		}
		if (admireCooldown >=0)
			admireCooldown -= Time.deltaTime;
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

	// When sanity is 0
	protected virtual void UpdateFleeing(){
		//dx = speed*dt;
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
				//rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
				transform.position -= new Vector3(speedFast*dt,0,0);
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
			}
		}
	}

	// PRIVATE FUNCTIONS

	private void Exit(){
		if (doorSound!=null)
			AudioSource.PlayClipAtPoint (doorSound, transform.position);
		DestroyImmediate(gameObject);
	}

	/*void DropFear(int amount){
		for (int1=0;int1<amount;int1++){
			//
			Instantiate(pickupFear,transform.position+new Vector3(int1,0,0),Quaternion.identity);
		}
	}*/

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
