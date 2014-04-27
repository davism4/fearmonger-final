using UnityEngine;
using System.Collections;

public class Hazard_Darkness : Hazard {

	CircleCollider2D circle;
	//SpriteRenderer renderer;
	Vector3 dtscale, rotvector;
	Vector2 origin;

	// Use this for initialization
	private void Start () {
		origin = (Vector2)transform.position;
		damage = GameVars.damage_darkness;
		duration = GameVars.duration_darkness;
		dtscale = transform.localScale/duration;
		rotvector = Vector3.back*300f;
		circle = transform.GetComponent<CircleCollider2D>();
	}

	// Update is called once per frame
	protected override void Update () {
		if (transform.localScale.x>0f){
			transform.localScale -= dtscale*GameVars.Tick*Time.deltaTime;
			transform.Rotate (rotvector*GameVars.Tick*Time.deltaTime);
		}
		Collider2D[] things = Physics2D.OverlapCircleAll(origin,transform.localScale.x,GameVars.interactLayer);
		foreach (Collider2D other in things){
			if (other.CompareTag("Person")){
				other.GetComponent<Person2>().Scare (damage);
			} else if (other.CompareTag("Lamp")){
				other.GetComponent<Lamp>().Off();
			}
		}
		base.Update();
	}
}
