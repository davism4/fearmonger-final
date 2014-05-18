using UnityEngine;
using System.Collections;

public class Person2 : MonoBehaviour {

	protected Animator anim;

	public bool IS_FACING_RIGHT=false;
	public bool IS_FACING_LEFT { get { return !IS_FACING_RIGHT; } }

	private float hpBarOffsetY, spriteWidth, spriteHeight;
	[HideInInspector] public bool CanMove=true; // manual movement

	protected int sanityMax;
	public int sanity;
	public float healthPercent {
		get {
			return ((float)sanity)/sanityMax;
		}
	}
	[HideInInspector] public Room room;
//	Game2 game;
	SpriteRenderer spriteRenderer;
	protected GUIText text;
	protected bool isHurt=false, isMessage=false, isMoving=false, isFleeing=false;
	public float hurtCooldown=0f, messageCooldown=0f, walkCooldown=0f, admireCooldown=0f, showHPCooldown=0f, healCooldown=0f;
	private const float hurtTimeMax=1.5f, messageTimeMax=1f, moveTimeMax=2.5f, waitTimeMax=0.5f, showHPCooldownMax=0.2f, healCooldownMax=2.2f;
	protected bool isLeaving=false;
	private bool isPossessed=false;
	private float gravityScale;

	protected int fearDropMin, fearDropMax, moneyDropMin, moneyDropMax;

	// Determined by subclass
	protected float speed;
	protected float speedNormal, speedFast, admireCooldownMax=99f, admireCooldownMin=99f;
	

	
	// PUBLIC FUNCTIONS

	public void DisplayHP(){
		showHPCooldown=showHPCooldownMax;
	}
	
	public virtual void Scare(int damage){
		if (!isHurt){
			sanity -= damage;
			if (sanity<0) {
				sanity=0;
				isFleeing=true;
			}
			isHurt=true;
			hurtCooldown=hurtTimeMax;
			speed=speedFast;
			if (!isPossessed)
				spriteRenderer.color = Color.red;
			// Maybe show some text:
			if (!isPossessed && UnityEngine.Random.value<0.35f){
				if (UnityEngine.Random.value<0.45f)
					text.text=ShockPhrases.Phrase ();
				else
					text.text="!";
			}
			int j = Mathf.Min (UnityEngine.Random.Range (fearDropMin,fearDropMax),damage)+1;
			if (GameVars.pickupFear!=null && j>0){
				for (int k=0;k<j;k++){
					// Use a circular/polygonal pattern
					Instantiate(GameVars.pickupFear,transform.position +
					            0.7f*(new Vector3(Mathf.Cos(k*Mathf.PI/j),Mathf.Sin(k*Mathf.PI/j),-5f)),Quaternion.identity);
				}
			}
			//Debug.Log(text.text);
			isMessage=true;
			messageCooldown=messageTimeMax;
		}
	}
	
	private void DropMoney(){
		if (GameVars.pickupCoin!=null){
			int i = UnityEngine.Random.Range (moneyDropMin,moneyDropMax);
			if (i>0){
				for (int j=0;j<i;j++){
					// use a circular/polygonal pattern
					Instantiate(GameVars.pickupCoin,transform.position +
					            0.7f*(new Vector3(Mathf.Cos(j*Mathf.PI/i),Mathf.Sin(j*Mathf.PI/i),-5f)),Quaternion.identity);
				}
			}
			admireCooldown = UnityEngine.Random.Range (admireCooldownMin,admireCooldownMax);
		}
	}

	// PROTECTED/PRIVATE FUNCTIONS

	protected virtual void Start () {
		speed = speedNormal;
		speedFast = speedNormal * 2f;
		sanity=sanityMax; 
		spriteRenderer=transform.GetComponent<SpriteRenderer>();
		IS_FACING_RIGHT=true;
		if (UnityEngine.Random.value<0.6f){
			StartMoving(); // sometimes they start moving, sometimes they don't
		}
		//healthBar=transform.GetChild (0).GetComponent<GUITexture>();
		text=transform.GetComponent<GUIText>();
//		text = transform.GetComponent<GUIText>();
		text.text="";

		anim = transform.GetComponent<Animator> ();
		if (!anim.enabled)
			anim=null;
		gravityScale = rigidbody2D.gravityScale;
		LoadHPBar ();
	}

	public void SetRoom(Room r){
		admireCooldownMax -= r.quality;
		admireCooldownMin -= r.quality;
		room = r;
		if (admireCooldownMax<12f)
			admireCooldownMax=12f;
		if (admireCooldownMin<8f)
			admireCooldownMin=8f;
	}

		// Update is called once per frame
	protected virtual void Update () {

		/*if (isPossessed){
			spriteRenderer.color = Color.blue;
		} else if (isHurt){
			spriteRenderer.color = Color.red;
		} else {
			spriteRenderer.color = Color.white;
		}*/
		if (isLeaving==true){
			UpdateLeaving ();
		} else {
			UpdateNormal ();
		}
		if (isMessage){
			if (messageCooldown>0) {
				messageCooldown -= Time.deltaTime;
			} else {
				messageCooldown=messageTimeMax;
//				text.text="";
				isMessage=false;
			}

		}

		if (anim!=null){
			anim.SetBool ("walkRight", IS_FACING_RIGHT);
			anim.SetBool ("walkLeft", IS_FACING_LEFT);
		}
	}

	public void SetPossessed(bool onoff){
		isPossessed = onoff;
		rigidbody2D.gravityScale = onoff? 0f : gravityScale;
		spriteRenderer.color = onoff? new Color(0.3f, 0.3f, 0.5f) : Color.white;
	}

	// Not leaving the room
	float dt, dx;
	protected virtual void UpdateNormal(){
		dt = Time.deltaTime;
		if (sanity < sanityMax){
			if (showHPCooldown>0) {
				showHPCooldown -= dt;
			}
			if (healCooldown>0 && !isHurt){
				healCooldown -= dt;
			} else {
				healCooldown= healCooldownMax;
				sanity++;
			}
		}
		if (isHurt){
			if (hurtCooldown>0){ // recovering from hit
				hurtCooldown -= dt;
			} else {
				isHurt=false; // vulnerable again
				//speed = speedNormal;
				if (!isPossessed)
					spriteRenderer.color = Color.white;
				StartMoving ();
			}
		} else {
			if (admireCooldown<=0f){ // people drop cash on intervals, which is offset by room quality
				DropMoney ();
			} else {
				admireCooldown -= dt;
			}
		}

		if (CanMove){
			speed = (isPossessed || isHurt)? speedFast: speedNormal;
			if (isMoving){
				dx = speed*dt;
				if (IS_FACING_RIGHT){
					if (transform.position.x+dx < GameVars.WallRight){
						rigidbody2D.velocity = new Vector2(speed/5,rigidbody2D.velocity.y);
					} else {
						StopMoving ();
					}
				} else {
					if (transform.position.x-dx > GameVars.WallLeft){
						rigidbody2D.velocity = new Vector2(-speed/5,rigidbody2D.velocity.y);
					} else {
						StopMoving();
					}
				}
				if (walkCooldown>0){ // moving -> keep moving
					walkCooldown -= dt;
				} else if (!isHurt) {
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
			if (transform.position.x > room.doorLeft){
				rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
				//transform.position -= new Vector3(speed*dt,0,0);
			} else {
				Exit (false);
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

	public virtual void Exit(bool forced){
		if (!forced && room!=null)
			room.PlayDoorSound();
		if (gameObject !=null)
			DestroyImmediate(gameObject);
	}

	private void OnGUI(){
		if (Time.timeScale<=0) return;
		if (showHPCooldown>0f){
			Vector3 v = Camera.main.WorldToScreenPoint(transform.position);
			GUI.DrawTexture (new Rect(v.x-spriteWidth/2,Screen.height-v.y-hpBarOffsetY,spriteWidth,spriteHeight),GameVars.hpBarRed,ScaleMode.StretchToFill);
			GUI.DrawTexture (new Rect(v.x-spriteWidth/2,Screen.height-v.y-hpBarOffsetY,spriteWidth*healthPercent,spriteHeight),GameVars.hpBarGreen,ScaleMode.StretchToFill);
		}
	}

	private void LoadHPBar(){
		spriteWidth = GameVars.hpBarRed.width*0.1f;
		spriteHeight = GameVars.hpBarRed.height*0.1f;
		hpBarOffsetY = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
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

	public void Leave(){
		//Debug.Log(name+": I AM CHECKING OUT!");
		this.IS_FACING_RIGHT=false;
		this.isLeaving=true;
	}
}
