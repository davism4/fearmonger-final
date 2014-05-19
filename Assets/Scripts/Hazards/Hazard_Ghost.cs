using UnityEngine;
using System.Collections;

public class Hazard_Ghost : Hazard {
	
	float t;
	Vector3 origin;

	// Use this for initialization
	protected override void Start () {
//		Debug.Log ("Duration is "+duration+" fade time is "+fadeTime);
		base.Start ();
		fadeTime = 0.15f*duration;
		origin = transform.position;
	}

	protected override void Update(){
		t += Time.deltaTime;
		transform.position = origin + new Vector3(0,Mathf.Sin (t*3f),0);
		//Debug.Log ("Lifetime: "+lifetime+" fadetime: "+fadeTime);
		base.Update ();
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			Person2 p = other.transform.GetComponent<Person2>();
			p.Scare (damage);
		}
	}
}
