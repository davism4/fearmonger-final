using UnityEngine;
using System.Collections;

// Let someone click on the lamp -> 3 damage
// Click on the lamp yourself -> 2 damage

public class Lamp_Scary : Lamp {

	float cooldown=0f;

	private new void Update(){
		if (cooldown>0f)
			cooldown -= Time.deltaTime;
	}

	public override void Flip(Person2 p){
		if (isOn){
			light.enabled=false;
		} else {
			light.enabled=true;
		}
		p.Scare(3);
		Flip ();
		
	}


	public override void Flip(){
		if (isOn){
			light.enabled=false;
		} else {
			light.enabled=true;
			cooldown=0.15f;
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Person")){
			Person2 p = other.GetComponent<Person2>();
			if (!isOn){
				p.Interact(this);
				Flip (p);
			}
			p.Scare (1);
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
