using UnityEngine;
using System.Collections;

public class Hazard_Claw : Hazard {

	public bool IS_FACING_RIGHT=false; // default sprite is facing left
	public bool IS_FACING_LEFT { get {return !IS_FACING_RIGHT; }}

	public Vector2 origin, velocity;
	private const float speed=10f;
	private bool isExtending=true;
	private GameObject target=null;

	// Find closest person and target them
	private void Start () {
		duration = GameVars.duration_claw;
		damage = GameVars.damage_claw;
		float minY=999f, minX=999f, disY, disX;
		origin = new Vector2(transform.position.x, transform.position.y);
		GameObject[] people = GameObject.FindGameObjectsWithTag ("Person");
		// approximates the closest person, usually within the same or closest floor
		foreach (GameObject o in people){
			disY = Mathf.Abs (o.transform.position.y-origin.y);
			if (minY - disY > 4f){
				minY = disY;
				target = o;
				disX = Mathf.Abs (o.transform.position.x-origin.x);
				if (minX > disX){
					minX = disX;
					target = o;
				}
			}

//			dist = ((Vector2)o.transform.position - origin).magnitude;
		}
		if (target.transform.position.x > origin.x){
			IS_FACING_RIGHT=true;
		}
	}
	/*
	private void OnTriggerEnter2D(Collider2D other){
		if (other.transform.CompareTag ("Person")){
			target = other.gameObject;
			isExtending=false;
			target.GetComponent<Person2>().CanMove=false;
		}
	}*/

	protected override void Finish() {
		if (target!=null)
			target.GetComponent<Person2>().CanMove=true;
		base.Finish ();
	}

	private new void Update () {
		if(gameObject==null || target==null || timer>duration){
			Finish ();
		} else {
			if (isExtending) { // extending
				timer += Time.deltaTime;
				velocity = (Vector2)target.transform.position - (Vector2)transform.position;
				if (velocity.magnitude<=0.1f) {
					isExtending=false;
					target.GetComponent<Person2>().Scare (damage);
					target.GetComponent<Person2>().CanMove=false;
				} else {
					transform.position += (Vector3)velocity.normalized*speed*Time.deltaTime;
				}
			}
			else if (timer<=0f){ // retracting
				target.GetComponent<Person2>().Scare (damage);
				Finish ();
			} else {
				timer -= Time.deltaTime;
				velocity = new Vector2(origin.x-transform.position.x,origin.y-transform.position.y);
				Debug.Log (velocity.magnitude);
				if (velocity.magnitude<=-0.1f){
					target.GetComponent<Person2>().Scare (damage);
					Finish ();
				} else {
					velocity = velocity.normalized*speed;
					target.rigidbody2D.velocity = velocity;
					transform.position = new Vector3(target.transform.position.x,target.transform.position.y,transform.position.z);
				}

			}
		}
	}
}
