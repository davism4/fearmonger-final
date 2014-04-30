using UnityEngine;
using System.Collections;

public class Person2_Priest : Person2 {

	[HideInInspector] public bool isDestroying=false;
	private float destroyCooldown;
	private const float destroyCooldownMax=0.5f;

	// Use this for initialization
	protected override void Start () {
		moneyDropMin=0;
		moneyDropMax=0;
		fearDropMin=0;
		fearDropMax=2;
		admireCooldownMin=15f;
		admireCooldownMax=25f;
		speedNormal += UnityEngine.Random.Range (-0.1f,0.06f);
		speedFast += UnityEngine.Random.Range (-0.1f,0f);
		destroyCooldown=destroyCooldownMax;
		base.Start ();
	}

	public override void Interact(Furniture f){
		base.Interact (f);
		if (f is Trap){
			isDestroying=true;
		}
	}

	protected override void UpdateNormal(){
		if (isDestroying){
			//CanMove=false;
			if (destroyCooldown>0f){
				destroyCooldown -= Time.deltaTime;
			} else {
				destroyCooldown=destroyCooldownMax;
			}
		}
		base.UpdateNormal ();
	}

	public override void Scare(int s){
		isDestroying=false;
		base.Scare (s);
	}

	private void HarmTrap(Trap t){
		destroyCooldown=destroyCooldownMax;
		t.durability--;
		if (t.durability<=0){
			t.Break ();
			isDestroying=false;
		}
	}

}

