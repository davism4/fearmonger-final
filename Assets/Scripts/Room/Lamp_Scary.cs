using UnityEngine;
using System.Collections;

// Let someone click on the lamp -> 3 damage
// Click on the lamp yourself -> 2 damage

public class Lamp_Scary : Lamp {

	protected Animator anim;
	public bool inactive=false;
	public bool Used=false;
	public int damage=4;
	float cooldown=0f;

	protected override void Start(){
		anim = transform.GetComponent<Animator> ();
		if (!anim.enabled)
			anim=null;
		base.Start();
	}

	private new void Update(){
		if (cooldown>0f)
			cooldown -= Time.deltaTime;

		if (anim!=null){
			anim.SetBool ("active", Used);
			anim.SetBool ("inactive", inactive);
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Person")){
			Person2 p = other.GetComponent<Person2>();
			if (!isOn){
				p.Interact(this);
			//	Flip (p);
				p.Scare(damage);
			}
		}
	}
	/* // why is this here?
	void OnTriggerStay2D(Collider2D other){
		if (cooldown>0){
			if (other.CompareTag("Person")){
				//Debug.Log("PERSON ALERT");
				Person2 p = other.GetComponent<Person2>();
				p.Scare (2);
				cooldown = -1;
			}
		}
	}*/

}
