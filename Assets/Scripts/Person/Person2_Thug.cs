using UnityEngine;
using System.Collections;

public class Person2_Thug : Person2 {


	private float destroyCooldown;
	private const float destroyCooldownMax=0.5f;
	private Furniture targetFurniture=null;

	// Use this for initialization
	protected override void Start () {
		moneyDropMin=0;
		moneyDropMax=0;
		fearDropMin=0;
		fearDropMax=2;
		admireCooldownMin=15f;
		admireCooldownMax=25f;
		destroyCooldown=destroyCooldownMax;
		speedNormal += UnityEngine.Random.Range (-0.2f,0.05f);
		speedFast += UnityEngine.Random.Range (-0.2f,0.05f);
		base.Start();
	}
	
	public override void Interact(Furniture f){
		base.Interact (f);
		Debug.Log ("Thug interacts with furniture");
		if (!(f is Trap)){
			if (!isHurt && f.durability>0) {
				StopMoving();
				targetFurniture=f;
				GetComponent<SpriteRenderer>().color=Color.green;
			}
		}

	}

	public override void Scare(int s){
		targetFurniture=null;
		base.Scare (s);
	}
	
	protected override void UpdateNormal(){
		if (targetFurniture!=null){
			//CanMove=false;
			if (destroyCooldown>0f){
				destroyCooldown -= Time.deltaTime;
			} else {
				destroyCooldown=destroyCooldownMax;
				HarmFurniture();
			}
		}
		base.UpdateNormal ();
	}
	
	private void HarmFurniture(){
		destroyCooldown=destroyCooldownMax;
		targetFurniture.durability--;
		if (targetFurniture.durability<=0){
			targetFurniture.Break ();
			targetFurniture=null;
			GetComponent<SpriteRenderer>().color=Color.white;
		}
	}
}
