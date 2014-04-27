using UnityEngine;
using System.Collections;

public class Hazard_Spiders : Hazard {

	//private ParticleSystem particles;
	private CircleCollider2D circle;
	private const float maximumRadius=5.0f;

	// Use this for initialization
	private void Start () {
		damage=GameVars.damage_spiders;
		duration = transform.GetComponent<ParticleSystem>().duration;
		//duration = GameVars.duration_spiders;
		circle = transform.GetComponent<CircleCollider2D>();
	//	particles = transform.GetComponent<ParticleSystem>();
		//if (particles.duration != this.duration){
		//	Debug.LogWarning("Spider hazard particle effect has wrong duration!");
		//}
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			//Debug.Log ("Spider collided with person");
			Person2 p = other.transform.GetComponent<Person2>();
			p.Scare (damage);
		}// else {
			//Debug.Log ("Spider collided with non-person");
		//}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (circle.radius<maximumRadius)
			circle.radius += 1.75f*(Time.deltaTime*GameVars.Tick);
	}
}
