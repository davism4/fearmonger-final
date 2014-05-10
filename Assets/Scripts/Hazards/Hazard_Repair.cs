using UnityEngine;
using System.Collections;

public class Hazard_Repair : Hazard {
	
	Furniture target;
	private float fixCooldown=0f, fixCooldownMax;
	int remainingRepairs=3;
	
	protected override void Start(){
		base.Start ();
		fixCooldownMax = duration/3f;
	}
	
	public void SetTarget(GameObject g){
		target = g.GetComponent<Furniture>();
	}
	
	protected override void Update(){
		if (target!=null){
			transform.position = target.transform.position;
			if (fixCooldown >0) {
				fixCooldown -= Time.deltaTime;
			} else if (remainingRepairs>0) {
				fixCooldown = fixCooldownMax;
				remainingRepairs--;
				target.Repair (5);
			}
			base.Update();
		} else {
			Finish ();
		}
	}

	protected override void Finish(){
		if (target!=null){
			if (remainingRepairs==3)
				target.Repair (15);
			else if (remainingRepairs==2)
				target.Repair (10);
			else if (remainingRepairs==1)
				target.Repair (5);
		}
		base.Finish();
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			other.GetComponent<Person2>().Scare (damage);
		}
	}
}
