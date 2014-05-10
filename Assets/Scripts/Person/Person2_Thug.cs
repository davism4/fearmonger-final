using UnityEngine;
using System.Collections;

public class Person2_Thug : Person2 {

	private float destroyCooldown;
	private const float destroyCooldownMax=1.5f;
	Furniture target=null;
	
	// Use this for initialization
	protected override void Start () {
		moneyDropMin=1;
		moneyDropMax=2;
		fearDropMin=1;
		fearDropMax=2;
		admireCooldownMin=20f;
		admireCooldownMax=25f;
		sanityMax=60;
		speedNormal += UnityEngine.Random.Range (-0.5f,0.05f);
		speedFast += UnityEngine.Random.Range (-0.4f,0.1f);
		destroyCooldown=destroyCooldownMax;
		base.Start ();
	}
	
	public override void Interact(Furniture f){
		if (!(f is Trap)){
			if (UnityEngine.Random.value<(1.0f)/room.NonTrapFurnitureCount ())
				target=f;
		} else {
			base.Interact (f);
		}
	}
	
	public void Reset(){
		sanity=sanityMax;
		destroyCooldown=destroyCooldownMax;
		target=null;
		isHurt=false;
	}
	
	protected override void UpdateNormal(){
		if (room!=null){
			if (room.NonTrapFurnitureCount ()<1){
				isLeaving=true;
			} else if (target!=null){
				if (destroyCooldown>0f){
					destroyCooldown -= Time.deltaTime;
				} else {
					destroyCooldown=destroyCooldownMax;
					Attack (target);
				}
			}
			base.UpdateNormal ();
		}
	}
	
	public override void Exit(bool forced){
		if (!forced && GameVars.IsNight && sanity>0f){
			
		} else {
			base.Exit(forced);
		}
	}
	
	public override void Scare(int s){
		// reset target trap
		target=null;
		destroyCooldown=destroyCooldownMax;
		base.Scare (s);
	}
	
	private void Attack(Furniture t){
		destroyCooldown=destroyCooldownMax;
		if (t.Damage(1)){
			t.Break ();
			target=null;
		}
	}
}
