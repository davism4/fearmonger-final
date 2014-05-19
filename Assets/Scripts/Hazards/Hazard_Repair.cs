using UnityEngine;
using System.Collections;

public class Hazard_Repair : Hazard {
	
	Furniture target;
	private float fixCooldown=0f, fixCooldownMax;
	int remainingRepairs=3;
	float t = 0f;
	
	protected override void Start(){
		base.Start ();
		fadeTime = 0.15f*duration;
		fixCooldownMax = duration/3f;
	}
	
	public void SetTarget(GameObject g){
		target = g.GetComponent<Furniture>();
	}
	
	protected override void Update(){
		t += Time.deltaTime;
		if (target!=null){
			transform.position = target.transform.position + new Vector3(0,Mathf.Sin (3f*t),0);
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
