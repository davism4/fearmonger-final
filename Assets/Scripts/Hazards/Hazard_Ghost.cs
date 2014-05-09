using UnityEngine;
using System.Collections;

public class Hazard_Ghost : Hazard {

	SpriteRenderer spriteRenderer;

	// Use this for initialization
	private void Start () {
		damage=GameVars.damage_spiders;
		duration = GameVars.duration_spiders;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected override void Update(){
		spriteRenderer.color=new Color(0.97f,1f,1f,(duration-timer)/duration);
		base.Update ();
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			Person2 p = other.transform.GetComponent<Person2>();
			p.Scare (damage);
		}
	}
}
