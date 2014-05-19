using UnityEngine;
using System.Collections;

/* The monster collides with walls and the floor, but not with people
 * But, the collision layers interact
 * So the monster has to get its own physics for ground detection
 * Fortunately it only hits the ground once
 */

public class Hazard_Monster : Hazard {

	public bool IS_FACING_RIGHT=false;
	public bool IS_FACING_LEFT { get {return !IS_FACING_RIGHT;}}
	private Animator anim;
	private const float speed=9f;
	private float halfwidth;
	private bool grounded=false;
	private float fallspeed=0f;
	private AudioClip roarSound;

	public void Animate(){
		if (anim!=null){
			anim.SetBool ("walkRight", IS_FACING_RIGHT);
			anim.SetBool ("walkLeft", IS_FACING_LEFT);
		}
	}

	// PRIVATE FUNCTIONS

	// Basically the monster just runs back and forth
	protected override void Update(){
		if (!grounded){
			fallspeed -= 0.5f*Physics2D.gravity.y*Time.deltaTime;
			transform.position += fallspeed*Vector3.down*Time.deltaTime;
		}
		if (IS_FACING_RIGHT){
			if (transform.position.x + halfwidth >= GameVars.WallRight){
				IS_FACING_RIGHT=false;
			} else {
				transform.position += speed*Vector3.right*Time.deltaTime;
			}
		} else {
			if (transform.position.x - halfwidth <= GameVars.WallLeft){
				IS_FACING_RIGHT=true;
			} else {
				transform.position += speed*Vector3.left*Time.deltaTime;
			}
		}
		base.Update ();
	}


	protected override void Start(){
		base.Start ();
		halfwidth = transform.GetComponent<BoxCollider2D>().size.x*transform.localScale.x*0.5f;
		//duration = GameVars.duration_monster;
		//damage = GameVars.damage_monster;
		if (transform.position.x < -6f)
			IS_FACING_RIGHT = true;
		else if (transform.position.x >6f)
			IS_FACING_RIGHT = false;
		else 
			IS_FACING_RIGHT = (0.5f>=UnityEngine.Random.value);
		anim = transform.GetComponent<Animator>();
		fadeTime = 0.2f;
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			Person2 p = other.GetComponent<Person2>();
			p.Scare (damage);
		}
		if (!grounded){
			// the monster doesn't jump, so only check this once
			if (other.CompareTag ("Solid") && other.transform.position.y < this.transform.position.y){
				grounded=true;
				fallspeed=0f;
			}
		}
	}

	/*

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		anim = transform.GetComponent<Animator>();
		duration = GameVars.duration_monster;
		damage=GameVars.damage_monster;
		duration = 8f;
		roarSound = Resources.Load<AudioClip>("Sounds/ghost_giggle_3");
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if(soundTimer<soundTimerMax)
			soundTimer+=GameVars.Tick*Time.deltaTime;
		else {
			soundTimer=0f;
			AudioSource.PlayClipAtPoint (roarSound,transform.position);
		}
		// Should find a new target?
		if (huntTimer<huntTimerMax)
			huntTimer+=GameVars.Tick*Time.deltaTime;
		else {
			huntTimer=0;
			distMin = 999f;
			foreach (Person p in currentRoom.occupants){
				if (p!=null && p.Sanity>0) {
					dist = (p.transform.position-this.transform.position).magnitude;
					if (dist<distMin){
						distMin=dist;
						nearestPersonTransform=p.transform;
					}
				}
			}
		}
		if (nearestPersonTransform!=null){
			// A target is chosen -> chase them!
			moveDirection=((Vector2)nearestPersonTransform.position-(Vector2)transform.position).normalized;
			rigidbody2D.velocity=moveDirection*runSpeed;
		}
		else if (currentRoom.occupants.Length==0) { // there's nobody else in the room
			//Debug.LogError("nobody in the room!");
			Finish ();
		}
		Animate ();

	}

	// use this to determine which sprite(s) to use
	private void SetFacingDirection()
	{
		if (Mathf.Abs(rigidbody2D.velocity.y)>Mathf.Abs(rigidbody2D.velocity.x)){
			if (rigidbody2D.velocity.y>0) {
				IS_FACING_UP=true; IS_FACING_DOWN=false;
				IS_FACING_LEFT=false; IS_FACING_RIGHT=false;
			}
			else {
				IS_FACING_UP=false; IS_FACING_DOWN=true;
				IS_FACING_LEFT=false; IS_FACING_RIGHT=false;
			}
		}
		else {
			if (rigidbody2D.velocity.x>0){
				IS_FACING_UP=false; IS_FACING_DOWN=false;
				IS_FACING_LEFT=false; IS_FACING_RIGHT=true;
			}
			else {
				IS_FACING_UP=false; IS_FACING_DOWN=false;
				IS_FACING_LEFT=true; IS_FACING_RIGHT=false;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.CompareTag ("Person")){
			//print ("collision with person!");
			Person p = other.transform.GetComponent<Person>();
			p.Threaten (this);
			p.Damage (damage);
			//Finish ();//
		}
	}

	// Scaring other people (not touching)
	private void OnTriggerEnter2D(Collider2D other){
		if (other.transform.CompareTag ("Person")){
			//print ("triggered with person!");
			Person p = other.transform.GetComponent<Person>();
			p.Threaten (this);
			//p.Damage (1);
		}
	}

*/
}
