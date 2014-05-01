using UnityEngine;
using System.Collections;

public class Person2_Priest : Person2 {

	private float destroyCooldown;
	private const float destroyCooldownMax=1.5f;
	bool isDestroying=false;


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
		if (UnityEngine.Random.value<(1.0f)/room.trapList.Count){
			isDestroying=true;
		} else {
			base.Interact (f);
		}
	}

	protected override void UpdateNormal(){
		if (room!=null){
			if (room.trapList.Count<1){
				isLeaving=true;
			} else if (isDestroying){
				if (destroyCooldown>0f){
					destroyCooldown -= Time.deltaTime;
				} else {
					destroyCooldown=destroyCooldownMax;
				}
			}
			base.UpdateNormal ();
		}
	}

	protected override void Exit(){
		if (GameVars.IsNight && sanityPercent>0f){

		} else {
			base.Exit();
		}
	}

	public override void Scare(int s){
		// reset target trap
		isDestroying=false;
		destroyCooldown=destroyCooldownMax;
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

