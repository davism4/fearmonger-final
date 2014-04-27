using UnityEngine;
using System.Collections;

public class Lamp: Furniture {

	public bool isOn {
		get {return light.enabled; }
	}

	public void Off(){
		light.enabled = false;
	}

	public virtual void Flip(){
		if (isOn){
			light.enabled=false;
		} else {
			light.enabled=true;
		}
	}

	public virtual void Flip(Person2 p){
		Flip ();
		// some animation, maybe
	}

	protected override void OnTriggerEnter2D(Collider2D other){
		if (!isOn){
			if (other.CompareTag("Person")){
				Person2 p = other.transform.GetComponent<Person2>();
				p.Interact (this);
				Flip (p);
			}
		}
	}
}
