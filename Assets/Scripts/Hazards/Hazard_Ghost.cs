using UnityEngine;
using System.Collections;

public class Hazard_Ghost : Hazard {

	SpriteRenderer spriteRenderer;
	float fadeTime;

	// Use this for initialization
	protected override void Start () {
		//damage=GameVars.damage_spiders;
		//duration = GameVars.duration_spiders;
		base.Start ();
		fadeTime = 0.5f*duration;
		spriteRenderer = GetComponent<SpriteRenderer>();
		Debug.Log ("Duration is "+duration+" fade time is "+fadeTime);
	}

	protected override void Update(){
		if (lifetime<fadeTime)
			spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
		else if (lifetime <= 0)
			spriteRenderer.color = new Color(0.97f,1f,1f, (fadeTime - lifetime)/fadeTime );
		base.Update ();
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			Person2 p = other.transform.GetComponent<Person2>();
			p.Scare (damage);
		}
	}
}
