using UnityEngine;
using System.Collections;

public class Hazard_Possess : Hazard {

	Person2 victim;
	Vector3 rotVector, scaleVector;
	SpriteRenderer sr;
	public float fadeTime, airTime;

	protected override void Start(){
		base.Start ();
		rotVector = Vector3.back*300f;
		sr = GetComponent<SpriteRenderer>();
		fadeTime = duration*0.1f;
		airTime = duration*0.9f;
		scaleVector = transform.localScale/fadeTime;
	}

	public void SetVictim(GameObject g){
		victim = g.GetComponent<Person2>();
		victim.Scare (damage);
		victim.SetPossessed (true);
	}
	
	protected override void Update(){
		if (victim!=null){
			if (lifetime > airTime){
				victim.transform.position += new Vector3(0f,0.9f*Time.deltaTime,0f);
			}
			if (lifetime <= 0f){
				sr.color = new Color (1f, 1f, 1f, 0f);
			} else if (lifetime < fadeTime){
				sr.color = new Color (1f, 1f, 1f, (fadeTime - lifetime)/fadeTime);
				transform.localScale -= scaleVector*Time.deltaTime;
			}

			transform.Rotate (rotVector*Time.deltaTime);

			transform.position = victim.transform.position;
			if (victim.transform.position.x > GameVars.WallRightSoft)
				victim.IS_FACING_RIGHT=false;
			else if (victim.transform.position.x < GameVars.WallLeftSoft)
				victim.IS_FACING_RIGHT=true;
			victim.Scare (1);
			base.Update();
		} else {
			Finish ();
		}
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			other.GetComponent<Person2>().Scare (damage);
		}
	}

	protected override void Finish(){
		if (victim != null) {
			victim.SetPossessed (false);
			victim.Scare (damage);
		}
		base.Finish ();
	}

	// Kind of acts like the monster, kind of overrides person's AI
	/*
	private Person victim;
	private bool isAttached=false;
	private Transform nearestPersonTransform;
	public float distMin, dist, huntTimer=0, huntTimerMax=0.5f;
	private const float runSpeed=3f;
	public Vector3 destination;
	private Vector2 moveDirection;
	private float motionTimer=0f, motionTimerMax; // how long to walk or wait
//	private SpriteRenderer spriteRenderer;
	private ParticleRenderer particleRenderer;

	// Use this for initialization
	protected override void Start () {
		duration = GameVars.duration_possession;
		damage = GameVars.damage_possession;
		base.Start ();
//		spriteRenderer=transform.GetComponent<SpriteRenderer>();
		particleRenderer=transform.GetComponent<ParticleRenderer>();
	}

	private void RestartWalk(){
		motionTimer=0f;
		motionTimerMax = Random.Range(0.5f, 1.5f);
		destination = new Vector3(
			3.85f*UnityEngine.Random.Range(-victim.attentionRadius,victim.attentionRadius)+currentRoom.transform.position.x,
			3.75f*UnityEngine.Random.Range(-victim.attentionRadius,victim.attentionRadius)+currentRoom.transform.position.y,
			0);
		moveDirection = ((Vector2)destination-(Vector2)transform.position).normalized;
		rigidbody2D.velocity = Vector2.zero;
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			victim = other.transform.GetComponent<Person>();
			victim.CanMove=false;
			victim.isPossessed=true;
			isAttached=true;
			//if (spriteRenderer!=null)
			//	spriteRenderer.enabled=false;
			if (particleRenderer!=null)
				particleRenderer.enabled=false;
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (isAttached) {
			victim.transform.position = transform.position; // same depth level
			if (huntTimer<huntTimerMax)
				huntTimer+=GameVars.Tick*Time.deltaTime;
			else {
				huntTimer=0;
				distMin = 999f;
				foreach (Person p in currentRoom.occupants){
					if (p!=null && p.Sanity>0 && p!=victim) {
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
			else if (((Vector2)transform.position-(Vector2)destination).magnitude<0.8f || motionTimer>=motionTimerMax){
				RestartWalk();
			}
			else {
				motionTimer += GameVars.Tick*Time.deltaTime;
			}
		}

	}*/
}
